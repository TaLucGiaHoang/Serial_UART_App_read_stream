///#define SELECT_CASE_1 


using System;
//using System.Collections.Generic;
//using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;



namespace VirtualSerialDevice
{
    public partial class Form1 : Form
    {
        public const int SERIAL_BAUD_RATE = 115200;
        const string UART_LOG_FILE_PATH = "serial.log"; // File to save log from Uart RichTextBox
        const string STATUS_LOG_FILE_PATH = "status.log"; // File to save log from Status RichTextBox
        string REPORT_FILE_PATH = "sensor-datax.csv";
        private int g_index = 0; // sensor-data-0.csv , sensor-data-1.csv , sensor-data-2.csv ...
        private string timeStam;

        private int SIZE_OF_FLOAT_TYPE = 4;  // the size of a float type data from STM32 is 4 bytes
        private int N_DATA_YZ = 2 * 1000; // (Y + Z)*1000
        private int HEADER_LENGTH = 0; // ignore header length
        private int SENSOR_DATA_LENGTH;// = SIZE_OF_FLOAT_TYPE * N_DATA_YZ; // 4 x 2 x 1000
        private int CHECKSUM_LENGTH = 0; // ignore CRC length
        private int FRAME_SIZE;// = HEADER_LENGTH + SENSOR_DATA_LENGTH + CHECKSUM_LENGTH;
#if SELECT_CASE_1
        private int total_bytes = 0;
#else // SELECT_CASE_2
        private int total_bytes = 0;
        private byte[] buffer;// = new byte[FRAME_SIZE];
#endif
        // Timer
        Stopwatch stopWatch = new Stopwatch();
        private static System.Timers.Timer aTimer;
        
        static SerialPort _serialPort;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnClose.Enabled = false; // Disable Close button

            string[] ports = SerialPort.GetPortNames();

            foreach (string port in ports)
            {
                cbSerialPort.Items.Add(port);
            }

            if (ports.Length > 0)
            {
                cbSerialPort.SelectedIndex = 0;
            }
            else
            {
                btnConnect.Enabled = false;
            }
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            _serialPort = new SerialPort();
            //_serialPort = new SerialPort("COM7");
            _serialPort.BaudRate = SERIAL_BAUD_RATE;
            _serialPort.PortName = cbSerialPort.Text;
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            try
            {
                Console.WriteLine("Connect button pressed");

                _serialPort.Open();
                btnConnect.Enabled = false;
                cbSerialPort.Enabled = false;
                btnClose.Enabled = true;

                printStatus("Open serial " + _serialPort.PortName + ", " + _serialPort.BaudRate + ", " + _serialPort.Parity + ", " + _serialPort.DataBits + ", " + _serialPort.StopBits + "\n\n");

                // Get 
                SIZE_OF_FLOAT_TYPE = Convert.ToInt32(txtBx_sizeof_float_type.Text);
                N_DATA_YZ = Convert.ToInt32(txtBx_n_elements.Text);
                HEADER_LENGTH = Convert.ToInt32(txtBx_sizeof_header.Text);
                CHECKSUM_LENGTH = Convert.ToInt32(txtBx_sizeof_checksum.Text);

                // Set
                SENSOR_DATA_LENGTH = SIZE_OF_FLOAT_TYPE * N_DATA_YZ;
                FRAME_SIZE = HEADER_LENGTH + SENSOR_DATA_LENGTH + CHECKSUM_LENGTH;
                txtBx_sizeof_sensor_data.Text = SENSOR_DATA_LENGTH.ToString();
                txtBoxFrameSize.Text = FRAME_SIZE.ToString();

#if SELECT_CASE_1
        total_bytes = 0;
#else // SELECT_CASE_2
                total_bytes = 0;
        buffer = new byte[FRAME_SIZE];
#endif
                // Disable input TextBox
                txtBx_sizeof_float_type.Enabled = false;
                txtBx_n_elements.Enabled = false;
                txtBx_sizeof_header.Enabled = false;
                txtBx_sizeof_sensor_data.Enabled = false;
                txtBx_sizeof_checksum.Enabled = false;
                txtBoxFrameSize.Enabled = false;

        // Delete all files in a directory    
                string[] files = { UART_LOG_FILE_PATH, STATUS_LOG_FILE_PATH , REPORT_FILE_PATH, /*HEX_FILE_PATH, HEX_CSV_FILE_PATH , BIN_FILE_PATH, BIN_CSV_FILE_PATH,*/};
                foreach (string file in files)
                {
                    File.Delete(file);
                    //printStatus($"{file} is deleted.");
                }

                // Reset variables
                //resend_cnt = 0;
                //txtBx_cnt.Text = resend_cnt.ToString();



                // Set Timer
                SetTimer();
                stopWatch.Start();
                lblRunTime.ResetText();
            }
            catch (Exception) { };

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                _serialPort.Close();
                btnConnect.Enabled = true;
                btnClose.Enabled = false;
                cbSerialPort.Enabled = true;

                // Enable input TextBoxs
                txtBx_sizeof_float_type.Enabled = true;
                txtBx_n_elements.Enabled = true;
                txtBx_sizeof_header.Enabled = true;
                txtBx_sizeof_sensor_data.Enabled = true;
                txtBx_sizeof_checksum.Enabled = true;
                txtBoxFrameSize.Enabled = true;

                printStatus("Close serial " + _serialPort.PortName);

                // Stop Timer
                StopTimer();
                stopWatch.Stop();
                stopWatch.Reset();

            }
            catch (Exception) { };
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            richTextBox_console.Clear();
            richTextBox_uart.Clear();
            rTxtBxReport.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SetTextConsole("\nSave all log\n");

            // Save Status RichTextBox
            using (StreamWriter sw = File.CreateText(STATUS_LOG_FILE_PATH))
            {
                sw.Write(richTextBox_console.Text);
            }

            // Save Serial RichTextBox
            using (StreamWriter sw = File.CreateText(UART_LOG_FILE_PATH))
            {
                sw.Write(richTextBox_uart.Text);
            }

            // Save Report RichTextBox
            using (StreamWriter sw = File.CreateText(REPORT_FILE_PATH))
            {
                sw.Write(rTxtBxReport.Text);
            }
        }

        // Callback Handler for richTextBox_uart
        // https://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
        delegate void SetTextUartCallback(string text);
        private void SetTextUart(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.richTextBox_uart.InvokeRequired)
            {
                SetTextUartCallback d = new SetTextUartCallback(SetTextUart);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBox_uart.AppendText(text);

                // Save serial log
                using (StreamWriter sw = File.AppendText(UART_LOG_FILE_PATH))
                {
                    sw.Write(text);
                }
            }
        }

        // Callback Handler for richTextBox_console
        delegate void SetTextConsoleCallback(string text);
        private void SetTextConsole(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.richTextBox_console.InvokeRequired)
            {
                SetTextConsoleCallback d = new SetTextConsoleCallback(SetTextConsole);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBox_console.AppendText(text);
                Console.Write(text); // Debug

                // Save log
                using (StreamWriter sw = File.AppendText(STATUS_LOG_FILE_PATH))
                {
                    sw.Write(text);
                }
            }
        }

        // Callback Handler for rTxtBxReport
        delegate void SetTextReportCallback(string text);
        private void SetTextReport(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.rTxtBxReport.InvokeRequired)
            {
                SetTextReportCallback d = new SetTextReportCallback(SetTextReport);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.rTxtBxReport.AppendText(text);

                // Save report log
                using (StreamWriter sw = File.AppendText(REPORT_FILE_PATH))
                {
                    sw.Write(text);
                }
            }
        }
       
        //// Callback Handler for txtBxExpected
        //delegate void SetTextBoxExpectedCallback(string text);
        //private void SetTextBoxExpected(string text)
        //{
        //    // InvokeRequired required compares the thread ID of the
        //    // calling thread to the thread ID of the creating thread.
        //    // If these threads are different, it returns true.
        //    if (this.txtBxExpected.InvokeRequired)
        //    {
        //        SetTextBoxExpectedCallback d = new SetTextBoxExpectedCallback(SetTextBoxExpected);
        //        this.Invoke(d, new object[] { text });
        //    }
        //    else
        //    {
        //        this.txtBxExpected.Text = text;
        //    }
        //}

        //// Callback Handler for lblWarning
        //delegate void SetTextLabelWarningCallback(string text);
        //private void SetTextLabelWarning(string text)
        //{
        //    // InvokeRequired required compares the thread ID of the
        //    // calling thread to the thread ID of the creating thread.
        //    // If these threads are different, it returns true.
        //    if (this.lblWarning.InvokeRequired)
        //    {
        //        SetTextLabelWarningCallback d = new SetTextLabelWarningCallback(SetTextLabelWarning);
        //        this.Invoke(d, new object[] { text });
        //    }
        //    else
        //    {
        //        this.lblWarning.Text = text;
        //    }
        //}

        // Callback Handler for lblRunTime
        delegate void SetTextLabelRunTimeCallback(string text);
        private void SetTextLabelRunTime(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblRunTime.InvokeRequired)
            {
                SetTextLabelRunTimeCallback d = new SetTextLabelRunTimeCallback(SetTextLabelRunTime);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblRunTime.Text = text;
            }
        }


        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;             
            if (!sp.IsOpen) return;

#if SELECT_CASE_1
            int bytes = sp.BytesToRead; // number of bytes received from UART COM port
            byte[] buffer = new byte[bytes];
            sp.Read(buffer, 0, bytes);
            total_bytes += bytes;
#else // SELECT_CASE_2
            int bytes = sp.BytesToRead; // number of bytes received from UART COM port
            //if (total_bytes + bytes +1 >= buffer.Length)
            //{
            //    total_bytes = 0; //!!!
            //}
            sp.Read(buffer, total_bytes, bytes);
            total_bytes += bytes;
#endif
            // Display origin UART data
            SetTextUart(buffer.ToString());

            // Get timeStamp
            timeStam = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff ");
            // Display to Console RichTextBox
            SetTextConsole(timeStam + "Received bytes: " + bytes + ", total_bytes: " + total_bytes + "\n");

#if SELECT_CASE_1
            float f_sensor_y = 0;
            float f_sensor_z = 0;
            string str = "";
            if (bytes == 8)
            {
                f_sensor_y = System.BitConverter.ToSingle(buffer, 0);
                f_sensor_z = System.BitConverter.ToSingle(buffer, 4);    
            } else
            {
                // receive failed, set sensor data = 0.0
            }

            str = String.Format("{0}\t{1}\n", f_sensor_y, f_sensor_z);
            // Display to Console RichTextBox
            SetTextConsole(str);

            // Display to Report RichTextBox
            SetTextReport(str);
#else // SELECT_CASE_2
            // Receive a completed frame
            if (total_bytes == FRAME_SIZE)
            {
                int sensor_data_length = total_bytes - HEADER_LENGTH - CHECKSUM_LENGTH; // ignore
                
                //byte[] indata_array = { 0x44, 0x9D , 0xE8, 0xA4};
                string str = "";
                REPORT_FILE_PATH = "sensor-data-" + g_index + ".csv";
                SetTextConsole("Save to " + REPORT_FILE_PATH + "\n");
                g_index += 1;
                if (sensor_data_length >= 2*SIZE_OF_FLOAT_TYPE)
                {
                    //buffer.SubArray(0, 2);

                    // Convert Byte array to Float array
                    float[] float_output = convertByteArray2FloatArrary(buffer, 0, sensor_data_length);
                    str = "Received: " + float_output.Length;
                    SetTextConsole(str);
                    for (int i = 0; i < float_output.Length; i+=2)
                    {
                        str = String.Format("{0}\t{1}\n", float_output[i], float_output[i + 1]);
                        //// Display to Console RichTextBox
                        //SetTextConsole(str);

                        // Display to Report RichTextBox
                        SetTextReport(str);
                    }
                    

                }

                total_bytes = 0;
            }
#endif

            //// Save hex string, bin string
            //if (indata.Contains(prefix_str))
            //{
            //    using (StreamWriter sw = File.AppendText(HEX_FILE_PATH))
            //    {
            //        sw.WriteLine(hex_str);
            //    }
            //    using (StreamWriter sw = File.AppendText(BIN_FILE_PATH))
            //    {
            //        sw.WriteLine(bin_str);
            //    }
            //    using (StreamWriter sw = File.AppendText(HEX_CSV_FILE_PATH))
            //    {
            //        for (int i = 0; i < hex_str.Length; i += 2)
            //        {
            //            // Add ", " comma and space separating
            //            // XXXXXXXX -> XX, XX, XX, XX, 
            //            string s1 = hex_str.Substring(i, 2);
            //            sw.Write(s1);
            //            sw.Write(", "); // add separator
            //        }
            //        sw.Write("\n");
            //    }
            //    using (StreamWriter sw = File.AppendText(BIN_CSV_FILE_PATH))
            //    {
            //        for (int i = 0; i < bin_str.Length; i += 2 * 4)
            //        {
            //            // Add ", " comma and space separating
            //            // XXXXXXXX -> XXXX, XXXX, 
            //            string s1 = bin_str.Substring(i, 2 * 4);
            //            sw.Write(s1);
            //            sw.Write(", "); // add separator
            //        }
            //        sw.Write("\n");
            //    }
            //}

        }

        private float[] convertByteArray2FloatArrary(byte[] byte_array, int start_index, int length)
        {
            /* Check size */
            if (length % SIZE_OF_FLOAT_TYPE == 0)
            {
                int number_of_float_elements = length / SIZE_OF_FLOAT_TYPE; // byte[8000],  8000 / 4 = 2000
                float[] float_array = new float[number_of_float_elements];  // float[2000]
                int byte_element_index = start_index;
                for (int i = 0; i < number_of_float_elements; i++)
                {
                    float_array[i] = System.BitConverter.ToSingle(byte_array, byte_element_index);
                    byte_element_index += SIZE_OF_FLOAT_TYPE;
                }
                return float_array;
            } else
            {
                // Print size error
                SetTextConsole("Convert Byte array to Float array failed\n");
                return null;
            }
            return null;
        }



        //private string Hex2Bin_String(string hex_str)
        //{
        //    string bin_str = "";
        //    foreach (char charHex in hex_str.ToUpper().ToCharArray())
        //    {
        //        // Convert a hexadecimal character to an binary string.
        //        bin_str = bin_str + Hex2Bin(charHex);
        //    }
        //    return bin_str;
        //}

        //private string Hex2Bin(char hex)
        //{
        //    switch (hex)
        //    {
        //        case '0': { return "0000"; break; }
        //        case '1': { return "0001"; break; }
        //        case '2': { return "0010"; break; }
        //        case '3': { return "0011"; break; }
        //        case '4': { return "0100"; break; }
        //        case '5': { return "0101"; break; }
        //        case '6': { return "0110"; break; }
        //        case '7': { return "0111"; break; }
        //        case '8': { return "1000"; break; }
        //        case '9': { return "1001"; break; }
        //        case 'A': case 'a': { return "1010"; break; }
        //        case 'B': case 'b': { return "1011"; break; }
        //        case 'C': case 'c': { return "1100"; break; }
        //        case 'D': case 'd': { return "1101"; break; }
        //        case 'E': case 'e': { return "1110"; break; }
        //        case 'F': case 'f': { return "1111"; break; }
        //        default: { return hex.ToString(); break; } // return itself character
        //    }
        //}

        //// swap 2 byte for 4-digit hexdecimal string
        //// retrun null if false
        //private string swapByte(string hex_4_digits_str)
        //{
        //    if (hex_4_digits_str.Length == 4)
        //    {
        //        string low = hex_4_digits_str.Substring(0, 2); // low digit
        //        string high = hex_4_digits_str.Substring(2, 2); // hight digit
        //        return (high + low);
        //    }
        //    else return null;
        //}

        private void printStatus(string str)
        {
            // Print timeStamp
            // https://stackoverflow.com/questions/21953090/c-sharp-time-stamp-issue
            SetTextConsole(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff "));
            // Print status
            SetTextConsole(str + "\n");
        }

        //
        // System Timer
        //
        private void SetTimer()
        {
            // Create a timer with a 100 milisecond interval.
            aTimer = new System.Timers.Timer(100);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void StopTimer()
        {
            aTimer.Stop();
            aTimer.Dispose();
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
            //SetTextLabelRunTime(string.Format("{0:HH:mm:ss}", e.SignalTime)); // {0:HH:mm:ss.fff} // display current time

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            //Console.WriteLine("RunTime " + elapsedTime);
            SetTextLabelRunTime(elapsedTime);
        }

    }
}

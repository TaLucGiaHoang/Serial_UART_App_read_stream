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
        string DELETE_REPORT_FILE_PATH = @"sensor-data*.csv";
        private int g_index = 0; // sensor-data-0.csv , sensor-data-1.csv , sensor-data-2.csv ...
        private string timeStam;

        private int SIZE_OF_FLOAT_TYPE;// = 4;  // the size of a float type data from STM32 is 4 bytes
        private int N_DATA_YZ;// = 2 * 1000; // (Y + Z)*1000
        private int HEADER_LENGTH;// = 0; // ignore header length
        private int SENSOR_DATA_LENGTH;// = SIZE_OF_FLOAT_TYPE * N_DATA_YZ; // 4 x 2 x 1000
        private int CHECKSUM_LENGTH;// = 0; // ignore CRC length
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
                SIZE_OF_FLOAT_TYPE = Convert.ToInt32(txtBxElementSize.Text);
                N_DATA_YZ = Convert.ToInt32(txtBxNElements.Text);
                HEADER_LENGTH = Convert.ToInt32(txtBxHeaderSize.Text);
                CHECKSUM_LENGTH = Convert.ToInt32(txtBxChecksumSize.Text);

                // Set
                SENSOR_DATA_LENGTH = SIZE_OF_FLOAT_TYPE * N_DATA_YZ;
                FRAME_SIZE = HEADER_LENGTH + SENSOR_DATA_LENGTH + CHECKSUM_LENGTH;
                txtBxDataSize.Text = SENSOR_DATA_LENGTH.ToString();
                txtBoxFrameSize.Text = FRAME_SIZE.ToString();

#if SELECT_CASE_1
                total_bytes = 0;
#else // SELECT_CASE_2
                total_bytes = 0;
                buffer = new byte[FRAME_SIZE];
#endif
                // Disable input TextBox
                txtBxElementSize.Enabled = false;
                txtBxNElements.Enabled = false;
                txtBxHeaderSize.Enabled = false;
                txtBxChecksumSize.Enabled = false;

                // Delete all files in a directory    
                string[] files = { UART_LOG_FILE_PATH, STATUS_LOG_FILE_PATH , REPORT_FILE_PATH, /*HEX_FILE_PATH, HEX_CSV_FILE_PATH , BIN_FILE_PATH, BIN_CSV_FILE_PATH,*/};

                foreach (string file in files)
                {
                    File.Delete(file);
                    //printStatus($"{file} is deleted.");
                }

                string rootFolderPath = @".";
                string filesToDelete = DELETE_REPORT_FILE_PATH; // @"sensor-data*.csv"; // Only delete .csv files containing "sensor-data" in their filenames
                string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                foreach (string file in fileList)
                {
                    System.Diagnostics.Debug.WriteLine(file + "will be deleted");
                    System.IO.File.Delete(file);
                }

                // Reset global variables
                g_index = 0;

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
                txtBxElementSize.Enabled = true;
                txtBxNElements.Enabled = true;
                txtBxHeaderSize.Enabled = true;
                txtBxChecksumSize.Enabled = true;

                printStatus("Close serial " + _serialPort.PortName);

                // Reset global variables
                //g_index = 0;

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

        // Callback Handler for txtBxDataSize
        delegate void SetTextBoxDataSizeCallback(string text);
        private void SetTextBoxDataSize(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtBxDataSize.InvokeRequired)
            {
                SetTextBoxDataSizeCallback d = new SetTextBoxDataSizeCallback(SetTextBoxDataSize);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtBxDataSize.Text = text;
            }
        }

        // Callback Handler for txtBoxFrameSize
        delegate void SetTextBoxFrameSizeCallback(string text);
        private void SetTextBoxFrameSize(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtBoxFrameSize.InvokeRequired)
            {
                SetTextBoxFrameSizeCallback d = new SetTextBoxFrameSizeCallback(SetTextBoxFrameSize);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtBoxFrameSize.Text = text;
            }
        }

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

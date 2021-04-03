//#define SELECT_CASE_1 


using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
//using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;



namespace VirtualSerialDevice
{
    public partial class Form1 : Form
    {
        //enum SensorType
        //{
        //    AD_TEMP_HUM,
        //    AD_ACCEL,
        //    AD_NONE,
        //}

        public const int SERIAL_BAUD_RATE = 115200;
        const string UART_LOG_FILE_PATH = "serial.log"; // File to save log from Uart RichTextBox
        const string STATUS_LOG_FILE_PATH = "status.log"; // File to save log from Status RichTextBox
        //const string HEX_FILE_PATH = "hex.txt";
        //const string HEX_CSV_FILE_PATH = "hex.csv";
        //const string BIN_FILE_PATH = "bin.txt";
        //const string BIN_CSV_FILE_PATH = "bin.csv";
        const string REPORT_FILE_PATH = "sensor-data.csv";
        //private int data_type_flag = (int)SensorType.AD_NONE;
        private string timeStam;

        //// Offset header
        //private const int _MUN_CODE_ = 0; // 2-byte length
        //private const int _STATUS_ = _MUN_CODE_ + (2 * 2); // 2-byte length
        //private const int _SENSOR_DATA_ = _STATUS_ + (2 * 2); // 14-byte or 22-byte length (USER DATA)

        //// Sensor value - User data
        //private const int _ACCEL_X1_ = 0; // 2-byte length . _SENSOR_DATA_ start
        //private const int _ACCEL_Y1_ = _ACCEL_X1_ + (2 * 2); // 2-byte length
        //private const int _ACCEL_Z1_ = _ACCEL_Y1_ + (2 * 2); // 2-byte length
        //private const int _ACCEL_X2_ = _ACCEL_Z1_ + (2 * 2); // 2-byte length
        //private const int _ACCEL_Y2_ = _ACCEL_X2_ + (2 * 2); // 2-byte length
        //private const int _ACCEL_Z2_ = _ACCEL_Y2_ + (2 * 2); // 2-byte length
        //private const int _ACCEL_X3_ = _ACCEL_Z2_ + (2 * 2); // 2-byte length
        //private const int _ACCEL_Y3_ = _ACCEL_X3_ + (2 * 2); // 2-byte length
        //private const int _ACCEL_Z3_ = _ACCEL_Y3_ + (2 * 2); // 2-byte length
        //private const int _ACCEL_LSEQ_ = _ACCEL_Z3_ + (2 * 2); // 2-byte length
        //private const int _FRAME_LEN_ACCEL_ = _ACCEL_LSEQ_ + (2 * 2); // AD-ACCEL frame length

        //// Sensor value - User data
        //private const int _AD1_ = 0; // 2-byte length , _SENSOR_DATA_ start
        //private const int _AD2_ = _AD1_ + (2 * 2); // 2-byte length
        //private const int _AD_H = _AD2_ + (2 * 2); // 2-byte length
        //private const int _ADTH_LSEQ_ = _AD_H + (2 * 2); // 2-byte length
        //private const int _LEN_ = _ADTH_LSEQ_ + (2 * 2); // 1-byte length
        //private const int _TYPE_ = _LEN_ + (1 * 2); // 1-byte length
        //private const int _DEVICE_NAME_ = _TYPE_ + (1 * 2); // 7-byte length
        //private const int _FRAME_LEN_ADTH_ = _DEVICE_NAME_ + (7 * 2); // AD-TEMP and AD-HUM frame length
        

        // Report lost packet variables
        private int RESEND_NUM = 0; // 3 get from TextBox
        private int TOTAL_PACK_NUM = 0;  // 8 get from TextBox
        private int SEQ_NUM_RANGE = 0;  // [0:4] get from TextBox
        private int resend_cnt = 0;
        //private int expected_pack_num = 0;
        //private int prev_pack_num = 0;
        //private int prev_lseq = 0;
        //private int prev_seq_num = 0;
        //private string prev_str = "";
        //private int total_lost_packet_num = 0;
        //private int total_recv_packet_num = 0;

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

                //// Get Packet varibles
                //RESEND_NUM = Convert.ToInt32(txtBx_ReSendNum.Text);
                //TOTAL_PACK_NUM = Convert.ToInt32(txtBx_TotalPacketNum.Text);
                //SEQ_NUM_RANGE = Convert.ToInt32(txtBxSeqNum.Text);
                //SetTextConsole("\n" + lbl_resend.Text + RESEND_NUM);
                //SetTextConsole("\n" + lbl_package.Text + TOTAL_PACK_NUM);
                //SetTextConsole("\n" + label11.Text + SEQ_NUM_RANGE);
                //SetTextConsole("\n" + label1.Text + txtBxPrefix.Text);

                //// Disable input TextBox
                //txtBx_ReSendNum.Enabled = false;
                //txtBx_TotalPacketNum.Enabled = false;
                //txtBxSeqNum.Enabled = false;
                //txtBxPrefix.Enabled = false;

                // Delete all files in a directory    
                string[] files = { UART_LOG_FILE_PATH, STATUS_LOG_FILE_PATH , REPORT_FILE_PATH, /*HEX_FILE_PATH, HEX_CSV_FILE_PATH , BIN_FILE_PATH, BIN_CSV_FILE_PATH,*/};
                foreach (string file in files)
                {
                    File.Delete(file);
                    //printStatus($"{file} is deleted.");
                }

                // Reset variables
                resend_cnt = 0;
                txtBx_cnt.Text = resend_cnt.ToString();
                //prev_pack_num = 0;
                txtBoxPacket.ResetText();
                //expected_pack_num = 0;
                txtBxExpected.ResetText();
                lblWarning.ResetText();
                //total_lost_packet_num = 0;
                //txtBxTotalLost.Text = total_lost_packet_num.ToString();
                lblTotalLostPercent.ResetText();
                //total_recv_packet_num = 0;
                //txtBxTotalRecv.Text = total_recv_packet_num.ToString();
                lblTotalReceivedPercent.ResetText();
                //prev_lseq = 0;
                //prev_seq_num = 0;
                //prev_str = "";

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
                txtBxPrefix.Enabled = true;
                txtBx_ReSendNum.Enabled = true;
                txtBx_TotalPacketNum.Enabled = true;
                txtBxSeqNum.Enabled = true;

                printStatus("Close serial " + _serialPort.PortName);

                // Stop Timer
                StopTimer();
                stopWatch.Stop();
                stopWatch.Reset();

                //// Display to Report RichTextBox
                //if(total_recv_packet_num > 0)
                //{
                //    SetTextReport("Total Received Packet: " + total_recv_packet_num + ", Total Lost Packet: " + total_lost_packet_num);
                //}
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

        // Callback Handler for txtBx_cnt
        delegate void SetTextBoxCntCallback(string text);
        private void SetTextBoxCnt(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtBx_cnt.InvokeRequired)
            {
                SetTextBoxCntCallback d = new SetTextBoxCntCallback(SetTextBoxCnt);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtBx_cnt.Text = text;
            }
        }

        // Callback Handler for txtBx_cnt
        delegate void SetTextBoxPacketCallback(string text);
        private void SetTextBoxPacket(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtBoxPacket.InvokeRequired)
            {
                SetTextBoxPacketCallback d = new SetTextBoxPacketCallback(SetTextBoxPacket);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtBoxPacket.Text = text;
            }
        }
       
        // Callback Handler for txtBxExpected
        delegate void SetTextBoxExpectedCallback(string text);
        private void SetTextBoxExpected(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtBxExpected.InvokeRequired)
            {
                SetTextBoxExpectedCallback d = new SetTextBoxExpectedCallback(SetTextBoxExpected);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtBxExpected.Text = text;
            }
        }

        // Callback Handler for lblWarning
        delegate void SetTextLabelWarningCallback(string text);
        private void SetTextLabelWarning(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblWarning.InvokeRequired)
            {
                SetTextLabelWarningCallback d = new SetTextLabelWarningCallback(SetTextLabelWarning);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblWarning.Text = text;
            }
        }

        // Callback Handler for txtBxTotalLost
        delegate void SetTextBoxTotalLostCallback(string text);
        private void SetTextBoxTotalLost(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtBxTotalLost.InvokeRequired)
            {
                SetTextBoxTotalLostCallback d = new SetTextBoxTotalLostCallback(SetTextBoxTotalLost);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtBxTotalLost.Text = text;
            }
        }

        // Callback Handler for txtBxTotalRecv
        delegate void SetTextBoxTotalRecviveCallback(string text);
        private void SetTextBoxTotalRecvive(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtBxTotalRecv.InvokeRequired)
            {
                SetTextBoxTotalRecviveCallback d = new SetTextBoxTotalRecviveCallback(SetTextBoxTotalRecvive);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txtBxTotalRecv.Text = text;
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

        // Callback Handler for lblTotalLostPercent
        delegate void SetTextLabelLostPercentCallback(string text);
        private void SetTextLabelLostPercent(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblTotalLostPercent.InvokeRequired)
            {
                SetTextLabelLostPercentCallback d = new SetTextLabelLostPercentCallback(SetTextLabelLostPercent);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblTotalLostPercent.Text = text;
            }
        }

        // Callback Handler for lblTotalReceivedPercent
        delegate void SetTextLabelRecvPercentCallback(string text);
        private void SetTextLabelRecvPercent(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblTotalReceivedPercent.InvokeRequired)
            {
                SetTextLabelRecvPercentCallback d = new SetTextLabelRecvPercentCallback(SetTextLabelRecvPercent);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblTotalReceivedPercent.Text = text;
            }
        }

        private const int SIZE_OF_FLOAT_TYPE = 4;  // the size of a float type data from STM32 is 4 bytes
        private const int N_DATA_YZ = 1000;
        private const int HEADER_LENGTH = 0; // ignore header length
        private const int SENSOR_DATA_LENGTH = SIZE_OF_FLOAT_TYPE * 2 * N_DATA_YZ; // 4 x 2 x 1000
        private const int CHECKSUM_LENGTH = 0; // ignore CRC length
        private const int FRAME_SIZE = HEADER_LENGTH + SENSOR_DATA_LENGTH + CHECKSUM_LENGTH;
        
        private int total_bytes = 0;
#if SELECT_CASE_1
#else // SELECT_CASE_2
        private byte[] buffer = new byte[FRAME_SIZE];
#endif
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

            str = String.Format("{0} , {1},\n", f_sensor_y, f_sensor_z);
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

                if (sensor_data_length >= 2*SIZE_OF_FLOAT_TYPE)
                {
                    //buffer.SubArray(0, 2);

                    // Convert Byte array to Float array
                    float[] float_output = convertByteArray2FloatArrary(buffer, 0, sensor_data_length);
                    str = "Len: " + float_output.Length;
                    for(int i = 0; i < float_output.Length; i+=2)
                    {
                        str = String.Format("{0} , {1},\n", float_output[i], float_output[i + 1]);
                        //// Display to Console RichTextBox
                        //SetTextConsole(str);

                        // Display to Report RichTextBox
                        SetTextReport(str);
                    }
                    SetTextConsole(str);

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


        /// Functions for Luan Van Tot Nghiep
        /// ////
        /// 
        // Function cho Luan Van Tot Nghiep

        /*
         * SIZE_OF_FLOAT_TYPE = 4 (bytes)
         */
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

        /// End of Functions for Luan Van Tot Nghiep
        /// ////
        /// 

        private string analyze(string hex_str)
        {
            //// Calculate lost/received percentage
            //float lost_percent = (float)total_lost_packet_num / (float)(total_lost_packet_num + total_recv_packet_num);
            //float recv_percent = (float)total_recv_packet_num / (float)(total_lost_packet_num + total_recv_packet_num);
            //string lost_percent_str = String.Format("{0:P2}", lost_percent); // example: 12.34%
            //string recv_percent_str = String.Format("{0:P2}", recv_percent);

            //// Display to Text Box
            //SetTextBoxCnt(resend_cnt.ToString());
            //SetTextBoxExpected(expected_pack_num.ToString());
            //SetTextBoxPacket(packet_num.ToString());
            //SetTextBoxTotalRecvive(total_recv_packet_num.ToString());
            //SetTextBoxTotalLost(total_lost_packet_num.ToString());

            //// Display to Label
            //SetTextLabelLostPercent(lost_percent_str);
            //SetTextLabelRecvPercent(recv_percent_str);

            // Create Report log


            //// Display to Report RichTextBox
            //SetTextReport(report_str + "\n");

            // Save to previous variables

            // Set return string

            return "";
        }

        private string Hex2Bin_String(string hex_str)
        {
            string bin_str = "";
            foreach (char charHex in hex_str.ToUpper().ToCharArray())
            {
                // Convert a hexadecimal character to an binary string.
                bin_str = bin_str + Hex2Bin(charHex);
            }
            return bin_str;
        }

        private string Hex2Bin(char hex)
        {
            switch (hex)
            {
                case '0': { return "0000"; break; }
                case '1': { return "0001"; break; }
                case '2': { return "0010"; break; }
                case '3': { return "0011"; break; }
                case '4': { return "0100"; break; }
                case '5': { return "0101"; break; }
                case '6': { return "0110"; break; }
                case '7': { return "0111"; break; }
                case '8': { return "1000"; break; }
                case '9': { return "1001"; break; }
                case 'A': case 'a': { return "1010"; break; }
                case 'B': case 'b': { return "1011"; break; }
                case 'C': case 'c': { return "1100"; break; }
                case 'D': case 'd': { return "1101"; break; }
                case 'E': case 'e': { return "1110"; break; }
                case 'F': case 'f': { return "1111"; break; }
                default: { return hex.ToString(); break; } // return itself character
            }
        }

        // swap 2 byte for 4-digit hexdecimal string
        // retrun null if false
        private string swapByte(string hex_4_digits_str)
        {
            if (hex_4_digits_str.Length == 4)
            {
                string low = hex_4_digits_str.Substring(0, 2); // low digit
                string high = hex_4_digits_str.Substring(2, 2); // hight digit
                return (high + low);
            }
            else return null;
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

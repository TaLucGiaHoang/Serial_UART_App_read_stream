namespace VirtualSerialDevice
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.cbSerialPort = new System.Windows.Forms.ComboBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.richTextBox_uart = new System.Windows.Forms.RichTextBox();
            this.txtBxHeaderSize = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox_console = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lbl_sizeof_float_type = new System.Windows.Forms.Label();
            this.lbl_n_elements = new System.Windows.Forms.Label();
            this.txtBxElementSize = new System.Windows.Forms.TextBox();
            this.txtBxNElements = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBoxFrameSize = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.rTxtBxReport = new System.Windows.Forms.RichTextBox();
            this.lblRunTime = new System.Windows.Forms.Label();
            this.txtBxChecksumSize = new System.Windows.Forms.TextBox();
            this.label_sizeof_checksum = new System.Windows.Forms.Label();
            this.txtBxDataSize = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbSerialPort
            // 
            this.cbSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSerialPort.FormattingEnabled = true;
            this.cbSerialPort.Location = new System.Drawing.Point(485, 83);
            this.cbSerialPort.Name = "cbSerialPort";
            this.cbSerialPort.Size = new System.Drawing.Size(99, 21);
            this.cbSerialPort.TabIndex = 0;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(590, 82);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // richTextBox_uart
            // 
            this.richTextBox_uart.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.richTextBox_uart.Location = new System.Drawing.Point(485, 150);
            this.richTextBox_uart.Name = "richTextBox_uart";
            this.richTextBox_uart.Size = new System.Drawing.Size(417, 138);
            this.richTextBox_uart.TabIndex = 3;
            this.richTextBox_uart.Text = "";
            this.richTextBox_uart.WordWrap = false;
            // 
            // txtBxHeaderSize
            // 
            this.txtBxHeaderSize.Location = new System.Drawing.Point(327, 72);
            this.txtBxHeaderSize.Name = "txtBxHeaderSize";
            this.txtBxHeaderSize.Size = new System.Drawing.Size(51, 20);
            this.txtBxHeaderSize.TabIndex = 5;
            this.txtBxHeaderSize.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(217, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Size of Header:";
            // 
            // richTextBox_console
            // 
            this.richTextBox_console.Location = new System.Drawing.Point(12, 320);
            this.richTextBox_console.Name = "richTextBox_console";
            this.richTextBox_console.Size = new System.Drawing.Size(890, 166);
            this.richTextBox_console.TabIndex = 7;
            this.richTextBox_console.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 304);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Status:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(482, 134);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "COM Terminal";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(669, 82);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lbl_sizeof_float_type
            // 
            this.lbl_sizeof_float_type.AutoSize = true;
            this.lbl_sizeof_float_type.Location = new System.Drawing.Point(12, 47);
            this.lbl_sizeof_float_type.Name = "lbl_sizeof_float_type";
            this.lbl_sizeof_float_type.Size = new System.Drawing.Size(83, 13);
            this.lbl_sizeof_float_type.TabIndex = 12;
            this.lbl_sizeof_float_type.Text = "Size of Element:";
            // 
            // lbl_n_elements
            // 
            this.lbl_n_elements.AutoSize = true;
            this.lbl_n_elements.Location = new System.Drawing.Point(12, 72);
            this.lbl_n_elements.Name = "lbl_n_elements";
            this.lbl_n_elements.Size = new System.Drawing.Size(105, 13);
            this.lbl_n_elements.TabIndex = 13;
            this.lbl_n_elements.Text = "Number of Elements:";
            // 
            // txtBxElementSize
            // 
            this.txtBxElementSize.Location = new System.Drawing.Point(122, 44);
            this.txtBxElementSize.Name = "txtBxElementSize";
            this.txtBxElementSize.Size = new System.Drawing.Size(51, 20);
            this.txtBxElementSize.TabIndex = 14;
            this.txtBxElementSize.Text = "4";
            // 
            // txtBxNElements
            // 
            this.txtBxNElements.Location = new System.Drawing.Point(122, 69);
            this.txtBxNElements.Name = "txtBxNElements";
            this.txtBxNElements.Size = new System.Drawing.Size(51, 20);
            this.txtBxNElements.TabIndex = 15;
            this.txtBxNElements.Text = "2000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(217, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Size of Frame:";
            // 
            // txtBoxFrameSize
            // 
            this.txtBoxFrameSize.Enabled = false;
            this.txtBoxFrameSize.Location = new System.Drawing.Point(327, 98);
            this.txtBoxFrameSize.Name = "txtBoxFrameSize";
            this.txtBoxFrameSize.Size = new System.Drawing.Size(51, 20);
            this.txtBoxFrameSize.TabIndex = 19;
            this.txtBoxFrameSize.Text = "8000";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(827, 82);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save Log";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(749, 82);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 21;
            this.btnClear.Text = "Clear Log";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Values of Sensor: (Y,Z)";
            // 
            // rTxtBxReport
            // 
            this.rTxtBxReport.Location = new System.Drawing.Point(12, 150);
            this.rTxtBxReport.Name = "rTxtBxReport";
            this.rTxtBxReport.Size = new System.Drawing.Size(464, 139);
            this.rTxtBxReport.TabIndex = 23;
            this.rTxtBxReport.Text = "";
            this.rTxtBxReport.WordWrap = false;
            // 
            // lblRunTime
            // 
            this.lblRunTime.AutoSize = true;
            this.lblRunTime.Location = new System.Drawing.Point(482, 51);
            this.lblRunTime.Name = "lblRunTime";
            this.lblRunTime.Size = new System.Drawing.Size(19, 13);
            this.lblRunTime.TabIndex = 29;
            this.lblRunTime.Text = "    ";
            // 
            // txtBxChecksumSize
            // 
            this.txtBxChecksumSize.Location = new System.Drawing.Point(327, 46);
            this.txtBxChecksumSize.Name = "txtBxChecksumSize";
            this.txtBxChecksumSize.Size = new System.Drawing.Size(51, 20);
            this.txtBxChecksumSize.TabIndex = 33;
            this.txtBxChecksumSize.Text = "0";
            // 
            // label_sizeof_checksum
            // 
            this.label_sizeof_checksum.AutoSize = true;
            this.label_sizeof_checksum.Location = new System.Drawing.Point(217, 49);
            this.label_sizeof_checksum.Name = "label_sizeof_checksum";
            this.label_sizeof_checksum.Size = new System.Drawing.Size(95, 13);
            this.label_sizeof_checksum.TabIndex = 34;
            this.label_sizeof_checksum.Text = "Size of Checksum:";
            // 
            // txtBxDataSize
            // 
            this.txtBxDataSize.Enabled = false;
            this.txtBxDataSize.Location = new System.Drawing.Point(122, 98);
            this.txtBxDataSize.Name = "txtBxDataSize";
            this.txtBxDataSize.Size = new System.Drawing.Size(51, 20);
            this.txtBxDataSize.TabIndex = 18;
            this.txtBxDataSize.Text = "8000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Size of Sensor Data:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 498);
            this.Controls.Add(this.label_sizeof_checksum);
            this.Controls.Add(this.txtBxChecksumSize);
            this.Controls.Add(this.lblRunTime);
            this.Controls.Add(this.rTxtBxReport);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtBoxFrameSize);
            this.Controls.Add(this.txtBxDataSize);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBxNElements);
            this.Controls.Add(this.txtBxElementSize);
            this.Controls.Add(this.lbl_n_elements);
            this.Controls.Add(this.lbl_sizeof_float_type);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox_console);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBxHeaderSize);
            this.Controls.Add(this.richTextBox_uart);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.cbSerialPort);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Virtual Serial Device";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ComboBox cbSerialPort;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox richTextBox_uart;
        private System.Windows.Forms.RichTextBox richTextBox_console;
        private System.Windows.Forms.TextBox txtBxHeaderSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lbl_sizeof_float_type;
        private System.Windows.Forms.Label lbl_n_elements;
        private System.Windows.Forms.TextBox txtBxElementSize;
        private System.Windows.Forms.TextBox txtBxNElements;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBoxFrameSize;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RichTextBox rTxtBxReport;
        private System.Windows.Forms.Label lblRunTime;
        private System.Windows.Forms.TextBox txtBxChecksumSize;
        private System.Windows.Forms.Label label_sizeof_checksum;
        private System.Windows.Forms.TextBox txtBxDataSize;
        private System.Windows.Forms.Label label5;
    }
}


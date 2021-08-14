namespace RecViewer
{
    partial class ReadData
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.btnRead = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.tbsend = new System.Windows.Forms.TextBox();
            this.tbread = new System.Windows.Forms.TextBox();
            this.tbreadno = new System.Windows.Forms.TextBox();
            this.tbTimeout = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rb099 = new System.Windows.Forms.RadioButton();
            this.rb0999 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "串口";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(101, 26);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "波特率";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "9600",
            "115200"});
            this.comboBox2.Location = new System.Drawing.Point(101, 60);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 20);
            this.comboBox2.TabIndex = 3;
            this.comboBox2.Text = "9600";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "读取类型";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "记录编号";
            // 
            // comboBox3
            // 
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Location = new System.Drawing.Point(101, 94);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 20);
            this.comboBox3.TabIndex = 6;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(101, 130);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(37, 21);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.Visible = false;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(156, 130);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(37, 21);
            this.numericUpDown2.TabIndex = 8;
            this.numericUpDown2.Visible = false;
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(43, 207);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 9;
            this.btnRead.Text = "读取";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 172);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "发送显示";
            // 
            // tbsend
            // 
            this.tbsend.Location = new System.Drawing.Point(101, 172);
            this.tbsend.Name = "tbsend";
            this.tbsend.Size = new System.Drawing.Size(201, 21);
            this.tbsend.TabIndex = 11;
            // 
            // tbread
            // 
            this.tbread.Location = new System.Drawing.Point(380, 23);
            this.tbread.Multiline = true;
            this.tbread.Name = "tbread";
            this.tbread.Size = new System.Drawing.Size(308, 207);
            this.tbread.TabIndex = 12;
            // 
            // tbreadno
            // 
            this.tbreadno.Location = new System.Drawing.Point(101, 130);
            this.tbreadno.Name = "tbreadno";
            this.tbreadno.Size = new System.Drawing.Size(121, 21);
            this.tbreadno.TabIndex = 13;
            this.tbreadno.Text = "0";
            // 
            // tbTimeout
            // 
            this.tbTimeout.Location = new System.Drawing.Point(285, 60);
            this.tbTimeout.Name = "tbTimeout";
            this.tbTimeout.Size = new System.Drawing.Size(41, 21);
            this.tbTimeout.TabIndex = 14;
            this.tbTimeout.Text = "5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(226, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "超时设置";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(228, 97);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "编号范围";
            // 
            // rb099
            // 
            this.rb099.AutoSize = true;
            this.rb099.Location = new System.Drawing.Point(243, 126);
            this.rb099.Name = "rb099";
            this.rb099.Size = new System.Drawing.Size(65, 16);
            this.rb099.TabIndex = 17;
            this.rb099.TabStop = true;
            this.rb099.Text = "0 ~ 255";
            this.rb099.UseVisualStyleBackColor = true;
            this.rb099.CheckedChanged += new System.EventHandler(this.numRangeCheckedChanged);
            // 
            // rb0999
            // 
            this.rb0999.AutoSize = true;
            this.rb0999.Location = new System.Drawing.Point(243, 148);
            this.rb0999.Name = "rb0999";
            this.rb0999.Size = new System.Drawing.Size(65, 16);
            this.rb0999.TabIndex = 18;
            this.rb0999.TabStop = true;
            this.rb0999.Text = "0 ~ 999";
            this.rb0999.UseVisualStyleBackColor = true;
            this.rb0999.CheckedChanged += new System.EventHandler(this.numRangeCheckedChanged);
            // 
            // ReadData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 261);
            this.Controls.Add(this.rb0999);
            this.Controls.Add(this.rb099);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbTimeout);
            this.Controls.Add(this.tbreadno);
            this.Controls.Add(this.tbread);
            this.Controls.Add(this.tbsend);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Name = "ReadData";
            this.Text = "记录读取";
            this.Load += new System.EventHandler(this.ReadData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button btnRead;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbsend;
        private System.Windows.Forms.TextBox tbread;
        private System.Windows.Forms.TextBox tbreadno;
        private System.Windows.Forms.TextBox tbTimeout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton rb099;
        private System.Windows.Forms.RadioButton rb0999;
    }
}
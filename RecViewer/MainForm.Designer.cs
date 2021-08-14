namespace RecViewer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pnltop = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbxzoomout = new System.Windows.Forms.ToolStripButton();
            this.tbxzommin = new System.Windows.Forms.ToolStripButton();
            this.tbyzoomout = new System.Windows.Forms.ToolStripButton();
            this.tbyzoomin = new System.Windows.Forms.ToolStripButton();
            this.tbzoomreset = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbnaodu = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsm_uploaddata = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_openfile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsm_saveinfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmitobin = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmitocsv = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmitoxml = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmclear = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmprint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiprint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiprintset = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiprintpreview = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmabout = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlfill = new System.Windows.Forms.Panel();
            this.groupboxinfo = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblinfo7 = new System.Windows.Forms.Label();
            this.lbldis7 = new System.Windows.Forms.Label();
            this.lblinfo6 = new System.Windows.Forms.Label();
            this.lbldis6 = new System.Windows.Forms.Label();
            this.lblinfo5 = new System.Windows.Forms.Label();
            this.lbldis5 = new System.Windows.Forms.Label();
            this.lbldiameter = new System.Windows.Forms.Label();
            this.lbldiameterdis = new System.Windows.Forms.Label();
            this.lblheight = new System.Windows.Forms.Label();
            this.lblheightdis = new System.Windows.Forms.Label();
            this.lblno = new System.Windows.Forms.Label();
            this.lblnodis = new System.Windows.Forms.Label();
            this.lbldate = new System.Windows.Forms.Label();
            this.lbldatedis = new System.Windows.Forms.Label();
            this.lbldis4 = new System.Windows.Forms.Label();
            this.lblinfo4 = new System.Windows.Forms.Label();
            this.lbldis3 = new System.Windows.Forms.Label();
            this.lbldis2 = new System.Windows.Forms.Label();
            this.lbldis1 = new System.Windows.Forms.Label();
            this.lblnodecntdis = new System.Windows.Forms.Label();
            this.lblinfo1 = new System.Windows.Forms.Label();
            this.lblnodecnt = new System.Windows.Forms.Label();
            this.lblinfo3 = new System.Windows.Forms.Label();
            this.lblinfo2 = new System.Windows.Forms.Label();
            this.lblrecordname = new System.Windows.Forms.Label();
            this.groupboxchart = new System.Windows.Forms.GroupBox();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.pnltop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnlfill.SuspendLayout();
            this.groupboxinfo.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupboxchart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnltop
            // 
            this.pnltop.Controls.Add(this.panel1);
            this.pnltop.Controls.Add(this.menuStrip1);
            this.pnltop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnltop.Location = new System.Drawing.Point(0, 0);
            this.pnltop.Name = "pnltop";
            this.pnltop.Size = new System.Drawing.Size(1002, 58);
            this.pnltop.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1002, 29);
            this.panel1.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbxzoomout,
            this.tbxzommin,
            this.tbyzoomout,
            this.tbyzoomin,
            this.tbzoomreset,
            this.toolStripSeparator1,
            this.tbnaodu});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1002, 29);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tbxzoomout
            // 
            this.tbxzoomout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbxzoomout.Image = ((System.Drawing.Image)(resources.GetObject("tbxzoomout.Image")));
            this.tbxzoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbxzoomout.Name = "tbxzoomout";
            this.tbxzoomout.Size = new System.Drawing.Size(56, 26);
            this.tbxzoomout.Text = "X轴放大";
            this.tbxzoomout.Click += new System.EventHandler(this.tbxzoomout_Click);
            // 
            // tbxzommin
            // 
            this.tbxzommin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbxzommin.Image = ((System.Drawing.Image)(resources.GetObject("tbxzommin.Image")));
            this.tbxzommin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbxzommin.Name = "tbxzommin";
            this.tbxzommin.Size = new System.Drawing.Size(56, 26);
            this.tbxzommin.Text = "X轴缩小";
            this.tbxzommin.Click += new System.EventHandler(this.tbxzommin_Click);
            // 
            // tbyzoomout
            // 
            this.tbyzoomout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbyzoomout.Image = ((System.Drawing.Image)(resources.GetObject("tbyzoomout.Image")));
            this.tbyzoomout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbyzoomout.Name = "tbyzoomout";
            this.tbyzoomout.Size = new System.Drawing.Size(55, 26);
            this.tbyzoomout.Text = "Y轴放大";
            this.tbyzoomout.Click += new System.EventHandler(this.tbyzoomout_Click);
            // 
            // tbyzoomin
            // 
            this.tbyzoomin.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbyzoomin.Image = ((System.Drawing.Image)(resources.GetObject("tbyzoomin.Image")));
            this.tbyzoomin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbyzoomin.Name = "tbyzoomin";
            this.tbyzoomin.Size = new System.Drawing.Size(55, 26);
            this.tbyzoomin.Text = "Y轴缩小";
            this.tbyzoomin.Click += new System.EventHandler(this.tbyzoomin_Click);
            // 
            // tbzoomreset
            // 
            this.tbzoomreset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tbzoomreset.Image = ((System.Drawing.Image)(resources.GetObject("tbzoomreset.Image")));
            this.tbzoomreset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbzoomreset.Name = "tbzoomreset";
            this.tbzoomreset.Size = new System.Drawing.Size(36, 26);
            this.tbzoomreset.Text = "还原";
            this.tbzoomreset.Click += new System.EventHandler(this.tbzoomreset_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 29);
            // 
            // tbnaodu
            // 
            this.tbnaodu.Image = ((System.Drawing.Image)(resources.GetObject("tbnaodu.Image")));
            this.tbnaodu.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbnaodu.Name = "tbnaodu";
            this.tbnaodu.Size = new System.Drawing.Size(76, 26);
            this.tbnaodu.Text = "挠度修正";
            this.tbnaodu.Visible = false;
            this.tbnaodu.Click += new System.EventHandler(this.tbnaodu_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_uploaddata,
            this.tsm_openfile,
            this.tsm_saveinfo,
            this.tsmclear,
            this.tsmprint,
            this.tsmabout});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1002, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsm_uploaddata
            // 
            this.tsm_uploaddata.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsm_uploaddata.Name = "tsm_uploaddata";
            this.tsm_uploaddata.Size = new System.Drawing.Size(86, 25);
            this.tsm_uploaddata.Text = "读取记录";
            this.tsm_uploaddata.Click += new System.EventHandler(this.tsm_uploaddata_Click);
            // 
            // tsm_openfile
            // 
            this.tsm_openfile.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tsm_openfile.Name = "tsm_openfile";
            this.tsm_openfile.Size = new System.Drawing.Size(86, 25);
            this.tsm_openfile.Text = "调用记录";
            this.tsm_openfile.Click += new System.EventHandler(this.tsm_openfile_Click);
            // 
            // tsm_saveinfo
            // 
            this.tsm_saveinfo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmitobin,
            this.tsmitocsv,
            this.tsmitoxml});
            this.tsm_saveinfo.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.tsm_saveinfo.Name = "tsm_saveinfo";
            this.tsm_saveinfo.Size = new System.Drawing.Size(118, 25);
            this.tsm_saveinfo.Text = "保存当前记录";
            this.tsm_saveinfo.Click += new System.EventHandler(this.tsm_saveinfo_Click);
            // 
            // tsmitobin
            // 
            this.tsmitobin.Name = "tsmitobin";
            this.tsmitobin.Size = new System.Drawing.Size(258, 26);
            this.tsmitobin.Text = "保存成二进制";
            this.tsmitobin.Click += new System.EventHandler(this.tsmitobin_Click);
            // 
            // tsmitocsv
            // 
            this.tsmitocsv.Name = "tsmitocsv";
            this.tsmitocsv.Size = new System.Drawing.Size(258, 26);
            this.tsmitocsv.Text = "保存成CSV";
            this.tsmitocsv.Visible = false;
            this.tsmitocsv.Click += new System.EventHandler(this.tsmitocsv_Click);
            // 
            // tsmitoxml
            // 
            this.tsmitoxml.Name = "tsmitoxml";
            this.tsmitoxml.Size = new System.Drawing.Size(258, 26);
            this.tsmitoxml.Text = "保存成XML(Excel可编辑)";
            this.tsmitoxml.Click += new System.EventHandler(this.tsmitoxml_Click);
            // 
            // tsmclear
            // 
            this.tsmclear.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.tsmclear.Name = "tsmclear";
            this.tsmclear.Size = new System.Drawing.Size(86, 25);
            this.tsmclear.Text = "清除显示";
            this.tsmclear.Click += new System.EventHandler(this.tsmclear_Click);
            // 
            // tsmprint
            // 
            this.tsmprint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiprint,
            this.tsmiprintset,
            this.tsmiprintpreview});
            this.tsmprint.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.tsmprint.Name = "tsmprint";
            this.tsmprint.Size = new System.Drawing.Size(54, 25);
            this.tsmprint.Text = "打印";
            this.tsmprint.Click += new System.EventHandler(this.tsmprint_Click);
            // 
            // tsmiprint
            // 
            this.tsmiprint.Name = "tsmiprint";
            this.tsmiprint.Size = new System.Drawing.Size(144, 26);
            this.tsmiprint.Text = "打印";
            this.tsmiprint.Click += new System.EventHandler(this.tsmiprint_Click);
            // 
            // tsmiprintset
            // 
            this.tsmiprintset.Name = "tsmiprintset";
            this.tsmiprintset.Size = new System.Drawing.Size(144, 26);
            this.tsmiprintset.Text = "打印设置";
            this.tsmiprintset.Click += new System.EventHandler(this.tsmiprintset_Click);
            // 
            // tsmiprintpreview
            // 
            this.tsmiprintpreview.Name = "tsmiprintpreview";
            this.tsmiprintpreview.Size = new System.Drawing.Size(144, 26);
            this.tsmiprintpreview.Text = "打印预览";
            this.tsmiprintpreview.Click += new System.EventHandler(this.tsmiprintpreview_Click);
            // 
            // tsmabout
            // 
            this.tsmabout.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.tsmabout.Name = "tsmabout";
            this.tsmabout.Size = new System.Drawing.Size(86, 25);
            this.tsmabout.Text = "关于版本";
            this.tsmabout.Click += new System.EventHandler(this.tsmabout_Click);
            // 
            // pnlfill
            // 
            this.pnlfill.Controls.Add(this.groupboxinfo);
            this.pnlfill.Controls.Add(this.groupboxchart);
            this.pnlfill.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlfill.Location = new System.Drawing.Point(0, 58);
            this.pnlfill.Name = "pnlfill";
            this.pnlfill.Size = new System.Drawing.Size(1002, 643);
            this.pnlfill.TabIndex = 2;
            // 
            // groupboxinfo
            // 
            this.groupboxinfo.Controls.Add(this.tableLayoutPanel1);
            this.groupboxinfo.Controls.Add(this.lblrecordname);
            this.groupboxinfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupboxinfo.Location = new System.Drawing.Point(697, 0);
            this.groupboxinfo.Name = "groupboxinfo";
            this.groupboxinfo.Size = new System.Drawing.Size(305, 643);
            this.groupboxinfo.TabIndex = 2;
            this.groupboxinfo.TabStop = false;
            this.groupboxinfo.Text = "详情";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblinfo7, 1, 11);
            this.tableLayoutPanel1.Controls.Add(this.lbldis7, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.lblinfo6, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.lbldis6, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.lblinfo5, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.lbldis5, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lbldiameter, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbldiameterdis, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblheight, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblheightdis, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblno, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblnodis, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbldate, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbldatedis, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbldis4, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lblinfo4, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.lbldis3, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lbldis2, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbldis1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblnodecntdis, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblinfo1, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblnodecnt, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblinfo3, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.lblinfo2, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 45);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333332F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(299, 595);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblinfo7
            // 
            this.lblinfo7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblinfo7.AutoSize = true;
            this.lblinfo7.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblinfo7.Location = new System.Drawing.Point(203, 556);
            this.lblinfo7.Name = "lblinfo7";
            this.lblinfo7.Size = new System.Drawing.Size(42, 21);
            this.lblinfo7.TabIndex = 23;
            this.lblinfo7.Text = "保留";
            // 
            // lbldis7
            // 
            this.lbldis7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldis7.AutoSize = true;
            this.lbldis7.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldis7.Location = new System.Drawing.Point(53, 556);
            this.lbldis7.Name = "lbldis7";
            this.lbldis7.Size = new System.Drawing.Size(42, 21);
            this.lbldis7.TabIndex = 22;
            this.lbldis7.Text = "保留";
            // 
            // lblinfo6
            // 
            this.lblinfo6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblinfo6.AutoSize = true;
            this.lblinfo6.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblinfo6.Location = new System.Drawing.Point(203, 504);
            this.lblinfo6.Name = "lblinfo6";
            this.lblinfo6.Size = new System.Drawing.Size(42, 21);
            this.lblinfo6.TabIndex = 21;
            this.lblinfo6.Text = "保留";
            // 
            // lbldis6
            // 
            this.lbldis6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldis6.AutoSize = true;
            this.lbldis6.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldis6.Location = new System.Drawing.Point(53, 504);
            this.lbldis6.Name = "lbldis6";
            this.lbldis6.Size = new System.Drawing.Size(42, 21);
            this.lbldis6.TabIndex = 20;
            this.lbldis6.Text = "保留";
            // 
            // lblinfo5
            // 
            this.lblinfo5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblinfo5.AutoSize = true;
            this.lblinfo5.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblinfo5.Location = new System.Drawing.Point(203, 455);
            this.lblinfo5.Name = "lblinfo5";
            this.lblinfo5.Size = new System.Drawing.Size(42, 21);
            this.lblinfo5.TabIndex = 19;
            this.lblinfo5.Text = "保留";
            // 
            // lbldis5
            // 
            this.lbldis5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldis5.AutoSize = true;
            this.lbldis5.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldis5.Location = new System.Drawing.Point(53, 455);
            this.lbldis5.Name = "lbldis5";
            this.lbldis5.Size = new System.Drawing.Size(42, 21);
            this.lbldis5.TabIndex = 18;
            this.lbldis5.Text = "保留";
            // 
            // lbldiameter
            // 
            this.lbldiameter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldiameter.AutoSize = true;
            this.lbldiameter.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldiameter.Location = new System.Drawing.Point(181, 161);
            this.lbldiameter.Name = "lbldiameter";
            this.lbldiameter.Size = new System.Drawing.Size(85, 21);
            this.lbldiameter.TabIndex = 7;
            this.lbldiameter.Text = "101.0 mm";
            // 
            // lbldiameterdis
            // 
            this.lbldiameterdis.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldiameterdis.AutoSize = true;
            this.lbldiameterdis.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldiameterdis.Location = new System.Drawing.Point(37, 161);
            this.lbldiameterdis.Name = "lbldiameterdis";
            this.lbldiameterdis.Size = new System.Drawing.Size(74, 21);
            this.lbldiameterdis.TabIndex = 6;
            this.lbldiameterdis.Text = "试件宽度";
            // 
            // lblheight
            // 
            this.lblheight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblheight.AutoSize = true;
            this.lblheight.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblheight.Location = new System.Drawing.Point(181, 112);
            this.lblheight.Name = "lblheight";
            this.lblheight.Size = new System.Drawing.Size(85, 21);
            this.lblheight.TabIndex = 5;
            this.lblheight.Text = "100.0 mm";
            // 
            // lblheightdis
            // 
            this.lblheightdis.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblheightdis.AutoSize = true;
            this.lblheightdis.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblheightdis.Location = new System.Drawing.Point(37, 112);
            this.lblheightdis.Name = "lblheightdis";
            this.lblheightdis.Size = new System.Drawing.Size(74, 21);
            this.lblheightdis.TabIndex = 4;
            this.lblheightdis.Text = "试件高度";
            // 
            // lblno
            // 
            this.lblno.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblno.AutoSize = true;
            this.lblno.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblno.Location = new System.Drawing.Point(210, 63);
            this.lblno.Name = "lblno";
            this.lblno.Size = new System.Drawing.Size(28, 21);
            this.lblno.TabIndex = 3;
            this.lblno.Text = "12";
            // 
            // lblnodis
            // 
            this.lblnodis.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblnodis.AutoSize = true;
            this.lblnodis.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblnodis.Location = new System.Drawing.Point(29, 63);
            this.lblnodis.Name = "lblnodis";
            this.lblnodis.Size = new System.Drawing.Size(90, 21);
            this.lblnodis.TabIndex = 2;
            this.lblnodis.Text = "传感器";
            // 
            // lbldate
            // 
            this.lbldate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldate.AutoSize = true;
            this.lbldate.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldate.Location = new System.Drawing.Point(152, 3);
            this.lbldate.Name = "lbldate";
            this.lbldate.Size = new System.Drawing.Size(144, 42);
            this.lbldate.TabIndex = 1;
            this.lbldate.Text = "2017年9月24日 15时17分";
            // 
            // lbldatedis
            // 
            this.lbldatedis.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldatedis.AutoSize = true;
            this.lbldatedis.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldatedis.Location = new System.Drawing.Point(53, 14);
            this.lbldatedis.Name = "lbldatedis";
            this.lbldatedis.Size = new System.Drawing.Size(42, 21);
            this.lbldatedis.TabIndex = 0;
            this.lbldatedis.Text = "日期";
            // 
            // lbldis4
            // 
            this.lbldis4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldis4.AutoSize = true;
            this.lbldis4.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldis4.Location = new System.Drawing.Point(31, 406);
            this.lbldis4.Name = "lbldis4";
            this.lbldis4.Size = new System.Drawing.Size(86, 21);
            this.lbldis4.TabIndex = 14;
            this.lbldis4.Text = "5mm 压强";
            // 
            // lblinfo4
            // 
            this.lblinfo4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblinfo4.AutoSize = true;
            this.lblinfo4.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblinfo4.Location = new System.Drawing.Point(179, 406);
            this.lblinfo4.Name = "lblinfo4";
            this.lblinfo4.Size = new System.Drawing.Size(89, 21);
            this.lblinfo4.TabIndex = 15;
            this.lblinfo4.Text = "11880 KPa";
            // 
            // lbldis3
            // 
            this.lbldis3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldis3.AutoSize = true;
            this.lbldis3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldis3.Location = new System.Drawing.Point(32, 357);
            this.lbldis3.Name = "lbldis3";
            this.lbldis3.Size = new System.Drawing.Size(85, 21);
            this.lbldis3.TabIndex = 12;
            this.lbldis3.Text = "5mm CBR";
            // 
            // lbldis2
            // 
            this.lbldis2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldis2.AutoSize = true;
            this.lbldis2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldis2.Location = new System.Drawing.Point(25, 308);
            this.lbldis2.Name = "lbldis2";
            this.lbldis2.Size = new System.Drawing.Size(99, 21);
            this.lbldis2.TabIndex = 10;
            this.lbldis2.Text = "2.5mm 压强";
            // 
            // lbldis1
            // 
            this.lbldis1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbldis1.AutoSize = true;
            this.lbldis1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbldis1.Location = new System.Drawing.Point(25, 259);
            this.lbldis1.Name = "lbldis1";
            this.lbldis1.Size = new System.Drawing.Size(98, 21);
            this.lbldis1.TabIndex = 8;
            this.lbldis1.Text = "2.5mm CBR";
            // 
            // lblnodecntdis
            // 
            this.lblnodecntdis.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblnodecntdis.AutoSize = true;
            this.lblnodecntdis.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblnodecntdis.Location = new System.Drawing.Point(37, 210);
            this.lblnodecntdis.Name = "lblnodecntdis";
            this.lblnodecntdis.Size = new System.Drawing.Size(74, 21);
            this.lblnodecntdis.TabIndex = 16;
            this.lblnodecntdis.Text = "记录点数";
            // 
            // lblinfo1
            // 
            this.lblinfo1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblinfo1.AutoSize = true;
            this.lblinfo1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblinfo1.Location = new System.Drawing.Point(189, 259);
            this.lblinfo1.Name = "lblinfo1";
            this.lblinfo1.Size = new System.Drawing.Size(69, 21);
            this.lblinfo1.TabIndex = 9;
            this.lblinfo1.Text = "85.69 %";
            // 
            // lblnodecnt
            // 
            this.lblnodecnt.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblnodecnt.AutoSize = true;
            this.lblnodecnt.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblnodecnt.Location = new System.Drawing.Point(205, 210);
            this.lblnodecnt.Name = "lblnodecnt";
            this.lblnodecnt.Size = new System.Drawing.Size(37, 21);
            this.lblnodecnt.TabIndex = 17;
            this.lblnodecnt.Text = "388";
            // 
            // lblinfo3
            // 
            this.lblinfo3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblinfo3.AutoSize = true;
            this.lblinfo3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblinfo3.Location = new System.Drawing.Point(184, 357);
            this.lblinfo3.Name = "lblinfo3";
            this.lblinfo3.Size = new System.Drawing.Size(80, 21);
            this.lblinfo3.TabIndex = 11;
            this.lblinfo3.Text = "5998 KPa";
            // 
            // lblinfo2
            // 
            this.lblinfo2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblinfo2.AutoSize = true;
            this.lblinfo2.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblinfo2.Location = new System.Drawing.Point(185, 308);
            this.lblinfo2.Name = "lblinfo2";
            this.lblinfo2.Size = new System.Drawing.Size(78, 21);
            this.lblinfo2.TabIndex = 13;
            this.lblinfo2.Text = "113.14 %";
            // 
            // lblrecordname
            // 
            this.lblrecordname.AutoSize = true;
            this.lblrecordname.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblrecordname.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblrecordname.Location = new System.Drawing.Point(3, 17);
            this.lblrecordname.Name = "lblrecordname";
            this.lblrecordname.Size = new System.Drawing.Size(96, 28);
            this.lblrecordname.TabIndex = 1;
            this.lblrecordname.Text = "记录名称";
            this.lblrecordname.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupboxchart
            // 
            this.groupboxchart.Controls.Add(this.chart1);
            this.groupboxchart.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupboxchart.Location = new System.Drawing.Point(0, 0);
            this.groupboxchart.Name = "groupboxchart";
            this.groupboxchart.Size = new System.Drawing.Size(697, 643);
            this.groupboxchart.TabIndex = 1;
            this.groupboxchart.TabStop = false;
            this.groupboxchart.Text = "曲线";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 17);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(691, 623);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "记录文件|*.rec.bin;*.xml";
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.printDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.printDocument1;
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.printDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1002, 701);
            this.Controls.Add(this.pnlfill);
            this.Controls.Add(this.pnltop);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "试验记录读取工具";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.pnltop.ResumeLayout(false);
            this.pnltop.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlfill.ResumeLayout(false);
            this.groupboxinfo.ResumeLayout(false);
            this.groupboxinfo.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupboxchart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel pnltop;
        private System.Windows.Forms.Panel pnlfill;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsm_uploaddata;
        private System.Windows.Forms.ToolStripMenuItem tsm_openfile;
        private System.Windows.Forms.GroupBox groupboxinfo;
        private System.Windows.Forms.GroupBox groupboxchart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblnodis;
        private System.Windows.Forms.Label lbldate;
        private System.Windows.Forms.Label lbldatedis;
        private System.Windows.Forms.Label lblinfo7;
        private System.Windows.Forms.Label lbldis7;
        private System.Windows.Forms.Label lblinfo6;
        private System.Windows.Forms.Label lbldis6;
        private System.Windows.Forms.Label lblinfo5;
        private System.Windows.Forms.Label lbldis5;
        private System.Windows.Forms.Label lblnodecnt;
        private System.Windows.Forms.Label lblnodecntdis;
        private System.Windows.Forms.Label lblinfo4;
        private System.Windows.Forms.Label lbldis4;
        private System.Windows.Forms.Label lblinfo2;
        private System.Windows.Forms.Label lbldis3;
        private System.Windows.Forms.Label lblinfo3;
        private System.Windows.Forms.Label lbldis2;
        private System.Windows.Forms.Label lblinfo1;
        private System.Windows.Forms.Label lbldis1;
        private System.Windows.Forms.Label lbldiameter;
        private System.Windows.Forms.Label lbldiameterdis;
        private System.Windows.Forms.Label lblheight;
        private System.Windows.Forms.Label lblheightdis;
        private System.Windows.Forms.Label lblno;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ToolStripMenuItem tsm_saveinfo;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label lblrecordname;
        private System.Windows.Forms.ToolStripMenuItem tsmclear;
        private System.Windows.Forms.ToolStripMenuItem tsmabout;
        private System.Windows.Forms.ToolStripMenuItem tsmprint;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.ToolStripMenuItem tsmiprintset;
        private System.Windows.Forms.ToolStripMenuItem tsmiprintpreview;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.ToolStripMenuItem tsmiprint;
        private System.Windows.Forms.ToolStripMenuItem tsmitobin;
        private System.Windows.Forms.ToolStripMenuItem tsmitocsv;
        private System.Windows.Forms.ToolStripMenuItem tsmitoxml;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbxzoomout;
        private System.Windows.Forms.ToolStripButton tbxzommin;
        private System.Windows.Forms.ToolStripButton tbyzoomout;
        private System.Windows.Forms.ToolStripButton tbyzoomin;
        private System.Windows.Forms.ToolStripButton tbzoomreset;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbnaodu;
        
    }
}


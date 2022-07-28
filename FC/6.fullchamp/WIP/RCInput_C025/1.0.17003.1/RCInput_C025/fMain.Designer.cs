﻿namespace RCInput_C025
{
    partial class fMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fMain));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tsDetial = new System.Windows.Forms.ToolStripMenuItem();
            this.tsDll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cmbParam = new System.Windows.Forms.ToolStripLabel();
            this.txtInput = new System.Windows.Forms.ToolStripTextBox();
            this.Tsb_OK = new System.Windows.Forms.ToolStripButton();
            this.Tsb_Clear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tsEmp = new System.Windows.Forms.ToolStripLabel();
            this.cmsTabpage = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.TcData = new System.Windows.Forms.TabControl();
            this.TpParam = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.gvCondition = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gvInput = new System.Windows.Forms.DataGridView();
            this.TpMachine = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.gvMachine = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.TbFindMachine = new System.Windows.Forms.TextBox();
            this.BtnFindMachine = new System.Windows.Forms.Button();
            this.LbMachineErrorMsg = new System.Windows.Forms.Label();
            this.TpInTime = new System.Windows.Forms.TabPage();
            this.CbReportTime = new System.Windows.Forms.CheckBox();
            this.LbLastReportTime = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DtpWipIn = new System.Windows.Forms.DateTimePicker();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tsMemo = new System.Windows.Forms.TextBox();
            this.BtnOK = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.cmsTabpage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.TcData.SuspendLayout();
            this.TpParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCondition)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvInput)).BeginInit();
            this.TpMachine.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMachine)).BeginInit();
            this.TpInTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "mouse.gif");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(144, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(880, 132);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // tsDetial
            // 
            this.tsDetial.Name = "tsDetial";
            this.tsDetial.Size = new System.Drawing.Size(116, 22);
            this.tsDetial.Text = "tsDetail";
            // 
            // tsDll
            // 
            this.tsDll.Name = "tsDll";
            this.tsDll.Size = new System.Drawing.Size(116, 22);
            this.tsDll.Text = "tsDll";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbParam,
            this.txtInput,
            this.Tsb_OK,
            this.Tsb_Clear,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.tsEmp});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(884, 28);
            this.toolStrip1.TabIndex = 37;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cmbParam
            // 
            this.cmbParam.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cmbParam.Name = "cmbParam";
            this.cmbParam.Size = new System.Drawing.Size(46, 25);
            this.cmbParam.Text = "RC No";
            // 
            // txtInput
            // 
            this.txtInput.Font = new System.Drawing.Font("新細明體", 9.75F);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(200, 28);
            // 
            // Tsb_OK
            // 
            this.Tsb_OK.AutoSize = false;
            this.Tsb_OK.BackColor = System.Drawing.Color.Khaki;
            this.Tsb_OK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Tsb_OK.Image = ((System.Drawing.Image)(resources.GetObject("Tsb_OK.Image")));
            this.Tsb_OK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Tsb_OK.Name = "Tsb_OK";
            this.Tsb_OK.Size = new System.Drawing.Size(75, 25);
            this.Tsb_OK.Text = "OK";
            // 
            // Tsb_Clear
            // 
            this.Tsb_Clear.AutoSize = false;
            this.Tsb_Clear.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Tsb_Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Tsb_Clear.Image = ((System.Drawing.Image)(resources.GetObject("Tsb_Clear.Image")));
            this.Tsb_Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Tsb_Clear.Name = "Tsb_Clear";
            this.Tsb_Clear.Size = new System.Drawing.Size(75, 25);
            this.Tsb_Clear.Text = "Clear";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(63, 25);
            this.toolStripLabel2.Text = "Employee";
            // 
            // tsEmp
            // 
            this.tsEmp.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tsEmp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.tsEmp.Name = "tsEmp";
            this.tsEmp.Size = new System.Drawing.Size(0, 25);
            // 
            // cmsTabpage
            // 
            this.cmsTabpage.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsTabpage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsDll,
            this.tsDetial});
            this.cmsTabpage.Name = "cmsTabpage";
            this.cmsTabpage.Size = new System.Drawing.Size(117, 48);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(884, 29);
            this.tableLayoutPanel2.TabIndex = 41;
            // 
            // TcData
            // 
            this.TcData.Controls.Add(this.TpParam);
            this.TcData.Controls.Add(this.TpMachine);
            this.TcData.Controls.Add(this.TpInTime);
            this.TcData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TcData.Location = new System.Drawing.Point(0, 57);
            this.TcData.Name = "TcData";
            this.TcData.SelectedIndex = 0;
            this.TcData.Size = new System.Drawing.Size(884, 476);
            this.TcData.TabIndex = 42;
            // 
            // TpParam
            // 
            this.TpParam.Controls.Add(this.splitContainer1);
            this.TpParam.Location = new System.Drawing.Point(4, 22);
            this.TpParam.Name = "TpParam";
            this.TpParam.Size = new System.Drawing.Size(876, 450);
            this.TpParam.TabIndex = 0;
            this.TpParam.Text = "Process Params";
            this.TpParam.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(876, 450);
            this.splitContainer1.SplitterDistance = 416;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.gvCondition);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 450);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Process Condition";
            // 
            // gvCondition
            // 
            this.gvCondition.AllowUserToAddRows = false;
            this.gvCondition.AllowUserToDeleteRows = false;
            this.gvCondition.BackgroundColor = System.Drawing.Color.White;
            this.gvCondition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvCondition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvCondition.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvCondition.Location = new System.Drawing.Point(3, 18);
            this.gvCondition.Name = "gvCondition";
            this.gvCondition.ReadOnly = true;
            this.gvCondition.RowHeadersWidth = 25;
            this.gvCondition.RowTemplate.Height = 24;
            this.gvCondition.Size = new System.Drawing.Size(410, 429);
            this.gvCondition.TabIndex = 3;
            this.gvCondition.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvCondition_CellClick);
            this.gvCondition.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.GvCondition_DataBindingComplete);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gvInput);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 450);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Data Collection";
            // 
            // gvInput
            // 
            this.gvInput.AllowUserToAddRows = false;
            this.gvInput.AllowUserToDeleteRows = false;
            this.gvInput.BackgroundColor = System.Drawing.Color.White;
            this.gvInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvInput.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvInput.Location = new System.Drawing.Point(3, 18);
            this.gvInput.Name = "gvInput";
            this.gvInput.RowHeadersWidth = 25;
            this.gvInput.RowTemplate.Height = 24;
            this.gvInput.Size = new System.Drawing.Size(450, 429);
            this.gvInput.TabIndex = 4;
            this.gvInput.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GvInput_CellEndEdit);
            // 
            // TpMachine
            // 
            this.TpMachine.BackColor = System.Drawing.SystemColors.Control;
            this.TpMachine.Controls.Add(this.tableLayoutPanel4);
            this.TpMachine.Location = new System.Drawing.Point(4, 22);
            this.TpMachine.Name = "TpMachine";
            this.TpMachine.Size = new System.Drawing.Size(876, 450);
            this.TpMachine.TabIndex = 1;
            this.TpMachine.Text = "Machine";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.gvMachine, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.TbFindMachine, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.BtnFindMachine, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.LbMachineErrorMsg, 3, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(876, 450);
            this.tableLayoutPanel4.TabIndex = 5;
            // 
            // gvMachine
            // 
            this.gvMachine.AllowUserToAddRows = false;
            this.gvMachine.AllowUserToDeleteRows = false;
            this.gvMachine.BackgroundColor = System.Drawing.Color.White;
            this.gvMachine.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel4.SetColumnSpan(this.gvMachine, 4);
            this.gvMachine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMachine.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvMachine.Location = new System.Drawing.Point(3, 48);
            this.gvMachine.Name = "gvMachine";
            this.gvMachine.RowHeadersWidth = 25;
            this.gvMachine.RowTemplate.Height = 24;
            this.gvMachine.Size = new System.Drawing.Size(870, 399);
            this.gvMachine.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(3, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Machine";
            // 
            // TbFindMachine
            // 
            this.TbFindMachine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TbFindMachine.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TbFindMachine.Location = new System.Drawing.Point(71, 9);
            this.TbFindMachine.Name = "TbFindMachine";
            this.TbFindMachine.Size = new System.Drawing.Size(150, 27);
            this.TbFindMachine.TabIndex = 6;
            // 
            // BtnFindMachine
            // 
            this.BtnFindMachine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnFindMachine.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.BtnFindMachine.Location = new System.Drawing.Point(227, 9);
            this.BtnFindMachine.Name = "BtnFindMachine";
            this.BtnFindMachine.Size = new System.Drawing.Size(100, 27);
            this.BtnFindMachine.TabIndex = 7;
            this.BtnFindMachine.Text = "Enter";
            this.BtnFindMachine.UseVisualStyleBackColor = true;
            // 
            // LbMachineErrorMsg
            // 
            this.LbMachineErrorMsg.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LbMachineErrorMsg.AutoSize = true;
            this.LbMachineErrorMsg.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbMachineErrorMsg.ForeColor = System.Drawing.Color.Red;
            this.LbMachineErrorMsg.Location = new System.Drawing.Point(333, 14);
            this.LbMachineErrorMsg.Name = "LbMachineErrorMsg";
            this.LbMachineErrorMsg.Size = new System.Drawing.Size(18, 16);
            this.LbMachineErrorMsg.TabIndex = 8;
            this.LbMachineErrorMsg.Text = "--";
            // 
            // TpInTime
            // 
            this.TpInTime.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TpInTime.Controls.Add(this.CbReportTime);
            this.TpInTime.Controls.Add(this.LbLastReportTime);
            this.TpInTime.Controls.Add(this.label4);
            this.TpInTime.Controls.Add(this.DtpWipIn);
            this.TpInTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TpInTime.Location = new System.Drawing.Point(4, 22);
            this.TpInTime.Margin = new System.Windows.Forms.Padding(2);
            this.TpInTime.Name = "TpInTime";
            this.TpInTime.Size = new System.Drawing.Size(876, 450);
            this.TpInTime.TabIndex = 5;
            this.TpInTime.Text = "In Process Time";
            // 
            // CbReportTime
            // 
            this.CbReportTime.AutoSize = true;
            this.CbReportTime.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.CbReportTime.Location = new System.Drawing.Point(24, 100);
            this.CbReportTime.Name = "CbReportTime";
            this.CbReportTime.Size = new System.Drawing.Size(91, 20);
            this.CbReportTime.TabIndex = 4;
            this.CbReportTime.Text = "Input time";
            this.CbReportTime.UseVisualStyleBackColor = true;
            // 
            // LbLastReportTime
            // 
            this.LbLastReportTime.AutoSize = true;
            this.LbLastReportTime.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.LbLastReportTime.Location = new System.Drawing.Point(180, 40);
            this.LbLastReportTime.Name = "LbLastReportTime";
            this.LbLastReportTime.Size = new System.Drawing.Size(18, 16);
            this.LbLastReportTime.TabIndex = 3;
            this.LbLastReportTime.Text = "--";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(40, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Last report time";
            // 
            // DtpWipIn
            // 
            this.DtpWipIn.CustomFormat = "yyyy/ MM/ dd HH: mm: ss";
            this.DtpWipIn.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.DtpWipIn.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpWipIn.Location = new System.Drawing.Point(180, 93);
            this.DtpWipIn.Margin = new System.Windows.Forms.Padding(2);
            this.DtpWipIn.Name = "DtpWipIn";
            this.DtpWipIn.Size = new System.Drawing.Size(250, 27);
            this.DtpWipIn.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tsMemo, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.BtnOK, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.BtnCancel, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 533);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(884, 36);
            this.tableLayoutPanel3.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remark";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tsMemo
            // 
            this.tsMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tsMemo.Location = new System.Drawing.Point(51, 7);
            this.tsMemo.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tsMemo.Name = "tsMemo";
            this.tsMemo.Size = new System.Drawing.Size(680, 22);
            this.tsMemo.TabIndex = 1;
            // 
            // BtnOK
            // 
            this.BtnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.BtnOK.Location = new System.Drawing.Point(734, 5);
            this.BtnOK.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 25);
            this.BtnOK.TabIndex = 2;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.BtnCancel.Location = new System.Drawing.Point(809, 5);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 25);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 569);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(884, 22);
            this.statusStrip1.TabIndex = 45;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(58, 17);
            this.toolStripStatusLabel1.Text = "Message";
            // 
            // tsMsg
            // 
            this.tsMsg.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tsMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.tsMsg.Name = "tsMsg";
            this.tsMsg.Size = new System.Drawing.Size(0, 17);
            // 
            // fMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 591);
            this.Controls.Add(this.TcData);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.statusStrip1);
            this.Name = "fMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RC Input";
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmsTabpage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.TcData.ResumeLayout(false);
            this.TpParam.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvCondition)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvInput)).EndInit();
            this.TpMachine.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvMachine)).EndInit();
            this.TpInTime.ResumeLayout(false);
            this.TpInTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem tsDetial;
        private System.Windows.Forms.ToolStripMenuItem tsDll;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox txtInput;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip cmsTabpage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStripLabel cmbParam;
        private System.Windows.Forms.TabControl TcData;
        private System.Windows.Forms.TabPage TpParam;
        private System.Windows.Forms.TabPage TpMachine;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gvCondition;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel tsEmp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tsMemo;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.DataGridView gvMachine;
        private System.Windows.Forms.DataGridView gvInput;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsMsg;
        private System.Windows.Forms.TabPage TpInTime;
        private System.Windows.Forms.DateTimePicker DtpWipIn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TbFindMachine;
        private System.Windows.Forms.Button BtnFindMachine;
        private System.Windows.Forms.Label LbMachineErrorMsg;
        private System.Windows.Forms.Label LbLastReportTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox CbReportTime;
        private System.Windows.Forms.ToolStripButton Tsb_OK;
        private System.Windows.Forms.ToolStripButton Tsb_Clear;
    }
}
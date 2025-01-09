namespace VisionLib.Test
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartAcquire = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblSetOriginImageFolderPath = new System.Windows.Forms.Label();
            this.btnSetOriginFolderPath = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.grbCameraParams = new System.Windows.Forms.GroupBox();
            this.txbTriggerInterval = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbExposure = new System.Windows.Forms.TextBox();
            this.txbContrast = new System.Windows.Forms.TextBox();
            this.btnSetContrast = new System.Windows.Forms.Button();
            this.txbBrightness = new System.Windows.Forms.TextBox();
            this.btnSetBrightness = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSetExposure = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grbCameraAction = new System.Windows.Forms.GroupBox();
            this.btnContinueStop = new System.Windows.Forms.Button();
            this.btnContinueAcquire = new System.Windows.Forms.Button();
            this.btnLive = new System.Windows.Forms.Button();
            this.btnStopLive = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.cmbCameraSerialList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txbNumberToFind = new System.Windows.Forms.TextBox();
            this.cmbSaveImageFormat = new System.Windows.Forms.ComboBox();
            this.chbSaveOverlayImage = new System.Windows.Forms.CheckBox();
            this.chbSaveOrgImage = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSetOverlayImageFolderPath = new System.Windows.Forms.Label();
            this.btnSetOverlayFolderPath = new System.Windows.Forms.Button();
            this.btnOpenVpp = new System.Windows.Forms.Button();
            this.btnLoadVppFromFile = new System.Windows.Forms.Button();
            this.btnLoadImageFromFile = new System.Windows.Forms.Button();
            this.pnlDisplay = new System.Windows.Forms.Panel();
            this.cogRecordDisplayControl1 = new VisionPro96.Controls.CogRecordDisplayControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grbCameraParams.SuspendLayout();
            this.grbCameraAction.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartAcquire
            // 
            this.btnStartAcquire.Location = new System.Drawing.Point(9, 29);
            this.btnStartAcquire.Name = "btnStartAcquire";
            this.btnStartAcquire.Size = new System.Drawing.Size(126, 23);
            this.btnStartAcquire.TabIndex = 0;
            this.btnStartAcquire.Text = "Acquire";
            this.btnStartAcquire.UseVisualStyleBackColor = true;
            this.btnStartAcquire.Click += new System.EventHandler(this.BtnStartAcquire_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.38699F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.61301F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlDisplay, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1168, 890);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnDisconnect);
            this.panel1.Controls.Add(this.grbCameraParams);
            this.panel1.Controls.Add(this.grbCameraAction);
            this.panel1.Controls.Add(this.btnConnect);
            this.panel1.Controls.Add(this.cmbCameraSerialList);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txbNumberToFind);
            this.panel1.Controls.Add(this.cmbSaveImageFormat);
            this.panel1.Controls.Add(this.chbSaveOverlayImage);
            this.panel1.Controls.Add(this.chbSaveOrgImage);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnOpenVpp);
            this.panel1.Controls.Add(this.btnLoadVppFromFile);
            this.panel1.Controls.Add(this.btnLoadImageFromFile);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(419, 884);
            this.panel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblSetOriginImageFolderPath);
            this.groupBox2.Controls.Add(this.btnSetOriginFolderPath);
            this.groupBox2.Location = new System.Drawing.Point(218, 763);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(152, 100);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "원본 이미지 저장 경로";
            // 
            // lblSetOriginImageFolderPath
            // 
            this.lblSetOriginImageFolderPath.AutoSize = true;
            this.lblSetOriginImageFolderPath.Location = new System.Drawing.Point(13, 64);
            this.lblSetOriginImageFolderPath.Name = "lblSetOriginImageFolderPath";
            this.lblSetOriginImageFolderPath.Size = new System.Drawing.Size(29, 12);
            this.lblSetOriginImageFolderPath.TabIndex = 13;
            this.lblSetOriginImageFolderPath.Text = "경로";
            // 
            // btnSetOriginFolderPath
            // 
            this.btnSetOriginFolderPath.Location = new System.Drawing.Point(15, 20);
            this.btnSetOriginFolderPath.Name = "btnSetOriginFolderPath";
            this.btnSetOriginFolderPath.Size = new System.Drawing.Size(126, 23);
            this.btnSetOriginFolderPath.TabIndex = 12;
            this.btnSetOriginFolderPath.Text = "Set Folder Path";
            this.btnSetOriginFolderPath.UseVisualStyleBackColor = true;
            this.btnSetOriginFolderPath.Click += new System.EventHandler(this.BtnSetOriginFolderPath_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new System.Drawing.Point(315, 41);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(91, 23);
            this.btnDisconnect.TabIndex = 32;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.BtnDisconnect_Click);
            // 
            // grbCameraParams
            // 
            this.grbCameraParams.Controls.Add(this.txbTriggerInterval);
            this.grbCameraParams.Controls.Add(this.label1);
            this.grbCameraParams.Controls.Add(this.txbExposure);
            this.grbCameraParams.Controls.Add(this.txbContrast);
            this.grbCameraParams.Controls.Add(this.btnSetContrast);
            this.grbCameraParams.Controls.Add(this.txbBrightness);
            this.grbCameraParams.Controls.Add(this.btnSetBrightness);
            this.grbCameraParams.Controls.Add(this.label3);
            this.grbCameraParams.Controls.Add(this.btnSetExposure);
            this.grbCameraParams.Controls.Add(this.label4);
            this.grbCameraParams.Controls.Add(this.label5);
            this.grbCameraParams.Enabled = false;
            this.grbCameraParams.Location = new System.Drawing.Point(9, 264);
            this.grbCameraParams.Name = "grbCameraParams";
            this.grbCameraParams.Size = new System.Drawing.Size(286, 128);
            this.grbCameraParams.TabIndex = 31;
            this.grbCameraParams.TabStop = false;
            this.grbCameraParams.Text = "Camera Parameter";
            // 
            // txbTriggerInterval
            // 
            this.txbTriggerInterval.Location = new System.Drawing.Point(15, 20);
            this.txbTriggerInterval.Name = "txbTriggerInterval";
            this.txbTriggerInterval.Size = new System.Drawing.Size(100, 21);
            this.txbTriggerInterval.TabIndex = 10;
            this.txbTriggerInterval.TextChanged += new System.EventHandler(this.TxbTriggerInterval_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "hz";
            // 
            // txbExposure
            // 
            this.txbExposure.Location = new System.Drawing.Point(104, 48);
            this.txbExposure.Name = "txbExposure";
            this.txbExposure.Size = new System.Drawing.Size(100, 21);
            this.txbExposure.TabIndex = 19;
            // 
            // txbContrast
            // 
            this.txbContrast.Location = new System.Drawing.Point(104, 97);
            this.txbContrast.Name = "txbContrast";
            this.txbContrast.Size = new System.Drawing.Size(100, 21);
            this.txbContrast.TabIndex = 20;
            // 
            // btnSetContrast
            // 
            this.btnSetContrast.Location = new System.Drawing.Point(209, 96);
            this.btnSetContrast.Name = "btnSetContrast";
            this.btnSetContrast.Size = new System.Drawing.Size(59, 23);
            this.btnSetContrast.TabIndex = 27;
            this.btnSetContrast.Text = "Set";
            this.btnSetContrast.UseVisualStyleBackColor = true;
            this.btnSetContrast.Click += new System.EventHandler(this.BtnSetContrast_Click);
            // 
            // txbBrightness
            // 
            this.txbBrightness.Location = new System.Drawing.Point(104, 73);
            this.txbBrightness.Name = "txbBrightness";
            this.txbBrightness.Size = new System.Drawing.Size(100, 21);
            this.txbBrightness.TabIndex = 21;
            // 
            // btnSetBrightness
            // 
            this.btnSetBrightness.Location = new System.Drawing.Point(209, 72);
            this.btnSetBrightness.Name = "btnSetBrightness";
            this.btnSetBrightness.Size = new System.Drawing.Size(59, 23);
            this.btnSetBrightness.TabIndex = 26;
            this.btnSetBrightness.Text = "Set";
            this.btnSetBrightness.UseVisualStyleBackColor = true;
            this.btnSetBrightness.Click += new System.EventHandler(this.BtnSetBrightness_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "exposure : ";
            // 
            // btnSetExposure
            // 
            this.btnSetExposure.Location = new System.Drawing.Point(209, 47);
            this.btnSetExposure.Name = "btnSetExposure";
            this.btnSetExposure.Size = new System.Drawing.Size(59, 23);
            this.btnSetExposure.TabIndex = 25;
            this.btnSetExposure.Text = "Set";
            this.btnSetExposure.UseVisualStyleBackColor = true;
            this.btnSetExposure.Click += new System.EventHandler(this.BtnSetExposure_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "brightness : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 12);
            this.label5.TabIndex = 24;
            this.label5.Text = "contrast : ";
            // 
            // grbCameraAction
            // 
            this.grbCameraAction.Controls.Add(this.btnContinueStop);
            this.grbCameraAction.Controls.Add(this.btnContinueAcquire);
            this.grbCameraAction.Controls.Add(this.btnStartAcquire);
            this.grbCameraAction.Controls.Add(this.btnLive);
            this.grbCameraAction.Controls.Add(this.btnStopLive);
            this.grbCameraAction.Controls.Add(this.btnRun);
            this.grbCameraAction.Enabled = false;
            this.grbCameraAction.Location = new System.Drawing.Point(9, 12);
            this.grbCameraAction.Name = "grbCameraAction";
            this.grbCameraAction.Size = new System.Drawing.Size(139, 217);
            this.grbCameraAction.TabIndex = 30;
            this.grbCameraAction.TabStop = false;
            this.grbCameraAction.Text = "Camera Action";
            // 
            // btnContinueStop
            // 
            this.btnContinueStop.Location = new System.Drawing.Point(9, 174);
            this.btnContinueStop.Name = "btnContinueStop";
            this.btnContinueStop.Size = new System.Drawing.Size(126, 23);
            this.btnContinueStop.TabIndex = 10;
            this.btnContinueStop.Text = "Contiue Stop";
            this.btnContinueStop.UseVisualStyleBackColor = true;
            this.btnContinueStop.Click += new System.EventHandler(this.BtnContinueStop_Click);
            // 
            // btnContinueAcquire
            // 
            this.btnContinueAcquire.Location = new System.Drawing.Point(9, 145);
            this.btnContinueAcquire.Name = "btnContinueAcquire";
            this.btnContinueAcquire.Size = new System.Drawing.Size(126, 23);
            this.btnContinueAcquire.TabIndex = 9;
            this.btnContinueAcquire.Text = "Contiue Acquire";
            this.btnContinueAcquire.UseVisualStyleBackColor = true;
            this.btnContinueAcquire.Click += new System.EventHandler(this.BtnContinueAcquire_Click);
            // 
            // btnLive
            // 
            this.btnLive.Location = new System.Drawing.Point(9, 58);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(126, 23);
            this.btnLive.TabIndex = 1;
            this.btnLive.Text = "Live";
            this.btnLive.UseVisualStyleBackColor = true;
            this.btnLive.Click += new System.EventHandler(this.BtnLive_Click);
            // 
            // btnStopLive
            // 
            this.btnStopLive.Location = new System.Drawing.Point(9, 87);
            this.btnStopLive.Name = "btnStopLive";
            this.btnStopLive.Size = new System.Drawing.Size(126, 23);
            this.btnStopLive.TabIndex = 2;
            this.btnStopLive.Text = "Live Stop";
            this.btnStopLive.UseVisualStyleBackColor = true;
            this.btnStopLive.Click += new System.EventHandler(this.BtnStopLive_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(9, 116);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(126, 23);
            this.btnRun.TabIndex = 3;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Enabled = false;
            this.btnConnect.Location = new System.Drawing.Point(315, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(91, 23);
            this.btnConnect.TabIndex = 29;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
            // 
            // cmbCameraSerialList
            // 
            this.cmbCameraSerialList.FormattingEnabled = true;
            this.cmbCameraSerialList.Location = new System.Drawing.Point(158, 13);
            this.cmbCameraSerialList.Name = "cmbCameraSerialList";
            this.cmbCameraSerialList.Size = new System.Drawing.Size(149, 20);
            this.cmbCameraSerialList.TabIndex = 28;
            this.cmbCameraSerialList.SelectedIndexChanged += new System.EventHandler(this.CmbCameraSerialList_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 644);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "검출 개수 : ";
            // 
            // txbNumberToFind
            // 
            this.txbNumberToFind.Location = new System.Drawing.Point(94, 640);
            this.txbNumberToFind.Name = "txbNumberToFind";
            this.txbNumberToFind.Size = new System.Drawing.Size(100, 21);
            this.txbNumberToFind.TabIndex = 17;
            this.txbNumberToFind.TextChanged += new System.EventHandler(this.TxbNumberToFind_TextChanged);
            // 
            // cmbSaveImageFormat
            // 
            this.cmbSaveImageFormat.FormattingEnabled = true;
            this.cmbSaveImageFormat.Location = new System.Drawing.Point(22, 676);
            this.cmbSaveImageFormat.Name = "cmbSaveImageFormat";
            this.cmbSaveImageFormat.Size = new System.Drawing.Size(121, 20);
            this.cmbSaveImageFormat.TabIndex = 16;
            this.cmbSaveImageFormat.SelectedIndexChanged += new System.EventHandler(this.CmbSaveImageFormat_SelectedIndexChanged);
            // 
            // chbSaveOverlayImage
            // 
            this.chbSaveOverlayImage.AutoSize = true;
            this.chbSaveOverlayImage.Location = new System.Drawing.Point(22, 730);
            this.chbSaveOverlayImage.Name = "chbSaveOverlayImage";
            this.chbSaveOverlayImage.Size = new System.Drawing.Size(138, 16);
            this.chbSaveOverlayImage.TabIndex = 15;
            this.chbSaveOverlayImage.Text = "Save Overlay Image";
            this.chbSaveOverlayImage.UseVisualStyleBackColor = true;
            this.chbSaveOverlayImage.CheckedChanged += new System.EventHandler(this.ChbSaveOverlayImage_CheckedChanged);
            // 
            // chbSaveOrgImage
            // 
            this.chbSaveOrgImage.AutoSize = true;
            this.chbSaveOrgImage.Location = new System.Drawing.Point(22, 708);
            this.chbSaveOrgImage.Name = "chbSaveOrgImage";
            this.chbSaveOrgImage.Size = new System.Drawing.Size(115, 16);
            this.chbSaveOrgImage.TabIndex = 14;
            this.chbSaveOrgImage.Text = "Save Org Image";
            this.chbSaveOrgImage.UseVisualStyleBackColor = true;
            this.chbSaveOrgImage.CheckedChanged += new System.EventHandler(this.ChbSaveOrgImage_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSetOverlayImageFolderPath);
            this.groupBox1.Controls.Add(this.btnSetOverlayFolderPath);
            this.groupBox1.Location = new System.Drawing.Point(22, 763);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 100);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "스샷 이미지 저장 경로";
            // 
            // lblSetOverlayImageFolderPath
            // 
            this.lblSetOverlayImageFolderPath.AutoSize = true;
            this.lblSetOverlayImageFolderPath.Location = new System.Drawing.Point(13, 64);
            this.lblSetOverlayImageFolderPath.Name = "lblSetOverlayImageFolderPath";
            this.lblSetOverlayImageFolderPath.Size = new System.Drawing.Size(29, 12);
            this.lblSetOverlayImageFolderPath.TabIndex = 13;
            this.lblSetOverlayImageFolderPath.Text = "경로";
            // 
            // btnSetOverlayFolderPath
            // 
            this.btnSetOverlayFolderPath.Location = new System.Drawing.Point(15, 20);
            this.btnSetOverlayFolderPath.Name = "btnSetOverlayFolderPath";
            this.btnSetOverlayFolderPath.Size = new System.Drawing.Size(126, 23);
            this.btnSetOverlayFolderPath.TabIndex = 12;
            this.btnSetOverlayFolderPath.Text = "Set Folder Path";
            this.btnSetOverlayFolderPath.UseVisualStyleBackColor = true;
            this.btnSetOverlayFolderPath.Click += new System.EventHandler(this.BtnSetOverlayFolderPath_Click);
            // 
            // btnOpenVpp
            // 
            this.btnOpenVpp.Location = new System.Drawing.Point(22, 469);
            this.btnOpenVpp.Name = "btnOpenVpp";
            this.btnOpenVpp.Size = new System.Drawing.Size(126, 23);
            this.btnOpenVpp.TabIndex = 8;
            this.btnOpenVpp.Text = "Open Vpp";
            this.btnOpenVpp.UseVisualStyleBackColor = true;
            this.btnOpenVpp.Click += new System.EventHandler(this.BtnOpenVpp_Click);
            // 
            // btnLoadVppFromFile
            // 
            this.btnLoadVppFromFile.Location = new System.Drawing.Point(22, 498);
            this.btnLoadVppFromFile.Name = "btnLoadVppFromFile";
            this.btnLoadVppFromFile.Size = new System.Drawing.Size(126, 23);
            this.btnLoadVppFromFile.TabIndex = 5;
            this.btnLoadVppFromFile.Text = "Load Vpp";
            this.btnLoadVppFromFile.UseVisualStyleBackColor = true;
            this.btnLoadVppFromFile.Click += new System.EventHandler(this.BtnLoadVppFromFile_Click);
            // 
            // btnLoadImageFromFile
            // 
            this.btnLoadImageFromFile.Location = new System.Drawing.Point(22, 527);
            this.btnLoadImageFromFile.Name = "btnLoadImageFromFile";
            this.btnLoadImageFromFile.Size = new System.Drawing.Size(126, 23);
            this.btnLoadImageFromFile.TabIndex = 4;
            this.btnLoadImageFromFile.Text = "Load Image";
            this.btnLoadImageFromFile.UseVisualStyleBackColor = true;
            this.btnLoadImageFromFile.Click += new System.EventHandler(this.BtnLoadImageFromFile_Click);
            // 
            // pnlDisplay
            // 
            this.pnlDisplay.BackColor = System.Drawing.SystemColors.Control;
            this.pnlDisplay.Controls.Add(this.cogRecordDisplayControl1);
            this.pnlDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDisplay.Location = new System.Drawing.Point(428, 3);
            this.pnlDisplay.Name = "pnlDisplay";
            this.pnlDisplay.Size = new System.Drawing.Size(737, 884);
            this.pnlDisplay.TabIndex = 1;
            // 
            // cogRecordDisplayControl1
            // 
            this.cogRecordDisplayControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cogRecordDisplayControl1.Location = new System.Drawing.Point(0, 0);
            this.cogRecordDisplayControl1.Name = "cogRecordDisplayControl1";
            this.cogRecordDisplayControl1.Size = new System.Drawing.Size(737, 884);
            this.cogRecordDisplayControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 890);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grbCameraParams.ResumeLayout(false);
            this.grbCameraParams.PerformLayout();
            this.grbCameraAction.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlDisplay.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartAcquire;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlDisplay;
        private System.Windows.Forms.Button btnLive;
        private System.Windows.Forms.Button btnStopLive;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnLoadImageFromFile;
        private System.Windows.Forms.Button btnLoadVppFromFile;
        private System.Windows.Forms.Button btnOpenVpp;
        private System.Windows.Forms.Button btnContinueAcquire;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbTriggerInterval;
        private System.Windows.Forms.Button btnSetOverlayFolderPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSetOverlayImageFolderPath;
        private System.Windows.Forms.CheckBox chbSaveOverlayImage;
        private System.Windows.Forms.CheckBox chbSaveOrgImage;
        private System.Windows.Forms.ComboBox cmbSaveImageFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbNumberToFind;
        private System.Windows.Forms.TextBox txbBrightness;
        private System.Windows.Forms.TextBox txbContrast;
        private System.Windows.Forms.TextBox txbExposure;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSetContrast;
        private System.Windows.Forms.Button btnSetBrightness;
        private System.Windows.Forms.Button btnSetExposure;
        private System.Windows.Forms.ComboBox cmbCameraSerialList;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox grbCameraAction;
        private System.Windows.Forms.GroupBox grbCameraParams;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnContinueStop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblSetOriginImageFolderPath;
        private System.Windows.Forms.Button btnSetOriginFolderPath;
        private VisionPro96.Controls.CogRecordDisplayControl cogRecordDisplayControl1;
    }
}



namespace DesignObject
{
    partial class Designer
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
			this.HRuler = new DesignObject.Controls.Ruler();
			this.VRuler = new DesignObject.Controls.Ruler();
			this.ucPalette = new DesignObject.Controls.ucPalette();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.tableLayoutPanel1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Controls.Add(this.vScrollBar1, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.HRuler, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.VRuler, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.ucPalette, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.statusStrip1, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.hScrollBar1, 1, 2);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(673, 585);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// vScrollBar1
			// 
			this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Right;
			this.vScrollBar1.Location = new System.Drawing.Point(656, 45);
			this.vScrollBar1.Name = "vScrollBar1";
			this.vScrollBar1.Size = new System.Drawing.Size(17, 498);
			this.vScrollBar1.TabIndex = 7;
			this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
			this.vScrollBar1.ValueChanged += new System.EventHandler(this.vScrollBar1_ValueChanged);
			// 
			// HRuler
			// 
			this.HRuler.Dock = System.Windows.Forms.DockStyle.Fill;
			this.HRuler.Location = new System.Drawing.Point(3, 48);
			this.HRuler.Name = "HRuler";
			this.HRuler.Size = new System.Drawing.Size(39, 492);
			this.HRuler.TabIndex = 6;
			// 
			// VRuler
			// 
			this.VRuler.Dock = System.Windows.Forms.DockStyle.Fill;
			this.VRuler.Location = new System.Drawing.Point(48, 3);
			this.VRuler.Name = "VRuler";
			this.VRuler.Size = new System.Drawing.Size(602, 39);
			this.VRuler.TabIndex = 5;
			// 
			// ucPalette
			// 
			this.ucPalette.enDraw = DesignObject.enDraw.None;
			this.ucPalette.enMode = DesignObject.enMode.None;
			this.ucPalette.ImeMode = System.Windows.Forms.ImeMode.Alpha;
			this.ucPalette.Location = new System.Drawing.Point(48, 48);
			this.ucPalette.Name = "ucPalette";
			this.ucPalette.Size = new System.Drawing.Size(388, 348);
			this.ucPalette.TabIndex = 8;
			// 
			// statusStrip1
			// 
			this.tableLayoutPanel1.SetColumnSpan(this.statusStrip1, 3);
			this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 563);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(673, 22);
			this.statusStrip1.TabIndex = 9;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(121, 17);
			this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hScrollBar1.Location = new System.Drawing.Point(45, 543);
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(608, 20);
			this.hScrollBar1.TabIndex = 10;
			this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
			this.hScrollBar1.ValueChanged += new System.EventHandler(this.hScrollBar1_ValueChanged);
			// 
			// Designer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "Designer";
			this.Size = new System.Drawing.Size(673, 585);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DesignObject.Controls.Ruler VRuler;
        private DesignObject.Controls.Ruler HRuler;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private DesignObject.Controls.ucPalette ucPalette;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

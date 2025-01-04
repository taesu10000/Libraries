namespace InputHooker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            richTextBox1 = new RichTextBox();
            btnRelease = new Button();
            btnSet = new Button();
            chkShowLog = new CheckBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            chkUseKeyboardHook = new CheckBox();
            chkUseMouseHook = new CheckBox();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(138, 12);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(698, 412);
            richTextBox1.TabIndex = 10;
            richTextBox1.Text = "";
            // 
            // btnRelease
            // 
            btnRelease.Location = new Point(12, 246);
            btnRelease.Name = "btnRelease";
            btnRelease.Size = new Size(120, 178);
            btnRelease.TabIndex = 9;
            btnRelease.Text = "Release";
            btnRelease.UseVisualStyleBackColor = true;
            btnRelease.Click += btnRelease_Click;
            // 
            // btnSet
            // 
            btnSet.Location = new Point(12, 62);
            btnSet.Name = "btnSet";
            btnSet.Size = new Size(120, 178);
            btnSet.TabIndex = 8;
            btnSet.Text = "Set";
            btnSet.UseVisualStyleBackColor = true;
            btnSet.Click += btnSet_Click;
            // 
            // chkShowLog
            // 
            chkShowLog.AutoSize = true;
            chkShowLog.Location = new Point(12, 430);
            chkShowLog.Name = "chkShowLog";
            chkShowLog.Size = new Size(80, 19);
            chkShowLog.TabIndex = 11;
            chkShowLog.Text = "Show Log";
            chkShowLog.UseVisualStyleBackColor = true;
            chkShowLog.CheckedChanged += chkShowLog_CheckedChanged;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 452);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(848, 22);
            statusStrip1.TabIndex = 12;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(121, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // chkUseKeyboardHook
            // 
            chkUseKeyboardHook.AutoSize = true;
            chkUseKeyboardHook.Location = new Point(12, 12);
            chkUseKeyboardHook.Name = "chkUseKeyboardHook";
            chkUseKeyboardHook.Size = new Size(124, 19);
            chkUseKeyboardHook.TabIndex = 11;
            chkUseKeyboardHook.Text = "UseKeyboardHook";
            chkUseKeyboardHook.UseVisualStyleBackColor = true;
            chkUseKeyboardHook.CheckedChanged += chkUseKeyboardHook_CheckedChanged;
            // 
            // chkUseMouseHook
            // 
            chkUseMouseHook.AutoSize = true;
            chkUseMouseHook.Location = new Point(12, 37);
            chkUseMouseHook.Name = "chkUseMouseHook";
            chkUseMouseHook.Size = new Size(110, 19);
            chkUseMouseHook.TabIndex = 11;
            chkUseMouseHook.Text = "UseMouseHook";
            chkUseMouseHook.UseVisualStyleBackColor = true;
            chkUseMouseHook.CheckedChanged += chkUseMouseHook_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(848, 474);
            Controls.Add(statusStrip1);
            Controls.Add(chkUseMouseHook);
            Controls.Add(chkUseKeyboardHook);
            Controls.Add(chkShowLog);
            Controls.Add(richTextBox1);
            Controls.Add(btnRelease);
            Controls.Add(btnSet);
            Name = "Form1";
            Text = "Form1";
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox richTextBox1;
        private Button btnRelease;
        private Button btnSet;
        private CheckBox chkShowLog;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private CheckBox chkUseKeyboardHook;
        private CheckBox chkUseMouseHook;
    }
}

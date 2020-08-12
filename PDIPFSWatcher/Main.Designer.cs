namespace PDIPFSWatcher {
    partial class Main {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.vistaFolderBrowserDialog1 = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage = new System.Windows.Forms.TabPage();
            this.groupBox_actions = new System.Windows.Forms.GroupBox();
            this.checkBox_autoScroll = new System.Windows.Forms.CheckBox();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_stopWatching = new System.Windows.Forms.Button();
            this.button_startWatching = new System.Windows.Forms.Button();
            this.richTextBox = new PDIPFSWatcher.RichTextBoxEx();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.groupBox_actions.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.fileToolStripMenuItem.Text = "Folder";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(35, 17);
            this.toolStripStatusLabel.Text = "Idle...";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 404);
            this.tabControl.TabIndex = 2;
            // 
            // tabPage
            // 
            this.tabPage.Controls.Add(this.richTextBox);
            this.tabPage.Controls.Add(this.groupBox_actions);
            this.tabPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage.Name = "tabPage";
            this.tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage.Size = new System.Drawing.Size(792, 378);
            this.tabPage.TabIndex = 0;
            this.tabPage.Text = "Main";
            this.tabPage.UseVisualStyleBackColor = true;
            // 
            // groupBox_actions
            // 
            this.groupBox_actions.Controls.Add(this.checkBox_autoScroll);
            this.groupBox_actions.Controls.Add(this.button_clear);
            this.groupBox_actions.Controls.Add(this.button_stopWatching);
            this.groupBox_actions.Controls.Add(this.button_startWatching);
            this.groupBox_actions.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_actions.Location = new System.Drawing.Point(3, 3);
            this.groupBox_actions.Name = "groupBox_actions";
            this.groupBox_actions.Size = new System.Drawing.Size(786, 43);
            this.groupBox_actions.TabIndex = 0;
            this.groupBox_actions.TabStop = false;
            this.groupBox_actions.Text = "Actions";
            // 
            // checkBox_autoScroll
            // 
            this.checkBox_autoScroll.AutoSize = true;
            this.checkBox_autoScroll.Checked = true;
            this.checkBox_autoScroll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_autoScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBox_autoScroll.Location = new System.Drawing.Point(611, 16);
            this.checkBox_autoScroll.Name = "checkBox_autoScroll";
            this.checkBox_autoScroll.Size = new System.Drawing.Size(75, 24);
            this.checkBox_autoScroll.TabIndex = 3;
            this.checkBox_autoScroll.Text = "Auto scroll";
            this.checkBox_autoScroll.UseVisualStyleBackColor = true;
            // 
            // button_clear
            // 
            this.button_clear.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_clear.Location = new System.Drawing.Point(686, 16);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(97, 24);
            this.button_clear.TabIndex = 2;
            this.button_clear.Text = "Clear log";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_stopWatching
            // 
            this.button_stopWatching.Location = new System.Drawing.Point(109, 19);
            this.button_stopWatching.Name = "button_stopWatching";
            this.button_stopWatching.Size = new System.Drawing.Size(97, 23);
            this.button_stopWatching.TabIndex = 1;
            this.button_stopWatching.Text = "Stop watching";
            this.button_stopWatching.UseVisualStyleBackColor = true;
            this.button_stopWatching.Click += new System.EventHandler(this.button_stopWatching_Click);
            // 
            // button_startWatching
            // 
            this.button_startWatching.Location = new System.Drawing.Point(6, 19);
            this.button_startWatching.Name = "button_startWatching";
            this.button_startWatching.Size = new System.Drawing.Size(97, 23);
            this.button_startWatching.TabIndex = 0;
            this.button_startWatching.Text = "Start watching";
            this.button_startWatching.UseVisualStyleBackColor = true;
            this.button_startWatching.Click += new System.EventHandler(this.button_startWatching_Click);
            // 
            // richTextBox
            // 
            this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox.Location = new System.Drawing.Point(3, 46);
            this.richTextBox.Name = "richTextBox";
            this.richTextBox.Size = new System.Drawing.Size(786, 329);
            this.richTextBox.TabIndex = 1;
            this.richTextBox.Text = "";
            this.richTextBox.TextChanged += new System.EventHandler(this.richTextBox_TextChanged);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "PDIPFS File Watcher by xfileFIN";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage.ResumeLayout(false);
            this.groupBox_actions.ResumeLayout(false);
            this.groupBox_actions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Ookii.Dialogs.WinForms.VistaFolderBrowserDialog vistaFolderBrowserDialog1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.GroupBox groupBox_actions;
        private System.Windows.Forms.Button button_startWatching;
        private System.Windows.Forms.Button button_stopWatching;
        private RichTextBoxEx richTextBox;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.CheckBox checkBox_autoScroll;
    }
}


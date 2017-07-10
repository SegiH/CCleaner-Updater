


namespace CCleaner_Updater {
    using System.Windows.Forms;

    partial class CCleanerUpdater {
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

        //#region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CCleanerUpdater));
            this.label1 = new System.Windows.Forms.Label();
            this.chkUpdatePrompt = new System.Windows.Forms.CheckBox();
            this.txtCCleanerPath = new System.Windows.Forms.TextBox();
            this.btnSelectCCleanerPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCheckUpdatesNow = new System.Windows.Forms.Button();
            this.installedVersion = new System.Windows.Forms.Label();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "CCleaner Updater\r\n\r\nWritten by Segi Hovav";
            // 
            // chkUpdatePrompt
            // 
            this.chkUpdatePrompt.AutoSize = true;
            this.chkUpdatePrompt.Location = new System.Drawing.Point(12, 143);
            this.chkUpdatePrompt.Name = "chkUpdatePrompt";
            this.chkUpdatePrompt.Size = new System.Drawing.Size(184, 17);
            this.chkUpdatePrompt.TabIndex = 1;
            this.chkUpdatePrompt.Text = "Ask me before updating CCleaner";
            this.chkUpdatePrompt.UseVisualStyleBackColor = true;
            this.chkUpdatePrompt.CheckedChanged += new System.EventHandler(this.chkUpdatePrompt_CheckedChanged);
            // 
            // txtCCleanerPath
            // 
            this.txtCCleanerPath.Location = new System.Drawing.Point(12, 198);
            this.txtCCleanerPath.Name = "txtCCleanerPath";
            this.txtCCleanerPath.Size = new System.Drawing.Size(378, 20);
            this.txtCCleanerPath.TabIndex = 2;
            // 
            // btnSelectCCleanerPath
            // 
            this.btnSelectCCleanerPath.Location = new System.Drawing.Point(416, 195);
            this.btnSelectCCleanerPath.Name = "btnSelectCCleanerPath";
            this.btnSelectCCleanerPath.Size = new System.Drawing.Size(75, 23);
            this.btnSelectCCleanerPath.TabIndex = 3;
            this.btnSelectCCleanerPath.Text = "Browse";
            this.btnSelectCCleanerPath.UseVisualStyleBackColor = true;
            this.btnSelectCCleanerPath.Click += new System.EventHandler(this.btnSelectCCleanerPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Specify location of CCleaner.exe";
            // 
            // btnCheckUpdatesNow
            // 
            this.btnCheckUpdatesNow.Location = new System.Drawing.Point(15, 235);
            this.btnCheckUpdatesNow.Name = "btnCheckUpdatesNow";
            this.btnCheckUpdatesNow.Size = new System.Drawing.Size(136, 23);
            this.btnCheckUpdatesNow.TabIndex = 5;
            this.btnCheckUpdatesNow.Text = "Check for updates now";
            this.btnCheckUpdatesNow.UseVisualStyleBackColor = true;
            this.btnCheckUpdatesNow.Click += new System.EventHandler(this.btnCheckUpdatesNow_Click);
            // 
            // installedVersion
            // 
            this.installedVersion.AutoSize = true;
            this.installedVersion.Location = new System.Drawing.Point(12, 88);
            this.installedVersion.Name = "installedVersion";
            this.installedVersion.Size = new System.Drawing.Size(171, 13);
            this.installedVersion.TabIndex = 6;
            this.installedVersion.Text = "CCleaner version on this computer:";
            // 
            // lblCurrentVersion
            // 
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Location = new System.Drawing.Point(145, 88);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(0, 13);
            this.lblCurrentVersion.TabIndex = 7;
            // 
            // CCleanerUpdater
            // 
            this.ClientSize = new System.Drawing.Size(664, 279);
            this.Controls.Add(this.lblCurrentVersion);
            this.Controls.Add(this.installedVersion);
            this.Controls.Add(this.btnCheckUpdatesNow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectCCleanerPath);
            this.Controls.Add(this.txtCCleanerPath);
            this.Controls.Add(this.chkUpdatePrompt);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CCleanerUpdater";
            this.Text = "CCleaner Updater 1.1";
            this.Load += new System.EventHandler(this.CCleanerUpdater_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label label1;
        private CheckBox chkUpdatePrompt;
        private TextBox txtCCleanerPath;
        private Button btnSelectCCleanerPath;
        private Label label2;
        private Button btnCheckUpdatesNow;
        private Label installedVersion;
        private Label lblCurrentVersion;
    }
}
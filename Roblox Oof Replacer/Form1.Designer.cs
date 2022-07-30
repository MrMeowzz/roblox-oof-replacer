namespace Roblox_Oof_Replacer
{
    partial class RobloxOofReplacer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobloxOofReplacer));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.version = new System.Windows.Forms.Label();
            this.versionToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button7 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(12, 197);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 46);
            this.button1.TabIndex = 0;
            this.button1.Text = "Replace!";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button2.Location = new System.Drawing.Point(298, 197);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(181, 46);
            this.button2.TabIndex = 1;
            this.button2.Text = "Reset All";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button3.Location = new System.Drawing.Point(12, 34);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(84, 46);
            this.button3.TabIndex = 2;
            this.button3.Text = "Pick Roblox Shortcut";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button4.Location = new System.Drawing.Point(12, 115);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(181, 46);
            this.button4.TabIndex = 5;
            this.button4.Text = "Pick Sound";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AllowDrop = true;
            this.label2.AutoEllipsis = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.ContextMenuStrip = this.contextMenuStrip1;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(199, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Old Oof";
            this.label2.DragDrop += new System.Windows.Forms.DragEventHandler(this.label2_DragDrop);
            this.label2.DragEnter += new System.Windows.Forms.DragEventHandler(this.label2_DragEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyPathToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(130, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // copyPathToolStripMenuItem
            // 
            this.copyPathToolStripMenuItem.Name = "copyPathToolStripMenuItem";
            this.copyPathToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.copyPathToolStripMenuItem.Text = "Copy Path";
            // 
            // openFileDialog2
            // 
            this.openFileDialog2.DefaultExt = "ogg";
            this.openFileDialog2.DereferenceLinks = false;
            this.openFileDialog2.Filter = "Sound files (*.ogg;*.mp3;*.wav)|*.ogg;*.mp3;*.wav";
            this.openFileDialog2.RestoreDirectory = true;
            this.openFileDialog2.Title = "Pick Sound";
            this.openFileDialog2.FileOk += new System.ComponentModel.CancelEventHandler(this.DisallowUrlExtensions);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "lnk";
            this.openFileDialog1.DereferenceLinks = false;
            this.openFileDialog1.Filter = "Shortcut files (*.lnk)|*.lnk";
            this.openFileDialog1.RestoreDirectory = true;
            this.openFileDialog1.Title = "Pick Roblox Shortcut";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.DisallowUrlExtensions);
            // 
            // version
            // 
            this.version.AutoSize = true;
            this.version.Location = new System.Drawing.Point(12, 9);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(34, 15);
            this.version.TabIndex = 9;
            this.version.Text = "v?.?.?";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(395, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "By Mr Meowzz";
            // 
            // button5
            // 
            this.button5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button5.AutoSize = true;
            this.button5.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button5.Location = new System.Drawing.Point(447, 119);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(40, 42);
            this.button5.TabIndex = 12;
            this.button5.Text = "X";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button6.AutoSize = true;
            this.button6.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.button6.Location = new System.Drawing.Point(447, 34);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(40, 42);
            this.button6.TabIndex = 13;
            this.button6.Text = "X";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Choose Roblox folder (Should be in AppData/Local/Roblox/Versions/(version you wan" +
    "t) )";
            this.folderBrowserDialog1.InitialDirectory = "C:\\Users\\mjher\\AppData\\Local";
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.LocalApplicationData;
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            this.folderBrowserDialog1.UseDescriptionForTitle = true;
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button7.Location = new System.Drawing.Point(102, 34);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(84, 46);
            this.button7.TabIndex = 14;
            this.button7.Text = "Pick Roblox Folder";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // label1
            // 
            this.label1.AllowDrop = true;
            this.label1.AutoEllipsis = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.ContextMenuStrip = this.contextMenuStrip1;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(199, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(242, 23);
            this.label1.TabIndex = 7;
            this.label1.DragDrop += new System.Windows.Forms.DragEventHandler(this.label1_DragDrop);
            this.label1.DragEnter += new System.Windows.Forms.DragEventHandler(this.label1_DragEnter);
            // 
            // RobloxOofReplacer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 255);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.version);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RobloxOofReplacer";
            this.Text = "Roblox Oof Replacer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RobloxOofReplacer_FormClosing);
            this.Load += new System.EventHandler(this.RobloxOofReplacer_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label version;
        private System.Windows.Forms.ToolTip versionToolTip;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyPathToolStripMenuItem;
        private System.Windows.Forms.Label label1;
    }
}


namespace JidamVision
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileTopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModelNewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModelOpenMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModelSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModelSaveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ImageLoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageSaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetupTopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileTopMenuItem,
            this.SetupTopMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 640);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1143, 35);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // FileTopMenuItem
            // 
            this.FileTopMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ModelNewMenuItem,
            this.ModelOpenMenuItem,
            this.ModelSaveMenuItem,
            this.ModelSaveAsMenuItem,
            this.toolStripSeparator1,
            this.ImageLoadMenuItem,
            this.ImageSaveMenuItem});
            this.FileTopMenuItem.Name = "FileTopMenuItem";
            this.FileTopMenuItem.Size = new System.Drawing.Size(55, 29);
            this.FileTopMenuItem.Text = "File";
            // 
            // ModelNewMenuItem
            // 
            this.ModelNewMenuItem.Name = "ModelNewMenuItem";
            this.ModelNewMenuItem.Size = new System.Drawing.Size(270, 34);
            this.ModelNewMenuItem.Text = "Model New";
            this.ModelNewMenuItem.Click += new System.EventHandler(this.ModelNewMenuItem_Click);
            // 
            // ModelOpenMenuItem
            // 
            this.ModelOpenMenuItem.Name = "ModelOpenMenuItem";
            this.ModelOpenMenuItem.Size = new System.Drawing.Size(270, 34);
            this.ModelOpenMenuItem.Text = "Model Open";
            this.ModelOpenMenuItem.Click += new System.EventHandler(this.ModelOpenMenuItem_Click);
            // 
            // ModelSaveMenuItem
            // 
            this.ModelSaveMenuItem.Name = "ModelSaveMenuItem";
            this.ModelSaveMenuItem.Size = new System.Drawing.Size(270, 34);
            this.ModelSaveMenuItem.Text = "Model Save";
            this.ModelSaveMenuItem.Click += new System.EventHandler(this.ModelSaveMenuItem_Click);
            // 
            // ModelSaveAsMenuItem
            // 
            this.ModelSaveAsMenuItem.Name = "ModelSaveAsMenuItem";
            this.ModelSaveAsMenuItem.Size = new System.Drawing.Size(270, 34);
            this.ModelSaveAsMenuItem.Text = "Model SaveAs";
            this.ModelSaveAsMenuItem.Click += new System.EventHandler(this.ModelSaveAsMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(267, 6);
            // 
            // ImageLoadMenuItem
            // 
            this.ImageLoadMenuItem.Name = "ImageLoadMenuItem";
            this.ImageLoadMenuItem.Size = new System.Drawing.Size(270, 34);
            this.ImageLoadMenuItem.Text = "Image Load";
            this.ImageLoadMenuItem.Click += new System.EventHandler(this.ImageLoadMenuItem_Click);
            // 
            // ImageSaveMenuItem
            // 
            this.ImageSaveMenuItem.Name = "ImageSaveMenuItem";
            this.ImageSaveMenuItem.Size = new System.Drawing.Size(270, 34);
            this.ImageSaveMenuItem.Text = "Image Save";
            this.ImageSaveMenuItem.Click += new System.EventHandler(this.ImageSaveMenuItem_Click);
            // 
            // SetupTopMenuItem
            // 
            this.SetupTopMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetupMenuItem});
            this.SetupTopMenuItem.Name = "SetupTopMenuItem";
            this.SetupTopMenuItem.Size = new System.Drawing.Size(75, 29);
            this.SetupTopMenuItem.Text = "Setup";
            // 
            // SetupMenuItem
            // 
            this.SetupMenuItem.Name = "SetupMenuItem";
            this.SetupMenuItem.Size = new System.Drawing.Size(161, 34);
            this.SetupMenuItem.Text = "Setup";
            this.SetupMenuItem.Click += new System.EventHandler(this.SetupMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(481, 298);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(8, 8);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(0, 0);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(0, 0);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1143, 675);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileTopMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetupTopMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SetupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ModelOpenMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ModelSaveMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ModelSaveAsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImageLoadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImageSaveMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ModelNewMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}
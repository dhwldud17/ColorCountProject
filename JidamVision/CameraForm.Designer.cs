﻿namespace JidamVision
{
    partial class CameraForm
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
            this.btnGrab = new System.Windows.Forms.Button();
            this.btnLive = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtnColor = new System.Windows.Forms.RadioButton();
            this.rbtnGrayChannel = new System.Windows.Forms.RadioButton();
            this.rbtnGreenChannel = new System.Windows.Forms.RadioButton();
            this.rbtnBlueChannel = new System.Windows.Forms.RadioButton();
            this.rbtnRedChannel = new System.Windows.Forms.RadioButton();
            this.btnInspect = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.chkCycle = new System.Windows.Forms.CheckBox();
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.chkShowROI = new System.Windows.Forms.CheckBox();
            this.imageViewer = new JidamVision.ImageViewCCtrl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGrab
            // 
            this.btnGrab.Location = new System.Drawing.Point(388, 12);
            this.btnGrab.Name = "btnGrab";
            this.btnGrab.Size = new System.Drawing.Size(75, 23);
            this.btnGrab.TabIndex = 1;
            this.btnGrab.Text = "Grab";
            this.btnGrab.UseVisualStyleBackColor = true;
            this.btnGrab.Click += new System.EventHandler(this.btnGrab_Click);
            // 
            // btnLive
            // 
            this.btnLive.Location = new System.Drawing.Point(388, 41);
            this.btnLive.Name = "btnLive";
            this.btnLive.Size = new System.Drawing.Size(75, 23);
            this.btnLive.TabIndex = 3;
            this.btnLive.Text = "Live";
            this.btnLive.UseVisualStyleBackColor = true;
            this.btnLive.Click += new System.EventHandler(this.btnLive_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnColor);
            this.groupBox1.Controls.Add(this.rbtnGrayChannel);
            this.groupBox1.Controls.Add(this.rbtnGreenChannel);
            this.groupBox1.Controls.Add(this.rbtnBlueChannel);
            this.groupBox1.Controls.Add(this.rbtnRedChannel);
            this.groupBox1.Location = new System.Drawing.Point(389, 209);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(74, 122);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Channel";
            // 
            // rbtnColor
            // 
            this.rbtnColor.AutoSize = true;
            this.rbtnColor.Location = new System.Drawing.Point(7, 19);
            this.rbtnColor.Name = "rbtnColor";
            this.rbtnColor.Size = new System.Drawing.Size(53, 16);
            this.rbtnColor.TabIndex = 4;
            this.rbtnColor.TabStop = true;
            this.rbtnColor.Text = "Color";
            this.rbtnColor.UseVisualStyleBackColor = true;
            this.rbtnColor.CheckedChanged += new System.EventHandler(this.rbtnColor_CheckedChanged);
            // 
            // rbtnGrayChannel
            // 
            this.rbtnGrayChannel.AutoSize = true;
            this.rbtnGrayChannel.Location = new System.Drawing.Point(7, 100);
            this.rbtnGrayChannel.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnGrayChannel.Name = "rbtnGrayChannel";
            this.rbtnGrayChannel.Size = new System.Drawing.Size(50, 16);
            this.rbtnGrayChannel.TabIndex = 3;
            this.rbtnGrayChannel.TabStop = true;
            this.rbtnGrayChannel.Text = "Gray";
            this.rbtnGrayChannel.UseVisualStyleBackColor = true;
            // 
            // rbtnGreenChannel
            // 
            this.rbtnGreenChannel.AutoSize = true;
            this.rbtnGreenChannel.Location = new System.Drawing.Point(7, 80);
            this.rbtnGreenChannel.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnGreenChannel.Name = "rbtnGreenChannel";
            this.rbtnGreenChannel.Size = new System.Drawing.Size(57, 16);
            this.rbtnGreenChannel.TabIndex = 2;
            this.rbtnGreenChannel.TabStop = true;
            this.rbtnGreenChannel.Text = "Green";
            this.rbtnGreenChannel.UseVisualStyleBackColor = true;
            this.rbtnGreenChannel.CheckedChanged += new System.EventHandler(this.rbtnGreenChannel_CheckedChanged);
            // 
            // rbtnBlueChannel
            // 
            this.rbtnBlueChannel.AutoSize = true;
            this.rbtnBlueChannel.Location = new System.Drawing.Point(7, 60);
            this.rbtnBlueChannel.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnBlueChannel.Name = "rbtnBlueChannel";
            this.rbtnBlueChannel.Size = new System.Drawing.Size(48, 16);
            this.rbtnBlueChannel.TabIndex = 1;
            this.rbtnBlueChannel.TabStop = true;
            this.rbtnBlueChannel.Text = "Blue";
            this.rbtnBlueChannel.UseVisualStyleBackColor = true;
            this.rbtnBlueChannel.CheckedChanged += new System.EventHandler(this.rbtnBlueChannel_CheckedChanged);
            // 
            // rbtnRedChannel
            // 
            this.rbtnRedChannel.AutoSize = true;
            this.rbtnRedChannel.Location = new System.Drawing.Point(7, 40);
            this.rbtnRedChannel.Margin = new System.Windows.Forms.Padding(2);
            this.rbtnRedChannel.Name = "rbtnRedChannel";
            this.rbtnRedChannel.Size = new System.Drawing.Size(45, 16);
            this.rbtnRedChannel.TabIndex = 0;
            this.rbtnRedChannel.TabStop = true;
            this.rbtnRedChannel.Text = "Red";
            this.rbtnRedChannel.UseVisualStyleBackColor = true;
            this.rbtnRedChannel.CheckedChanged += new System.EventHandler(this.rbtnRedChannel_CheckedChanged);
            // 
            // btnInspect
            // 
            this.btnInspect.Location = new System.Drawing.Point(388, 70);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(75, 24);
            this.btnInspect.TabIndex = 6;
            this.btnInspect.Text = "검사";
            this.btnInspect.UseVisualStyleBackColor = true;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(388, 100);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 27);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "정지";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // chkCycle
            // 
            this.chkCycle.AutoSize = true;
            this.chkCycle.Location = new System.Drawing.Point(389, 133);
            this.chkCycle.Name = "chkCycle";
            this.chkCycle.Size = new System.Drawing.Size(57, 16);
            this.chkCycle.TabIndex = 8;
            this.chkCycle.Text = "Cycle";
            this.chkCycle.UseVisualStyleBackColor = true;
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Location = new System.Drawing.Point(388, 155);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(69, 16);
            this.chkPreview.TabIndex = 9;
            this.chkPreview.Text = "Preview";
            this.chkPreview.UseVisualStyleBackColor = true;
            this.chkPreview.CheckedChanged += new System.EventHandler(this.chkPreview_CheckedChanged);
            // 
            // chkShowROI
            // 
            this.chkShowROI.AutoSize = true;
            this.chkShowROI.Checked = true;
            this.chkShowROI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowROI.Location = new System.Drawing.Point(388, 177);
            this.chkShowROI.Name = "chkShowROI";
            this.chkShowROI.Size = new System.Drawing.Size(80, 16);
            this.chkShowROI.TabIndex = 9;
            this.chkShowROI.Text = "Show ROI";
            this.chkShowROI.UseVisualStyleBackColor = true;
            this.chkShowROI.CheckedChanged += new System.EventHandler(this.chkShowROI_CheckedChanged);
            // 
            // imageViewer
            // 
            this.imageViewer.AutoSize = true;
            this.imageViewer.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageViewer.Location = new System.Drawing.Point(12, 12);
            this.imageViewer.Margin = new System.Windows.Forms.Padding(4);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(369, 319);
            this.imageViewer.TabIndex = 2;
            // 
            // CameraForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 342);
            this.Controls.Add(this.chkShowROI);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.chkCycle);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnInspect);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLive);
            this.Controls.Add(this.btnGrab);
            this.Name = "CameraForm";
            this.Text = "CameraForm";
            this.Load += new System.EventHandler(this.CameraForm_Load);
            this.Resize += new System.EventHandler(this.CameraForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGrab;
        private ImageViewCCtrl imageViewer;
        private System.Windows.Forms.Button btnLive;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnRedChannel;
        private System.Windows.Forms.RadioButton rbtnGrayChannel;
        private System.Windows.Forms.RadioButton rbtnGreenChannel;
        private System.Windows.Forms.RadioButton rbtnBlueChannel;
        private System.Windows.Forms.Button btnInspect;
        private System.Windows.Forms.RadioButton rbtnColor;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox chkCycle;
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.CheckBox chkShowROI;
    }
}
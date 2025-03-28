namespace JidamVision.Property
{
    partial class BinaryInspProp
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
                if (trackBarLower != null)
                    trackBarLower.ValueChanged -= OnValueChanged;

                if (trackBarUpper != null)
                    trackBarUpper.ValueChanged -= OnValueChanged;

                if (txtAreaMin != null)
                    txtAreaMin.Leave -= OnFilterChanged;

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpBinary = new System.Windows.Forms.GroupBox();
            this.chkShowBinary = new System.Windows.Forms.CheckBox();
            this.chkInvert = new System.Windows.Forms.CheckBox();
            this.chkHighlight = new System.Windows.Forms.CheckBox();
            this.trackBarUpper = new System.Windows.Forms.TrackBar();
            this.trackBarLower = new System.Windows.Forms.TrackBar();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.lbCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHeightMax = new System.Windows.Forms.TextBox();
            this.txtWidthMax = new System.Windows.Forms.TextBox();
            this.txtAreaMax = new System.Windows.Forms.TextBox();
            this.txtHeightMin = new System.Windows.Forms.TextBox();
            this.lbHeight = new System.Windows.Forms.Label();
            this.txtWidthMin = new System.Windows.Forms.TextBox();
            this.lbWidth = new System.Windows.Forms.Label();
            this.txtAreaMin = new System.Windows.Forms.TextBox();
            this.lbArea = new System.Windows.Forms.Label();
            this.grpBinary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLower)).BeginInit();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBinary
            // 
            this.grpBinary.Controls.Add(this.chkShowBinary);
            this.grpBinary.Controls.Add(this.chkInvert);
            this.grpBinary.Controls.Add(this.chkHighlight);
            this.grpBinary.Controls.Add(this.trackBarUpper);
            this.grpBinary.Controls.Add(this.trackBarLower);
            this.grpBinary.Location = new System.Drawing.Point(3, 3);
            this.grpBinary.Name = "grpBinary";
            this.grpBinary.Size = new System.Drawing.Size(250, 172);
            this.grpBinary.TabIndex = 0;
            this.grpBinary.TabStop = false;
            this.grpBinary.Text = "이진화";
            // 
            // chkShowBinary
            // 
            this.chkShowBinary.AutoSize = true;
            this.chkShowBinary.Location = new System.Drawing.Point(125, 125);
            this.chkShowBinary.Name = "chkShowBinary";
            this.chkShowBinary.Size = new System.Drawing.Size(60, 16);
            this.chkShowBinary.TabIndex = 5;
            this.chkShowBinary.Text = "이진화";
            this.chkShowBinary.UseVisualStyleBackColor = true;
            this.chkShowBinary.CheckedChanged += new System.EventHandler(this.chkBinaryOnly_CheckedChanged);
            // 
            // chkInvert
            // 
            this.chkInvert.AutoSize = true;
            this.chkInvert.Location = new System.Drawing.Point(23, 148);
            this.chkInvert.Name = "chkInvert";
            this.chkInvert.Size = new System.Drawing.Size(48, 16);
            this.chkInvert.TabIndex = 4;
            this.chkInvert.Text = "반전";
            this.chkInvert.UseVisualStyleBackColor = true;
            this.chkInvert.CheckedChanged += new System.EventHandler(this.chkInvert_CheckedChanged);
            // 
            // chkHighlight
            // 
            this.chkHighlight.AutoSize = true;
            this.chkHighlight.Checked = true;
            this.chkHighlight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHighlight.Location = new System.Drawing.Point(23, 125);
            this.chkHighlight.Name = "chkHighlight";
            this.chkHighlight.Size = new System.Drawing.Size(72, 16);
            this.chkHighlight.TabIndex = 3;
            this.chkHighlight.Text = "Highlight";
            this.chkHighlight.UseVisualStyleBackColor = true;
            this.chkHighlight.CheckedChanged += new System.EventHandler(this.chkHighlight_CheckedChanged);
            // 
            // trackBarUpper
            // 
            this.trackBarUpper.Location = new System.Drawing.Point(23, 74);
            this.trackBarUpper.Maximum = 255;
            this.trackBarUpper.Name = "trackBarUpper";
            this.trackBarUpper.Size = new System.Drawing.Size(219, 45);
            this.trackBarUpper.TabIndex = 1;
            this.trackBarUpper.Value = 255;
            // 
            // trackBarLower
            // 
            this.trackBarLower.Location = new System.Drawing.Point(23, 23);
            this.trackBarLower.Maximum = 255;
            this.trackBarLower.Name = "trackBarLower";
            this.trackBarLower.Size = new System.Drawing.Size(219, 45);
            this.trackBarLower.TabIndex = 0;
            // 
            // grpFilter
            // 
            this.grpFilter.Controls.Add(this.txtCount);
            this.grpFilter.Controls.Add(this.lbCount);
            this.grpFilter.Controls.Add(this.label5);
            this.grpFilter.Controls.Add(this.label3);
            this.grpFilter.Controls.Add(this.label1);
            this.grpFilter.Controls.Add(this.txtHeightMax);
            this.grpFilter.Controls.Add(this.txtWidthMax);
            this.grpFilter.Controls.Add(this.txtAreaMax);
            this.grpFilter.Controls.Add(this.txtHeightMin);
            this.grpFilter.Controls.Add(this.lbHeight);
            this.grpFilter.Controls.Add(this.txtWidthMin);
            this.grpFilter.Controls.Add(this.lbWidth);
            this.grpFilter.Controls.Add(this.txtAreaMin);
            this.grpFilter.Controls.Add(this.lbArea);
            this.grpFilter.Location = new System.Drawing.Point(4, 192);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(249, 139);
            this.grpFilter.TabIndex = 1;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "필터";
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(72, 100);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(57, 21);
            this.txtCount.TabIndex = 5;
            // 
            // lbCount
            // 
            this.lbCount.AutoSize = true;
            this.lbCount.Location = new System.Drawing.Point(20, 103);
            this.lbCount.Name = "lbCount";
            this.lbCount.Size = new System.Drawing.Size(38, 12);
            this.lbCount.TabIndex = 4;
            this.lbCount.Text = "Count";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(135, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "~";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(135, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "~";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "~";
            // 
            // txtHeightMax
            // 
            this.txtHeightMax.Location = new System.Drawing.Point(155, 72);
            this.txtHeightMax.Name = "txtHeightMax";
            this.txtHeightMax.Size = new System.Drawing.Size(55, 21);
            this.txtHeightMax.TabIndex = 2;
            // 
            // txtWidthMax
            // 
            this.txtWidthMax.Location = new System.Drawing.Point(155, 45);
            this.txtWidthMax.Name = "txtWidthMax";
            this.txtWidthMax.Size = new System.Drawing.Size(55, 21);
            this.txtWidthMax.TabIndex = 2;
            // 
            // txtAreaMax
            // 
            this.txtAreaMax.Location = new System.Drawing.Point(155, 18);
            this.txtAreaMax.Name = "txtAreaMax";
            this.txtAreaMax.Size = new System.Drawing.Size(55, 21);
            this.txtAreaMax.TabIndex = 2;
            // 
            // txtHeightMin
            // 
            this.txtHeightMin.Location = new System.Drawing.Point(72, 72);
            this.txtHeightMin.Name = "txtHeightMin";
            this.txtHeightMin.Size = new System.Drawing.Size(57, 21);
            this.txtHeightMin.TabIndex = 1;
            // 
            // lbHeight
            // 
            this.lbHeight.AutoSize = true;
            this.lbHeight.Location = new System.Drawing.Point(20, 75);
            this.lbHeight.Name = "lbHeight";
            this.lbHeight.Size = new System.Drawing.Size(40, 12);
            this.lbHeight.TabIndex = 0;
            this.lbHeight.Text = "Height";
            // 
            // txtWidthMin
            // 
            this.txtWidthMin.Location = new System.Drawing.Point(72, 45);
            this.txtWidthMin.Name = "txtWidthMin";
            this.txtWidthMin.Size = new System.Drawing.Size(57, 21);
            this.txtWidthMin.TabIndex = 1;
            // 
            // lbWidth
            // 
            this.lbWidth.AutoSize = true;
            this.lbWidth.Location = new System.Drawing.Point(20, 48);
            this.lbWidth.Name = "lbWidth";
            this.lbWidth.Size = new System.Drawing.Size(35, 12);
            this.lbWidth.TabIndex = 0;
            this.lbWidth.Text = "Width";
            // 
            // txtAreaMin
            // 
            this.txtAreaMin.Location = new System.Drawing.Point(72, 18);
            this.txtAreaMin.Name = "txtAreaMin";
            this.txtAreaMin.Size = new System.Drawing.Size(57, 21);
            this.txtAreaMin.TabIndex = 1;
            // 
            // lbArea
            // 
            this.lbArea.AutoSize = true;
            this.lbArea.Location = new System.Drawing.Point(20, 21);
            this.lbArea.Name = "lbArea";
            this.lbArea.Size = new System.Drawing.Size(31, 12);
            this.lbArea.TabIndex = 0;
            this.lbArea.Text = "Area";
            // 
            // BinaryInspProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.grpBinary);
            this.Name = "BinaryInspProp";
            this.Size = new System.Drawing.Size(271, 357);
            this.grpBinary.ResumeLayout(false);
            this.grpBinary.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLower)).EndInit();
            this.grpFilter.ResumeLayout(false);
            this.grpFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBinary;
        private System.Windows.Forms.TrackBar trackBarUpper;
        private System.Windows.Forms.TrackBar trackBarLower;
        private System.Windows.Forms.CheckBox chkHighlight;
        private System.Windows.Forms.CheckBox chkInvert;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.TextBox txtAreaMin;
        private System.Windows.Forms.Label lbArea;
        private System.Windows.Forms.CheckBox chkShowBinary;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHeightMax;
        private System.Windows.Forms.TextBox txtWidthMax;
        private System.Windows.Forms.TextBox txtAreaMax;
        private System.Windows.Forms.TextBox txtHeightMin;
        private System.Windows.Forms.Label lbHeight;
        private System.Windows.Forms.TextBox txtWidthMin;
        private System.Windows.Forms.Label lbWidth;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label lbCount;
    }
}

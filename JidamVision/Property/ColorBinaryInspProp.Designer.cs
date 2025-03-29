namespace JidamVision.Property
{
    partial class ColorBinaryInspProp
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
            this.grpHSV = new System.Windows.Forms.GroupBox();
            this.panelColor = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.hTrackBarUpper = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.vTrackBarUpper = new System.Windows.Forms.TrackBar();
            this.chkShowColorBinaryOnly = new System.Windows.Forms.CheckBox();
            this.chkInvert = new System.Windows.Forms.CheckBox();
            this.btnApplyHSV = new System.Windows.Forms.Button();
            this.txtV = new System.Windows.Forms.TextBox();
            this.txtS = new System.Windows.Forms.TextBox();
            this.txtH = new System.Windows.Forms.TextBox();
            this.panelColorPreview = new System.Windows.Forms.Panel();
            this.btnTeachinColor = new System.Windows.Forms.Button();
            this.chkHighlight = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.vTrackBarLower = new System.Windows.Forms.TrackBar();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblSaturation = new System.Windows.Forms.Label();
            this.lblHue = new System.Windows.Forms.Label();
            this.sTrackBarUpper = new System.Windows.Forms.TrackBar();
            this.hTrackBarLower = new System.Windows.Forms.TrackBar();
            this.sTrackBarLower = new System.Windows.Forms.TrackBar();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.select_effect2 = new System.Windows.Forms.ComboBox();
            this.select_effect = new System.Windows.Forms.ComboBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.grpHSV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hTrackBarUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vTrackBarUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vTrackBarLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTrackBarUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hTrackBarLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTrackBarLower)).BeginInit();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpHSV
            // 
            this.grpHSV.Controls.Add(this.panelColor);
            this.grpHSV.Controls.Add(this.label8);
            this.grpHSV.Controls.Add(this.label11);
            this.grpHSV.Controls.Add(this.hTrackBarUpper);
            this.grpHSV.Controls.Add(this.label3);
            this.grpHSV.Controls.Add(this.label5);
            this.grpHSV.Controls.Add(this.label1);
            this.grpHSV.Controls.Add(this.label2);
            this.grpHSV.Controls.Add(this.vTrackBarUpper);
            this.grpHSV.Controls.Add(this.chkShowColorBinaryOnly);
            this.grpHSV.Controls.Add(this.chkInvert);
            this.grpHSV.Controls.Add(this.btnApplyHSV);
            this.grpHSV.Controls.Add(this.txtV);
            this.grpHSV.Controls.Add(this.txtS);
            this.grpHSV.Controls.Add(this.txtH);
            this.grpHSV.Controls.Add(this.panelColorPreview);
            this.grpHSV.Controls.Add(this.btnTeachinColor);
            this.grpHSV.Controls.Add(this.chkHighlight);
            this.grpHSV.Controls.Add(this.label10);
            this.grpHSV.Controls.Add(this.label12);
            this.grpHSV.Controls.Add(this.label7);
            this.grpHSV.Controls.Add(this.label9);
            this.grpHSV.Controls.Add(this.label6);
            this.grpHSV.Controls.Add(this.label4);
            this.grpHSV.Controls.Add(this.vTrackBarLower);
            this.grpHSV.Controls.Add(this.lblValue);
            this.grpHSV.Controls.Add(this.lblSaturation);
            this.grpHSV.Controls.Add(this.lblHue);
            this.grpHSV.Controls.Add(this.sTrackBarUpper);
            this.grpHSV.Controls.Add(this.hTrackBarLower);
            this.grpHSV.Controls.Add(this.sTrackBarLower);
            this.grpHSV.Location = new System.Drawing.Point(20, 19);
            this.grpHSV.Name = "grpHSV";
            this.grpHSV.Size = new System.Drawing.Size(483, 564);
            this.grpHSV.TabIndex = 0;
            this.grpHSV.TabStop = false;
            this.grpHSV.Text = "HSV";
            // 
            // panelColor
            // 
            this.panelColor.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.panelColor.Location = new System.Drawing.Point(226, 488);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(68, 47);
            this.panelColor.TabIndex = 39;
            this.panelColor.Click += new System.EventHandler(this.panelColor_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(191, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 18);
            this.label8.TabIndex = 38;
            this.label8.Text = "128.00";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(284, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 18);
            this.label11.TabIndex = 37;
            this.label11.Text = "0.00";
            // 
            // hTrackBarUpper
            // 
            this.hTrackBarUpper.Location = new System.Drawing.Point(284, 13);
            this.hTrackBarUpper.Maximum = 128;
            this.hTrackBarUpper.Name = "hTrackBarUpper";
            this.hTrackBarUpper.Size = new System.Drawing.Size(189, 69);
            this.hTrackBarUpper.TabIndex = 36;
            this.hTrackBarUpper.Value = 128;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(400, 218);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 18);
            this.label3.TabIndex = 35;
            this.label3.Text = "255.00";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(310, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 18);
            this.label5.TabIndex = 34;
            this.label5.Text = "0.00";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(405, 341);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 18);
            this.label1.TabIndex = 32;
            this.label1.Text = "255.00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(306, 341);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 18);
            this.label2.TabIndex = 31;
            this.label2.Text = "0.00";
            // 
            // vTrackBarUpper
            // 
            this.vTrackBarUpper.Location = new System.Drawing.Point(289, 278);
            this.vTrackBarUpper.Maximum = 255;
            this.vTrackBarUpper.Name = "vTrackBarUpper";
            this.vTrackBarUpper.Size = new System.Drawing.Size(184, 69);
            this.vTrackBarUpper.TabIndex = 30;
            this.vTrackBarUpper.Value = 255;
            // 
            // chkShowColorBinaryOnly
            // 
            this.chkShowColorBinaryOnly.AutoSize = true;
            this.chkShowColorBinaryOnly.Location = new System.Drawing.Point(141, 406);
            this.chkShowColorBinaryOnly.Name = "chkShowColorBinaryOnly";
            this.chkShowColorBinaryOnly.Size = new System.Drawing.Size(124, 22);
            this.chkShowColorBinaryOnly.TabIndex = 29;
            this.chkShowColorBinaryOnly.Text = "컬러이진화";
            this.chkShowColorBinaryOnly.UseVisualStyleBackColor = true;
            this.chkShowColorBinaryOnly.CheckedChanged += new System.EventHandler(this.chkShowColorBinaryOnly_CheckedChanged);
            // 
            // chkInvert
            // 
            this.chkInvert.AutoSize = true;
            this.chkInvert.Location = new System.Drawing.Point(20, 406);
            this.chkInvert.Name = "chkInvert";
            this.chkInvert.Size = new System.Drawing.Size(70, 22);
            this.chkInvert.TabIndex = 28;
            this.chkInvert.Text = "반전";
            this.chkInvert.UseVisualStyleBackColor = true;
            this.chkInvert.CheckedChanged += new System.EventHandler(this.chkInvert_CheckedChanged);
            // 
            // btnApplyHSV
            // 
            this.btnApplyHSV.Location = new System.Drawing.Point(329, 434);
            this.btnApplyHSV.Name = "btnApplyHSV";
            this.btnApplyHSV.Size = new System.Drawing.Size(134, 94);
            this.btnApplyHSV.TabIndex = 27;
            this.btnApplyHSV.Text = "HSV적용";
            this.btnApplyHSV.UseVisualStyleBackColor = true;
            this.btnApplyHSV.Click += new System.EventHandler(this.btnApplyHSV_Click);
            // 
            // txtV
            // 
            this.txtV.Location = new System.Drawing.Point(5, 333);
            this.txtV.Name = "txtV";
            this.txtV.Size = new System.Drawing.Size(86, 28);
            this.txtV.TabIndex = 26;
            // 
            // txtS
            // 
            this.txtS.Location = new System.Drawing.Point(14, 209);
            this.txtS.Name = "txtS";
            this.txtS.Size = new System.Drawing.Size(86, 28);
            this.txtS.TabIndex = 25;
            // 
            // txtH
            // 
            this.txtH.Location = new System.Drawing.Point(1, 85);
            this.txtH.Name = "txtH";
            this.txtH.Size = new System.Drawing.Size(88, 28);
            this.txtH.TabIndex = 24;
            // 
            // panelColorPreview
            // 
            this.panelColorPreview.Location = new System.Drawing.Point(223, 663);
            this.panelColorPreview.Name = "panelColorPreview";
            this.panelColorPreview.Size = new System.Drawing.Size(72, 40);
            this.panelColorPreview.TabIndex = 23;
            // 
            // btnTeachinColor
            // 
            this.btnTeachinColor.Location = new System.Drawing.Point(20, 488);
            this.btnTeachinColor.Name = "btnTeachinColor";
            this.btnTeachinColor.Size = new System.Drawing.Size(187, 40);
            this.btnTeachinColor.TabIndex = 22;
            this.btnTeachinColor.Text = "TeachinColor";
            this.btnTeachinColor.UseVisualStyleBackColor = true;
            this.btnTeachinColor.Click += new System.EventHandler(this.btnTeachinColor_Click);
            // 
            // chkHighlight
            // 
            this.chkHighlight.AutoSize = true;
            this.chkHighlight.Location = new System.Drawing.Point(20, 434);
            this.chkHighlight.Name = "chkHighlight";
            this.chkHighlight.Size = new System.Drawing.Size(246, 22);
            this.chkHighlight.TabIndex = 21;
            this.chkHighlight.Text = "Highlight in Camera viewer";
            this.chkHighlight.UseVisualStyleBackColor = true;
            this.chkHighlight.CheckedChanged += new System.EventHandler(this.chkHighlight_CheckedChanged_1);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(206, 341);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 18);
            this.label10.TabIndex = 18;
            this.label10.Text = "255.00";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(99, 341);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 18);
            this.label12.TabIndex = 16;
            this.label12.Text = "0.00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(229, 218);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 18);
            this.label7.TabIndex = 13;
            this.label7.Text = "255.00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(125, 218);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 18);
            this.label9.TabIndex = 11;
            this.label9.Text = "0.00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(400, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 18);
            this.label6.TabIndex = 8;
            this.label6.Text = "128.00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(102, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "0.00";
            // 
            // vTrackBarLower
            // 
            this.vTrackBarLower.Location = new System.Drawing.Point(91, 278);
            this.vTrackBarLower.Maximum = 255;
            this.vTrackBarLower.Name = "vTrackBarLower";
            this.vTrackBarLower.Size = new System.Drawing.Size(169, 69);
            this.vTrackBarLower.TabIndex = 5;
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(17, 278);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(52, 18);
            this.lblValue.TabIndex = 4;
            this.lblValue.Text = "Value";
            // 
            // lblSaturation
            // 
            this.lblSaturation.AutoSize = true;
            this.lblSaturation.Location = new System.Drawing.Point(15, 146);
            this.lblSaturation.Name = "lblSaturation";
            this.lblSaturation.Size = new System.Drawing.Size(89, 18);
            this.lblSaturation.TabIndex = 3;
            this.lblSaturation.Text = "Saturation";
            // 
            // lblHue
            // 
            this.lblHue.AutoSize = true;
            this.lblHue.Location = new System.Drawing.Point(30, 36);
            this.lblHue.Name = "lblHue";
            this.lblHue.Size = new System.Drawing.Size(39, 18);
            this.lblHue.TabIndex = 2;
            this.lblHue.Text = "Hue";
            // 
            // sTrackBarUpper
            // 
            this.sTrackBarUpper.Location = new System.Drawing.Point(293, 146);
            this.sTrackBarUpper.Maximum = 255;
            this.sTrackBarUpper.Name = "sTrackBarUpper";
            this.sTrackBarUpper.Size = new System.Drawing.Size(184, 69);
            this.sTrackBarUpper.TabIndex = 1;
            this.sTrackBarUpper.Value = 255;
            // 
            // hTrackBarLower
            // 
            this.hTrackBarLower.Location = new System.Drawing.Point(106, 13);
            this.hTrackBarLower.Maximum = 128;
            this.hTrackBarLower.Name = "hTrackBarLower";
            this.hTrackBarLower.Size = new System.Drawing.Size(159, 69);
            this.hTrackBarLower.TabIndex = 0;
            // 
            // sTrackBarLower
            // 
            this.sTrackBarLower.Location = new System.Drawing.Point(128, 146);
            this.sTrackBarLower.Maximum = 255;
            this.sTrackBarLower.Name = "sTrackBarLower";
            this.sTrackBarLower.Size = new System.Drawing.Size(159, 69);
            this.sTrackBarLower.TabIndex = 33;
            // 
            // grpFilter
            // 
            this.grpFilter.Controls.Add(this.btnApply);
            this.grpFilter.Controls.Add(this.select_effect2);
            this.grpFilter.Controls.Add(this.select_effect);
            this.grpFilter.Location = new System.Drawing.Point(25, 589);
            this.grpFilter.Name = "grpFilter";
            this.grpFilter.Size = new System.Drawing.Size(476, 181);
            this.grpFilter.TabIndex = 1;
            this.grpFilter.TabStop = false;
            this.grpFilter.Text = "필터";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(287, 62);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(121, 82);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "적용";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApplyFilter_Click);
            // 
            // select_effect2
            // 
            this.select_effect2.FormattingEnabled = true;
            this.select_effect2.Items.AddRange(new object[] {
            "NOT 연산"});
            this.select_effect2.Location = new System.Drawing.Point(103, 118);
            this.select_effect2.Name = "select_effect2";
            this.select_effect2.Size = new System.Drawing.Size(121, 26);
            this.select_effect2.TabIndex = 1;
            this.select_effect2.Text = "선택2";
            this.select_effect2.SelectedIndexChanged += new System.EventHandler(this.select_effect2_SelectedIndexChanged_1);
            // 
            // select_effect
            // 
            this.select_effect.AutoCompleteCustomSource.AddRange(new string[] {
            "비트연산"});
            this.select_effect.FormattingEnabled = true;
            this.select_effect.Items.AddRange(new object[] {
            "Bitwise(비트연산)"});
            this.select_effect.Location = new System.Drawing.Point(103, 60);
            this.select_effect.Name = "select_effect";
            this.select_effect.Size = new System.Drawing.Size(121, 26);
            this.select_effect.TabIndex = 0;
            this.select_effect.Text = "선택1";
            this.select_effect.SelectedIndexChanged += new System.EventHandler(this.select_effect_SelectedIndexChanged);
            // 
            // ColorBinaryInspProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpFilter);
            this.Controls.Add(this.grpHSV);
            this.Name = "ColorBinaryInspProp";
            this.Size = new System.Drawing.Size(539, 969);
            this.grpHSV.ResumeLayout(false);
            this.grpHSV.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hTrackBarUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vTrackBarUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vTrackBarLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTrackBarUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hTrackBarLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTrackBarLower)).EndInit();
            this.grpFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ImageViewCCtrl imageViewer;
        private System.Windows.Forms.GroupBox grpHSV;
        private System.Windows.Forms.TrackBar hTrackBarLower;
        private System.Windows.Forms.Label lblHue;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblSaturation;
        private System.Windows.Forms.TrackBar vTrackBarLower;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkHighlight;
        private System.Windows.Forms.Button btnTeachinColor;
        private System.Windows.Forms.Panel panelColorPreview;
        private System.Windows.Forms.TrackBar sTrackBarUpper;
        private System.Windows.Forms.TextBox txtV;
        private System.Windows.Forms.TextBox txtS;
        private System.Windows.Forms.TextBox txtH;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.ComboBox select_effect;
        private System.Windows.Forms.ComboBox select_effect2;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnApplyHSV;
        private System.Windows.Forms.CheckBox chkInvert;
        private System.Windows.Forms.CheckBox chkShowColorBinaryOnly;
        private System.Windows.Forms.TrackBar vTrackBarUpper;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar sTrackBarLower;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar hTrackBarUpper;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Panel panelColor;
    }
}

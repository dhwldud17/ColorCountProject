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
            this.vTrackBar = new System.Windows.Forms.TrackBar();
            this.lblValue = new System.Windows.Forms.Label();
            this.lblSaturation = new System.Windows.Forms.Label();
            this.lblHue = new System.Windows.Forms.Label();
            this.sTrackBar = new System.Windows.Forms.TrackBar();
            this.hTrackBar = new System.Windows.Forms.TrackBar();
            this.grpFilter = new System.Windows.Forms.GroupBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.select_effect2 = new System.Windows.Forms.ComboBox();
            this.select_effect = new System.Windows.Forms.ComboBox();
            this.btnApplyHSV = new System.Windows.Forms.Button();
            this.chkInvert = new System.Windows.Forms.CheckBox();
            this.grpHSV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hTrackBar)).BeginInit();
            this.grpFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpHSV
            // 
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
            this.grpHSV.Controls.Add(this.vTrackBar);
            this.grpHSV.Controls.Add(this.lblValue);
            this.grpHSV.Controls.Add(this.lblSaturation);
            this.grpHSV.Controls.Add(this.lblHue);
            this.grpHSV.Controls.Add(this.sTrackBar);
            this.grpHSV.Controls.Add(this.hTrackBar);
            this.grpHSV.Location = new System.Drawing.Point(20, 19);
            this.grpHSV.Name = "grpHSV";
            this.grpHSV.Size = new System.Drawing.Size(483, 532);
            this.grpHSV.TabIndex = 0;
            this.grpHSV.TabStop = false;
            this.grpHSV.Text = "HSV";
            // 
            // txtV
            // 
            this.txtV.Location = new System.Drawing.Point(9, 297);
            this.txtV.Name = "txtV";
            this.txtV.Size = new System.Drawing.Size(86, 28);
            this.txtV.TabIndex = 26;
            // 
            // txtS
            // 
            this.txtS.Location = new System.Drawing.Point(9, 191);
            this.txtS.Name = "txtS";
            this.txtS.Size = new System.Drawing.Size(86, 28);
            this.txtS.TabIndex = 25;
            // 
            // txtH
            // 
            this.txtH.Location = new System.Drawing.Point(7, 93);
            this.txtH.Name = "txtH";
            this.txtH.Size = new System.Drawing.Size(88, 28);
            this.txtH.TabIndex = 24;
            // 
            // panelColorPreview
            // 
            this.panelColorPreview.Location = new System.Drawing.Point(224, 436);
            this.panelColorPreview.Name = "panelColorPreview";
            this.panelColorPreview.Size = new System.Drawing.Size(72, 40);
            this.panelColorPreview.TabIndex = 23;
            // 
            // btnTeachinColor
            // 
            this.btnTeachinColor.Location = new System.Drawing.Point(20, 436);
            this.btnTeachinColor.Name = "btnTeachinColor";
            this.btnTeachinColor.Size = new System.Drawing.Size(187, 40);
            this.btnTeachinColor.TabIndex = 22;
            this.btnTeachinColor.Text = "TeachinColor";
            this.btnTeachinColor.UseVisualStyleBackColor = true;
            // 
            // chkHighlight
            // 
            this.chkHighlight.AutoSize = true;
            this.chkHighlight.Location = new System.Drawing.Point(20, 382);
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
            this.label10.Location = new System.Drawing.Point(400, 316);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 18);
            this.label10.TabIndex = 18;
            this.label10.Text = "360.00";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(107, 319);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 18);
            this.label12.TabIndex = 16;
            this.label12.Text = "0.00";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(400, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 18);
            this.label7.TabIndex = 13;
            this.label7.Text = "360.00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(107, 192);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 18);
            this.label9.TabIndex = 11;
            this.label9.Text = "0.00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(399, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 18);
            this.label6.TabIndex = 8;
            this.label6.Text = "360.00";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(107, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "0.00";
            // 
            // vTrackBar
            // 
            this.vTrackBar.Location = new System.Drawing.Point(110, 268);
            this.vTrackBar.Name = "vTrackBar";
            this.vTrackBar.Size = new System.Drawing.Size(367, 69);
            this.vTrackBar.TabIndex = 5;
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(17, 253);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(52, 18);
            this.lblValue.TabIndex = 4;
            this.lblValue.Text = "Value";
            // 
            // lblSaturation
            // 
            this.lblSaturation.AutoSize = true;
            this.lblSaturation.Location = new System.Drawing.Point(6, 145);
            this.lblSaturation.Name = "lblSaturation";
            this.lblSaturation.Size = new System.Drawing.Size(89, 18);
            this.lblSaturation.TabIndex = 3;
            this.lblSaturation.Text = "Saturation";
            // 
            // lblHue
            // 
            this.lblHue.AutoSize = true;
            this.lblHue.Location = new System.Drawing.Point(30, 54);
            this.lblHue.Name = "lblHue";
            this.lblHue.Size = new System.Drawing.Size(39, 18);
            this.lblHue.TabIndex = 2;
            this.lblHue.Text = "Hue";
            // 
            // sTrackBar
            // 
            this.sTrackBar.Location = new System.Drawing.Point(101, 151);
            this.sTrackBar.Name = "sTrackBar";
            this.sTrackBar.Size = new System.Drawing.Size(367, 69);
            this.sTrackBar.TabIndex = 1;
            // 
            // hTrackBar
            // 
            this.hTrackBar.Location = new System.Drawing.Point(96, 24);
            this.hTrackBar.Name = "hTrackBar";
            this.hTrackBar.Size = new System.Drawing.Size(367, 69);
            this.hTrackBar.TabIndex = 0;
            // 
            // grpFilter
            // 
            this.grpFilter.Controls.Add(this.btnApply);
            this.grpFilter.Controls.Add(this.select_effect2);
            this.grpFilter.Controls.Add(this.select_effect);
            this.grpFilter.Location = new System.Drawing.Point(27, 572);
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
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
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
            // 
            // select_effect
            // 
            this.select_effect.AutoCompleteCustomSource.AddRange(new string[] {
            "비트연산"});
            this.select_effect.FormattingEnabled = true;
            this.select_effect.Location = new System.Drawing.Point(103, 60);
            this.select_effect.Name = "select_effect";
            this.select_effect.Size = new System.Drawing.Size(121, 26);
            this.select_effect.TabIndex = 0;
            this.select_effect.Text = "선택1";
            this.select_effect.SelectedIndexChanged += new System.EventHandler(this.select_effect_SelectedIndexChanged);
            // 
            // btnApplyHSV
            // 
            this.btnApplyHSV.Location = new System.Drawing.Point(329, 382);
            this.btnApplyHSV.Name = "btnApplyHSV";
            this.btnApplyHSV.Size = new System.Drawing.Size(134, 94);
            this.btnApplyHSV.TabIndex = 27;
            this.btnApplyHSV.Text = "HSV적용";
            this.btnApplyHSV.UseVisualStyleBackColor = true;
            this.btnApplyHSV.Click += new System.EventHandler(this.btnApplyHSV_Click);
            // 
            // chkInvert
            // 
            this.chkInvert.AutoSize = true;
            this.chkInvert.Location = new System.Drawing.Point(20, 354);
            this.chkInvert.Name = "chkInvert";
            this.chkInvert.Size = new System.Drawing.Size(70, 22);
            this.chkInvert.TabIndex = 28;
            this.chkInvert.Text = "반전";
            this.chkInvert.UseVisualStyleBackColor = true;
            this.chkInvert.CheckedChanged += new System.EventHandler(this.chkInvert_CheckedChanged);
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
            ((System.ComponentModel.ISupportInitialize)(this.vTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hTrackBar)).EndInit();
            this.grpFilter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpHSV;
        private System.Windows.Forms.TrackBar hTrackBar;
        private System.Windows.Forms.Label lblHue;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblSaturation;
        private System.Windows.Forms.TrackBar vTrackBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkHighlight;
        private System.Windows.Forms.Button btnTeachinColor;
        private System.Windows.Forms.Panel panelColorPreview;
        private System.Windows.Forms.TrackBar sTrackBar;
        private System.Windows.Forms.TextBox txtV;
        private System.Windows.Forms.TextBox txtS;
        private System.Windows.Forms.TextBox txtH;
        private System.Windows.Forms.GroupBox grpFilter;
        private System.Windows.Forms.ComboBox select_effect;
        private System.Windows.Forms.ComboBox select_effect2;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnApplyHSV;
        private System.Windows.Forms.CheckBox chkInvert;
    }
}

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
            this.grpHSV.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // grpHSV
            // 
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
            this.btnTeachinColor.Click += new System.EventHandler(this.btnTeachinColor_Click);
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
            this.hTrackBar.Location = new System.Drawing.Point(96, 42);
            this.hTrackBar.Name = "hTrackBar";
            this.hTrackBar.Size = new System.Drawing.Size(367, 69);
            this.hTrackBar.TabIndex = 0;
            // 
            // ColorBinaryInspProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpHSV);
            this.Name = "ColorBinaryInspProp";
            this.Size = new System.Drawing.Size(518, 570);
            this.grpHSV.ResumeLayout(false);
            this.grpHSV.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hTrackBar)).EndInit();
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
    }
}

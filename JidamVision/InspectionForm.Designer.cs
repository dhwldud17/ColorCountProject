namespace JidamVision
{
    partial class InspectionForm
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
            this.bntStart = new System.Windows.Forms.Button();
            this.bntStop = new System.Windows.Forms.Button();
            this.rtbTotalnumber = new System.Windows.Forms.RichTextBox();
            this.rtbGood = new System.Windows.Forms.RichTextBox();
            this.rtbFaulty = new System.Windows.Forms.RichTextBox();
            this.rtbPercent = new System.Windows.Forms.RichTextBox();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dgvMetric = new System.Windows.Forms.DataGridView();
            this.dtpCurrenttime = new System.Windows.Forms.DateTimePicker();
            this.lbStartTime = new System.Windows.Forms.Label();
            this.lbCurrenttime = new System.Windows.Forms.Label();
            this.lbGood = new System.Windows.Forms.Label();
            this.lbFaulty = new System.Windows.Forms.Label();
            this.lbPercent = new System.Windows.Forms.Label();
            this.lbTotalnumber = new System.Windows.Forms.Label();
            this.imageViewCCtrl1 = new JidamVision.ImageViewCCtrl();
            this.imageViewer = new JidamVision.ImageViewCCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetric)).BeginInit();
            this.SuspendLayout();
            // 
            // bntStart
            // 
            this.bntStart.Location = new System.Drawing.Point(559, 112);
            this.bntStart.Name = "bntStart";
            this.bntStart.Size = new System.Drawing.Size(184, 71);
            this.bntStart.TabIndex = 1;
            this.bntStart.Text = "시작";
            this.bntStart.UseVisualStyleBackColor = true;
            // 
            // bntStop
            // 
            this.bntStop.Location = new System.Drawing.Point(559, 226);
            this.bntStop.Name = "bntStop";
            this.bntStop.Size = new System.Drawing.Size(184, 71);
            this.bntStop.TabIndex = 2;
            this.bntStop.Text = "정지";
            this.bntStop.UseVisualStyleBackColor = true;
            // 
            // rtbTotalnumber
            // 
            this.rtbTotalnumber.Location = new System.Drawing.Point(559, 338);
            this.rtbTotalnumber.Name = "rtbTotalnumber";
            this.rtbTotalnumber.Size = new System.Drawing.Size(184, 152);
            this.rtbTotalnumber.TabIndex = 3;
            this.rtbTotalnumber.Text = "";
            // 
            // rtbGood
            // 
            this.rtbGood.Location = new System.Drawing.Point(559, 532);
            this.rtbGood.Name = "rtbGood";
            this.rtbGood.Size = new System.Drawing.Size(85, 78);
            this.rtbGood.TabIndex = 4;
            this.rtbGood.Text = "";
            // 
            // rtbFaulty
            // 
            this.rtbFaulty.Location = new System.Drawing.Point(659, 532);
            this.rtbFaulty.Name = "rtbFaulty";
            this.rtbFaulty.Size = new System.Drawing.Size(84, 78);
            this.rtbFaulty.TabIndex = 5;
            this.rtbFaulty.Text = "";
            // 
            // rtbPercent
            // 
            this.rtbPercent.Location = new System.Drawing.Point(762, 532);
            this.rtbPercent.Name = "rtbPercent";
            this.rtbPercent.Size = new System.Drawing.Size(183, 78);
            this.rtbPercent.TabIndex = 6;
            this.rtbPercent.Text = "";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(27, 66);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(336, 28);
            this.dtpStartTime.TabIndex = 7;
            // 
            // dgvMetric
            // 
            this.dgvMetric.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMetric.Location = new System.Drawing.Point(762, 66);
            this.dgvMetric.Name = "dgvMetric";
            this.dgvMetric.RowHeadersWidth = 62;
            this.dgvMetric.RowTemplate.Height = 30;
            this.dgvMetric.Size = new System.Drawing.Size(183, 424);
            this.dgvMetric.TabIndex = 8;
            // 
            // dtpCurrenttime
            // 
            this.dtpCurrenttime.Location = new System.Drawing.Point(407, 66);
            this.dtpCurrenttime.Name = "dtpCurrenttime";
            this.dtpCurrenttime.Size = new System.Drawing.Size(336, 28);
            this.dtpCurrenttime.TabIndex = 9;
            // 
            // lbStartTime
            // 
            this.lbStartTime.AutoSize = true;
            this.lbStartTime.Location = new System.Drawing.Point(24, 45);
            this.lbStartTime.Name = "lbStartTime";
            this.lbStartTime.Size = new System.Drawing.Size(80, 18);
            this.lbStartTime.TabIndex = 10;
            this.lbStartTime.Text = "시작시간";
            // 
            // lbCurrenttime
            // 
            this.lbCurrenttime.AutoSize = true;
            this.lbCurrenttime.Location = new System.Drawing.Point(404, 45);
            this.lbCurrenttime.Name = "lbCurrenttime";
            this.lbCurrenttime.Size = new System.Drawing.Size(80, 18);
            this.lbCurrenttime.TabIndex = 11;
            this.lbCurrenttime.Text = "현재시간";
            // 
            // lbGood
            // 
            this.lbGood.AutoSize = true;
            this.lbGood.Location = new System.Drawing.Point(559, 511);
            this.lbGood.Name = "lbGood";
            this.lbGood.Size = new System.Drawing.Size(44, 18);
            this.lbGood.TabIndex = 12;
            this.lbGood.Text = "양품";
            // 
            // lbFaulty
            // 
            this.lbFaulty.AutoSize = true;
            this.lbFaulty.Location = new System.Drawing.Point(656, 511);
            this.lbFaulty.Name = "lbFaulty";
            this.lbFaulty.Size = new System.Drawing.Size(44, 18);
            this.lbFaulty.TabIndex = 13;
            this.lbFaulty.Text = "불량";
            // 
            // lbPercent
            // 
            this.lbPercent.AutoSize = true;
            this.lbPercent.Location = new System.Drawing.Point(759, 511);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(62, 18);
            this.lbPercent.TabIndex = 14;
            this.lbPercent.Text = "퍼센트";
            // 
            // lbTotalnumber
            // 
            this.lbTotalnumber.AutoSize = true;
            this.lbTotalnumber.Location = new System.Drawing.Point(559, 317);
            this.lbTotalnumber.Name = "lbTotalnumber";
            this.lbTotalnumber.Size = new System.Drawing.Size(62, 18);
            this.lbTotalnumber.TabIndex = 15;
            this.lbTotalnumber.Text = "총개수";
            // 
            // imageViewCCtrl1
            // 
            this.imageViewCCtrl1.AutoSize = true;
            this.imageViewCCtrl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.imageViewCCtrl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageViewCCtrl1.Location = new System.Drawing.Point(421, 479);
            this.imageViewCCtrl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imageViewCCtrl1.Name = "imageViewCCtrl1";
            this.imageViewCCtrl1.Size = new System.Drawing.Size(4, 4);
            this.imageViewCCtrl1.TabIndex = 16;
            // 
            // imageViewer
            // 
            this.imageViewer.AutoSize = true;
            this.imageViewer.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageViewer.Location = new System.Drawing.Point(24, 112);
            this.imageViewer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(528, 490);
            this.imageViewer.TabIndex = 17;
            // 
            // InspectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 698);
            this.Controls.Add(this.imageViewer);
            this.Controls.Add(this.imageViewCCtrl1);
            this.Controls.Add(this.lbTotalnumber);
            this.Controls.Add(this.lbPercent);
            this.Controls.Add(this.lbFaulty);
            this.Controls.Add(this.lbGood);
            this.Controls.Add(this.lbCurrenttime);
            this.Controls.Add(this.lbStartTime);
            this.Controls.Add(this.dtpCurrenttime);
            this.Controls.Add(this.dgvMetric);
            this.Controls.Add(this.dtpStartTime);
            this.Controls.Add(this.rtbPercent);
            this.Controls.Add(this.rtbFaulty);
            this.Controls.Add(this.rtbGood);
            this.Controls.Add(this.rtbTotalnumber);
            this.Controls.Add(this.bntStop);
            this.Controls.Add(this.bntStart);
            this.Name = "InspectionForm";
            this.Text = "InspectionForm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bntStart;
        private System.Windows.Forms.Button bntStop;
        private System.Windows.Forms.RichTextBox rtbTotalnumber;
        private System.Windows.Forms.RichTextBox rtbGood;
        private System.Windows.Forms.RichTextBox rtbFaulty;
        private System.Windows.Forms.RichTextBox rtbPercent;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.DataGridView dgvMetric;
        private System.Windows.Forms.DateTimePicker dtpCurrenttime;
        private System.Windows.Forms.Label lbStartTime;
        private System.Windows.Forms.Label lbCurrenttime;
        private System.Windows.Forms.Label lbGood;
        private System.Windows.Forms.Label lbFaulty;
        private System.Windows.Forms.Label lbPercent;
        private System.Windows.Forms.Label lbTotalnumber;
        private ImageViewCCtrl imageViewCCtrl1;
        private ImageViewCCtrl imageViewer;
    }
}
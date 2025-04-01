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
            this.imageViewer = new JidamVision.ImageViewCCtrl();
            this.lblResult = new System.Windows.Forms.Label();
            this.imageViewCCtrl1 = new JidamVision.ImageViewCCtrl();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetric)).BeginInit();
            this.SuspendLayout();
            // 
            // bntStart
            // 
            this.bntStart.Location = new System.Drawing.Point(437, 61);
            this.bntStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bntStart.Name = "bntStart";
            this.bntStart.Size = new System.Drawing.Size(147, 42);
            this.bntStart.TabIndex = 1;
            this.bntStart.Text = "시작";
            this.bntStart.UseVisualStyleBackColor = true;
            this.bntStart.Click += new System.EventHandler(this.bntStart_Click);
            // 
            // bntStop
            // 
            this.bntStop.Location = new System.Drawing.Point(436, 108);
            this.bntStop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bntStop.Name = "bntStop";
            this.bntStop.Size = new System.Drawing.Size(147, 48);
            this.bntStop.TabIndex = 2;
            this.bntStop.Text = "정지";
            this.bntStop.UseVisualStyleBackColor = true;
            this.bntStop.Click += new System.EventHandler(this.bntStop_Click);
            // 
            // rtbTotalnumber
            // 
            this.rtbTotalnumber.Location = new System.Drawing.Point(437, 200);
            this.rtbTotalnumber.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbTotalnumber.Name = "rtbTotalnumber";
            this.rtbTotalnumber.Size = new System.Drawing.Size(148, 127);
            this.rtbTotalnumber.TabIndex = 3;
            this.rtbTotalnumber.Text = "";
            // 
            // rtbGood
            // 
            this.rtbGood.Location = new System.Drawing.Point(436, 346);
            this.rtbGood.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbGood.Name = "rtbGood";
            this.rtbGood.Size = new System.Drawing.Size(69, 66);
            this.rtbGood.TabIndex = 4;
            this.rtbGood.Text = "";
            // 
            // rtbFaulty
            // 
            this.rtbFaulty.Location = new System.Drawing.Point(509, 346);
            this.rtbFaulty.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbFaulty.Name = "rtbFaulty";
            this.rtbFaulty.Size = new System.Drawing.Size(68, 66);
            this.rtbFaulty.TabIndex = 5;
            this.rtbFaulty.Text = "";
            // 
            // rtbPercent
            // 
            this.rtbPercent.Location = new System.Drawing.Point(436, 431);
            this.rtbPercent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbPercent.Name = "rtbPercent";
            this.rtbPercent.Size = new System.Drawing.Size(147, 66);
            this.rtbPercent.TabIndex = 6;
            this.rtbPercent.Text = "";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(12, 23);
            this.dtpStartTime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(269, 25);
            this.dtpStartTime.TabIndex = 7;
            // 
            // dgvMetric
            // 
            this.dgvMetric.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMetric.Location = new System.Drawing.Point(597, 23);
            this.dgvMetric.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvMetric.Name = "dgvMetric";
            this.dgvMetric.RowHeadersWidth = 62;
            this.dgvMetric.RowTemplate.Height = 30;
            this.dgvMetric.Size = new System.Drawing.Size(146, 473);
            this.dgvMetric.TabIndex = 8;
            // 
            // dtpCurrenttime
            // 
            this.dtpCurrenttime.Location = new System.Drawing.Point(316, 23);
            this.dtpCurrenttime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpCurrenttime.Name = "dtpCurrenttime";
            this.dtpCurrenttime.Size = new System.Drawing.Size(269, 25);
            this.dtpCurrenttime.TabIndex = 9;
            // 
            // lbStartTime
            // 
            this.lbStartTime.AutoSize = true;
            this.lbStartTime.Location = new System.Drawing.Point(9, 6);
            this.lbStartTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbStartTime.Name = "lbStartTime";
            this.lbStartTime.Size = new System.Drawing.Size(67, 15);
            this.lbStartTime.TabIndex = 10;
            this.lbStartTime.Text = "시작시간";
            // 
            // lbCurrenttime
            // 
            this.lbCurrenttime.AutoSize = true;
            this.lbCurrenttime.Location = new System.Drawing.Point(313, 6);
            this.lbCurrenttime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbCurrenttime.Name = "lbCurrenttime";
            this.lbCurrenttime.Size = new System.Drawing.Size(67, 15);
            this.lbCurrenttime.TabIndex = 11;
            this.lbCurrenttime.Text = "현재시간";
            // 
            // lbGood
            // 
            this.lbGood.AutoSize = true;
            this.lbGood.Location = new System.Drawing.Point(437, 329);
            this.lbGood.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbGood.Name = "lbGood";
            this.lbGood.Size = new System.Drawing.Size(37, 15);
            this.lbGood.TabIndex = 12;
            this.lbGood.Text = "양품";
            // 
            // lbFaulty
            // 
            this.lbFaulty.AutoSize = true;
            this.lbFaulty.Location = new System.Drawing.Point(509, 329);
            this.lbFaulty.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbFaulty.Name = "lbFaulty";
            this.lbFaulty.Size = new System.Drawing.Size(37, 15);
            this.lbFaulty.TabIndex = 13;
            this.lbFaulty.Text = "불량";
            // 
            // lbPercent
            // 
            this.lbPercent.AutoSize = true;
            this.lbPercent.Location = new System.Drawing.Point(437, 414);
            this.lbPercent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(52, 15);
            this.lbPercent.TabIndex = 14;
            this.lbPercent.Text = "퍼센트";
            // 
            // lbTotalnumber
            // 
            this.lbTotalnumber.AutoSize = true;
            this.lbTotalnumber.Location = new System.Drawing.Point(437, 182);
            this.lbTotalnumber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTotalnumber.Name = "lbTotalnumber";
            this.lbTotalnumber.Size = new System.Drawing.Size(52, 15);
            this.lbTotalnumber.TabIndex = 15;
            this.lbTotalnumber.Text = "총개수";
            // 
            // imageViewer
            // 
            this.imageViewer.AutoSize = true;
            this.imageViewer.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageViewer.Location = new System.Drawing.Point(12, 89);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.Size = new System.Drawing.Size(406, 408);
            this.imageViewer.TabIndex = 16;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(12, 61);
            this.lblResult.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(37, 15);
            this.lblResult.TabIndex = 18;
            this.lblResult.Text = "결과";
            // 
            // imageViewCCtrl1
            // 
            this.imageViewCCtrl1.AutoSize = true;
            this.imageViewCCtrl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.imageViewCCtrl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageViewCCtrl1.Location = new System.Drawing.Point(61, 255);
            this.imageViewCCtrl1.Name = "imageViewCCtrl1";
            this.imageViewCCtrl1.Size = new System.Drawing.Size(4, 4);
            this.imageViewCCtrl1.TabIndex = 19;
            // 
            // InspectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 509);
            this.Controls.Add(this.imageViewCCtrl1);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.imageViewer);
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
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "InspectionForm";
            this.Text = "InspectionForm";
            this.Resize += new System.EventHandler(this.InspectionForm_Resize);
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
        private ImageViewCCtrl imageViewer;
        private System.Windows.Forms.Label lblResult;
        private ImageViewCCtrl imageViewCCtrl1;
    }
}
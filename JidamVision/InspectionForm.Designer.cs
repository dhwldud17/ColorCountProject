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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // bntStart
            // 
            this.bntStart.Location = new System.Drawing.Point(391, 75);
            this.bntStart.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bntStart.Name = "bntStart";
            this.bntStart.Size = new System.Drawing.Size(129, 47);
            this.bntStart.TabIndex = 1;
            this.bntStart.Text = "시작";
            this.bntStart.UseVisualStyleBackColor = true;
            this.bntStart.Click += new System.EventHandler(this.bntStart_Click);
            // 
            // bntStop
            // 
            this.bntStop.Location = new System.Drawing.Point(391, 151);
            this.bntStop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.bntStop.Name = "bntStop";
            this.bntStop.Size = new System.Drawing.Size(129, 47);
            this.bntStop.TabIndex = 2;
            this.bntStop.Text = "정지";
            this.bntStop.UseVisualStyleBackColor = true;
            this.bntStop.Click += new System.EventHandler(this.bntStop_Click);
            // 
            // rtbTotalnumber
            // 
            this.rtbTotalnumber.Location = new System.Drawing.Point(391, 225);
            this.rtbTotalnumber.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbTotalnumber.Name = "rtbTotalnumber";
            this.rtbTotalnumber.Size = new System.Drawing.Size(130, 103);
            this.rtbTotalnumber.TabIndex = 3;
            this.rtbTotalnumber.Text = "";
            // 
            // rtbGood
            // 
            this.rtbGood.Location = new System.Drawing.Point(391, 355);
            this.rtbGood.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbGood.Name = "rtbGood";
            this.rtbGood.Size = new System.Drawing.Size(61, 53);
            this.rtbGood.TabIndex = 4;
            this.rtbGood.Text = "";
            // 
            // rtbFaulty
            // 
            this.rtbFaulty.Location = new System.Drawing.Point(461, 355);
            this.rtbFaulty.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbFaulty.Name = "rtbFaulty";
            this.rtbFaulty.Size = new System.Drawing.Size(60, 53);
            this.rtbFaulty.TabIndex = 5;
            this.rtbFaulty.Text = "";
            // 
            // rtbPercent
            // 
            this.rtbPercent.Location = new System.Drawing.Point(533, 355);
            this.rtbPercent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rtbPercent.Name = "rtbPercent";
            this.rtbPercent.Size = new System.Drawing.Size(129, 53);
            this.rtbPercent.TabIndex = 6;
            this.rtbPercent.Text = "";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(19, 44);
            this.dtpStartTime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(236, 21);
            this.dtpStartTime.TabIndex = 7;
            // 
            // dgvMetric
            // 
            this.dgvMetric.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMetric.Location = new System.Drawing.Point(533, 44);
            this.dgvMetric.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvMetric.Name = "dgvMetric";
            this.dgvMetric.RowHeadersWidth = 62;
            this.dgvMetric.RowTemplate.Height = 30;
            this.dgvMetric.Size = new System.Drawing.Size(128, 283);
            this.dgvMetric.TabIndex = 8;
            // 
            // dtpCurrenttime
            // 
            this.dtpCurrenttime.Location = new System.Drawing.Point(285, 44);
            this.dtpCurrenttime.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dtpCurrenttime.Name = "dtpCurrenttime";
            this.dtpCurrenttime.Size = new System.Drawing.Size(236, 21);
            this.dtpCurrenttime.TabIndex = 9;
            // 
            // lbStartTime
            // 
            this.lbStartTime.AutoSize = true;
            this.lbStartTime.Location = new System.Drawing.Point(17, 30);
            this.lbStartTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbStartTime.Name = "lbStartTime";
            this.lbStartTime.Size = new System.Drawing.Size(53, 12);
            this.lbStartTime.TabIndex = 10;
            this.lbStartTime.Text = "시작시간";
            // 
            // lbCurrenttime
            // 
            this.lbCurrenttime.AutoSize = true;
            this.lbCurrenttime.Location = new System.Drawing.Point(283, 30);
            this.lbCurrenttime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbCurrenttime.Name = "lbCurrenttime";
            this.lbCurrenttime.Size = new System.Drawing.Size(53, 12);
            this.lbCurrenttime.TabIndex = 11;
            this.lbCurrenttime.Text = "현재시간";
            // 
            // lbGood
            // 
            this.lbGood.AutoSize = true;
            this.lbGood.Location = new System.Drawing.Point(391, 341);
            this.lbGood.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbGood.Name = "lbGood";
            this.lbGood.Size = new System.Drawing.Size(29, 12);
            this.lbGood.TabIndex = 12;
            this.lbGood.Text = "양품";
            // 
            // lbFaulty
            // 
            this.lbFaulty.AutoSize = true;
            this.lbFaulty.Location = new System.Drawing.Point(459, 341);
            this.lbFaulty.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbFaulty.Name = "lbFaulty";
            this.lbFaulty.Size = new System.Drawing.Size(29, 12);
            this.lbFaulty.TabIndex = 13;
            this.lbFaulty.Text = "불량";
            // 
            // lbPercent
            // 
            this.lbPercent.AutoSize = true;
            this.lbPercent.Location = new System.Drawing.Point(531, 341);
            this.lbPercent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbPercent.Name = "lbPercent";
            this.lbPercent.Size = new System.Drawing.Size(41, 12);
            this.lbPercent.TabIndex = 14;
            this.lbPercent.Text = "퍼센트";
            // 
            // lbTotalnumber
            // 
            this.lbTotalnumber.AutoSize = true;
            this.lbTotalnumber.Location = new System.Drawing.Point(391, 211);
            this.lbTotalnumber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbTotalnumber.Name = "lbTotalnumber";
            this.lbTotalnumber.Size = new System.Drawing.Size(41, 12);
            this.lbTotalnumber.TabIndex = 15;
            this.lbTotalnumber.Text = "총개수";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(19, 76);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(368, 332);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // InspectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 412);
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
            this.Controls.Add(this.pictureBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "InspectionForm";
            this.Text = "InspectionForm";
            this.Resize += new System.EventHandler(this.InspectionForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMetric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
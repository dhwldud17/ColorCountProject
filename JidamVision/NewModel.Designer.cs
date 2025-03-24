namespace JidamVision
{
    partial class NewModel
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
            this.lbModelName = new System.Windows.Forms.Label();
            this.lbModelInfo = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.TextBox();
            this.txtModelInfo = new System.Windows.Forms.RichTextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbModelName
            // 
            this.lbModelName.AutoSize = true;
            this.lbModelName.Location = new System.Drawing.Point(25, 14);
            this.lbModelName.Name = "lbModelName";
            this.lbModelName.Size = new System.Drawing.Size(41, 12);
            this.lbModelName.TabIndex = 0;
            this.lbModelName.Text = "모델명";
            // 
            // lbModelInfo
            // 
            this.lbModelInfo.AutoSize = true;
            this.lbModelInfo.Location = new System.Drawing.Point(27, 59);
            this.lbModelInfo.Name = "lbModelInfo";
            this.lbModelInfo.Size = new System.Drawing.Size(57, 12);
            this.lbModelInfo.TabIndex = 1;
            this.lbModelInfo.Text = "모델 정보";
            // 
            // txtModelName
            // 
            this.txtModelName.Location = new System.Drawing.Point(90, 11);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Size = new System.Drawing.Size(190, 21);
            this.txtModelName.TabIndex = 2;
            // 
            // txtModelInfo
            // 
            this.txtModelInfo.Location = new System.Drawing.Point(90, 56);
            this.txtModelInfo.Name = "txtModelInfo";
            this.txtModelInfo.Size = new System.Drawing.Size(190, 104);
            this.txtModelInfo.TabIndex = 3;
            this.txtModelInfo.Text = "";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(286, 185);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(79, 30);
            this.btnCreate.TabIndex = 4;
            this.btnCreate.Text = "만들기";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // NewModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 225);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtModelInfo);
            this.Controls.Add(this.txtModelName);
            this.Controls.Add(this.lbModelInfo);
            this.Controls.Add(this.lbModelName);
            this.Name = "NewModel";
            this.Text = "NewModel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbModelName;
        private System.Windows.Forms.Label lbModelInfo;
        private System.Windows.Forms.TextBox txtModelName;
        private System.Windows.Forms.RichTextBox txtModelInfo;
        private System.Windows.Forms.Button btnCreate;
    }
}
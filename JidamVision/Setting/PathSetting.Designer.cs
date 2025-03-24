namespace JidamVision.Setting
{
    partial class PathSetting
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbModelDir = new System.Windows.Forms.Label();
            this.txtModelDir = new System.Windows.Forms.TextBox();
            this.btnSelModelDir = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbModelDir
            // 
            this.lbModelDir.AutoSize = true;
            this.lbModelDir.Location = new System.Drawing.Point(14, 21);
            this.lbModelDir.Name = "lbModelDir";
            this.lbModelDir.Size = new System.Drawing.Size(57, 12);
            this.lbModelDir.TabIndex = 0;
            this.lbModelDir.Text = "모델 경로";
            // 
            // txtModelDir
            // 
            this.txtModelDir.Location = new System.Drawing.Point(85, 19);
            this.txtModelDir.Name = "txtModelDir";
            this.txtModelDir.Size = new System.Drawing.Size(197, 21);
            this.txtModelDir.TabIndex = 1;
            // 
            // btnSelModelDir
            // 
            this.btnSelModelDir.Location = new System.Drawing.Point(295, 18);
            this.btnSelModelDir.Name = "btnSelModelDir";
            this.btnSelModelDir.Size = new System.Drawing.Size(39, 21);
            this.btnSelModelDir.TabIndex = 2;
            this.btnSelModelDir.Text = "...";
            this.btnSelModelDir.UseVisualStyleBackColor = true;
            this.btnSelModelDir.Click += new System.EventHandler(this.btnSelModelDir_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(284, 203);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(59, 26);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "적용";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // PathSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnSelModelDir);
            this.Controls.Add(this.txtModelDir);
            this.Controls.Add(this.lbModelDir);
            this.Name = "PathSetting";
            this.Size = new System.Drawing.Size(357, 253);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbModelDir;
        private System.Windows.Forms.TextBox txtModelDir;
        private System.Windows.Forms.Button btnSelModelDir;
        private System.Windows.Forms.Button btnApply;
    }
}

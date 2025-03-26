namespace JidamVision
{
    partial class TeachForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.imageViewCCtrl1 = new JidamVision.ImageViewCCtrl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Reference Image";
            // 
            // imageViewCCtrl1
            // 
            this.imageViewCCtrl1.AutoSize = true;
            this.imageViewCCtrl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.imageViewCCtrl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageViewCCtrl1.Location = new System.Drawing.Point(30, 42);
            this.imageViewCCtrl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.imageViewCCtrl1.Name = "imageViewCCtrl1";
            this.imageViewCCtrl1.Size = new System.Drawing.Size(768, 465);
            this.imageViewCCtrl1.TabIndex = 5;
            // 
            // TeachForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1146, 697);
            this.Controls.Add(this.imageViewCCtrl1);
            this.Controls.Add(this.label1);
            this.Name = "TeachForm";
            this.Text = "TeachForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private ImageViewCCtrl imageViewCCtrl1;
    }
}
namespace JidamVision.Teach
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
            this.imageViewCCtrl1 = new JidamVision.ImageViewCCtrl();
            this.SuspendLayout();
            // 
            // imageViewCCtrl1
            // 
            this.imageViewCCtrl1.AutoSize = true;
            this.imageViewCCtrl1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.imageViewCCtrl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imageViewCCtrl1.Location = new System.Drawing.Point(17, 32);
            this.imageViewCCtrl1.Margin = new System.Windows.Forms.Padding(6);
            this.imageViewCCtrl1.Name = "imageViewCCtrl1";
            this.imageViewCCtrl1.Size = new System.Drawing.Size(467, 372);
            this.imageViewCCtrl1.TabIndex = 0;
            // 
            // TeachForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.imageViewCCtrl1);
            this.Name = "TeachForm";
            this.Text = "  ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ImageViewCCtrl imageViewCCtrl1;
    }
}
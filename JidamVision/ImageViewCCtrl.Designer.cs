namespace JidamVision
{
    partial class ImageViewCCtrl
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
            this.SuspendLayout();
            // 
            // ImageViewCCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Name = "ImageViewCCtrl";
            this.Size = new System.Drawing.Size(480, 375);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageViewCCtrl_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageViewCCtrl_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageViewCCtrl_MouseUp);
            this.Resize += new System.EventHandler(this.ImageViewCCtrl_Resize);
            this.ResumeLayout(false);

        }

        #endregion
    }
}

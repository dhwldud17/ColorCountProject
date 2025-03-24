namespace JidamVision.Setting
{
    partial class CameraSetting
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
			this.lbCameraType = new System.Windows.Forms.Label();
			this.cbCameraType = new System.Windows.Forms.ComboBox();
			this.btnApply = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbCameraType
			// 
			this.lbCameraType.AutoSize = true;
			this.lbCameraType.Location = new System.Drawing.Point(14, 30);
			this.lbCameraType.Name = "lbCameraType";
			this.lbCameraType.Size = new System.Drawing.Size(69, 12);
			this.lbCameraType.TabIndex = 0;
			this.lbCameraType.Text = "카메라 종료";
			// 
			// cbCameraType
			// 
			this.cbCameraType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCameraType.FormattingEnabled = true;
			this.cbCameraType.Location = new System.Drawing.Point(89, 27);
			this.cbCameraType.Name = "cbCameraType";
			this.cbCameraType.Size = new System.Drawing.Size(141, 20);
			this.cbCameraType.TabIndex = 1;
			// 
			// btnApply
			// 
			this.btnApply.Location = new System.Drawing.Point(264, 184);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(91, 26);
			this.btnApply.TabIndex = 2;
			this.btnApply.Text = "적용";
			this.btnApply.UseVisualStyleBackColor = true;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// CameraSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.cbCameraType);
			this.Controls.Add(this.lbCameraType);
			this.Name = "CameraSetting";
			this.Size = new System.Drawing.Size(386, 249);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbCameraType;
        private System.Windows.Forms.ComboBox cbCameraType;
        private System.Windows.Forms.Button btnApply;
    }
}

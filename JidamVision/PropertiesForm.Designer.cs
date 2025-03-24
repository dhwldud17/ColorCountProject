using WeifenLuo.WinFormsUI.Docking;

namespace JidamVision
{
    partial class PropertiesForm : DockContent
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
            this.tabPropControl = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // tabPropControl
            // 
            this.tabPropControl.Location = new System.Drawing.Point(12, 12);
            this.tabPropControl.Name = "tabPropControl";
            this.tabPropControl.SelectedIndex = 0;
            this.tabPropControl.Size = new System.Drawing.Size(318, 366);
            this.tabPropControl.TabIndex = 0;
            // 
            // PropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 390);
            this.Controls.Add(this.tabPropControl);
            this.Name = "PropertiesForm";
            this.Text = "PropertiesForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabPropControl;
    }
}
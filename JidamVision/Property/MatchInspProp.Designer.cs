﻿namespace JidamVision.Property
{
    partial class MatchInspProp
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.picTeachImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTeachImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picTeachImage
            // 
            this.picTeachImage.Location = new System.Drawing.Point(3, 3);
            this.picTeachImage.Name = "picTeachImage";
            this.picTeachImage.Size = new System.Drawing.Size(333, 321);
            this.picTeachImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTeachImage.TabIndex = 0;
            this.picTeachImage.TabStop = false;
            // 
            // MatchInspProp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picTeachImage);
            this.Name = "MatchInspProp";
            this.Size = new System.Drawing.Size(339, 327);
            ((System.ComponentModel.ISupportInitialize)(this.picTeachImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picTeachImage;
    }
}

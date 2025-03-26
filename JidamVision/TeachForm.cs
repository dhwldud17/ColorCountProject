using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using JidamVision.Algorithm;
using JidamVision.Core;
using JidamVision.Property;
using OpenCvSharp.Extensions;
using OpenCvSharp;
using WeifenLuo.WinFormsUI.Docking;

namespace JidamVision.Teach
{
    public partial class TeachForm : DockContent
    {


        public TeachForm()
        {
            InitializeComponent();

            // 이미지 클릭 시 색상 추출 이벤트 등록
            imageViewCCtrl1.MouseClick += ImageViewCCtrl1_MouseClick;
        }
        private void ImageViewCCtrl1_MouseClick(object sender, MouseEventArgs e)
        {
            // 1. 현재 이미지 가져오기
            Bitmap bitmap = imageViewCCtrl1.GetBitmap();
            if (bitmap == null)
            {
                MessageBox.Show("이미지가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. 클릭한 위치의 색상 추출 (스포이드 기능)
            if (e.X >= 0 && e.X < bitmap.Width && e.Y >= 0 && e.Y < bitmap.Height)
            {
                Color pickedColor = bitmap.GetPixel(e.X, e.Y);  // 클릭한 위치의 색상 가져오기
                lblPickedColor.BackColor = pickedColor;  // UI에서 색상 표시

                // 3. OpenCV Mat 변환
                Mat matImage = BitmapConverter.ToMat(bitmap);

                // 4. ColorBinaryInspProp을 사용하여 색상 기반 이진화 수행
                ColorBinaryInspProp colorInspProp = new ColorBinaryInspProp();
                Mat binaryImage = colorInspProp.ProcessColor(matImage, pickedColor);

                // 5. 결과 이미지를 UI에 표시
                if (binaryImage != null)
                {
                    imageViewCCtrl1.SetImage(BitmapConverter.ToBitmap(binaryImage));  // 이진화된 이미지 표시
                }
            }
            else
            {
                MessageBox.Show("이미지 범위를 벗어난 클릭입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // 색상 추출 후 처리
        private void ProcessColorExtraction(Color color)
        {
            // ColorBinaryInspProp 또는 ColorBlobAlgorithm과 연계하여 색상에 맞는 로직을 추가
            // 예시로, 색상 기반 바이너리 처리를 한다고 가정
            ColorBinaryInspProp colorInspProp = new ColorBinaryInspProp();
            colorInspProp.ProcessColor(color);

            // 또한 Blob 알고리즘에 색상 적용
            ColorBlobAlgorithm blobAlgorithm = new ColorBlobAlgorithm();
            blobAlgorithm.ApplyColor(color);
        }
    }
}

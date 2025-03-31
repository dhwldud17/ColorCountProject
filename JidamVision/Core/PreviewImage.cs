using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JidamVision.Property;
using JidamVision.Teach;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace JidamVision.Core
{
   
    public class PreviewImage
    {
        private Mat _orinalImage = null;
        private Mat _previewImage = null;
        private InspWindow _inspWindow = null;
        private bool _usePreview = false;


        //레퍼런스 이미지 가져옴
        public void SetImage(Mat image)
        {
            _orinalImage = image;  //imageload한게 _orinalImage에 들어감
            _previewImage = new Mat();
        }

        //검사할 이미지 가져옴.
        public void SetImage_Inspection(Mat image)
        {
            _orinalImage = image;  //imageload한게 _orinalImage에 들어감
            _previewImage = new Mat();
        }


        //usePreview 값에 따라 미리보기 화면을 설정
        public void SetPreview(bool usePreview)
        {
            _usePreview = usePreview;
            if (usePreview == false)
            {
                var cameraForm = MainForm.GetDockForm<CameraForm>();
                if (cameraForm == null)
                    return;

                Bitmap bmpImage = BitmapConverter.ToBitmap(_orinalImage);
                cameraForm.UpdateDisplay(bmpImage);
                return;
            }
        }

        public void SetPreview()
        {
            
            var InspectionForm = MainForm.GetDockForm<InspectionForm>();
            if (InspectionForm == null)
                return;
            Bitmap bmpImage = BitmapConverter.ToBitmap(_orinalImage);
            InspectionForm.UpdateDisplay(bmpImage);
            return;
          
        }



        public void SetInspWindow(InspWindow inspwindow)
        {
            _inspWindow = inspwindow;
        }

        //#BINARY FILTER#15 기존 이진화 프리뷰에, 배경없이 이진화 이미지만 보이는 모드 추가
        public void SetBinary(int lowerValue, int upperValue, bool invert, ShowBinaryMode showBinMode)
        {
            if (_usePreview == false)
                return;

            if (_orinalImage == null)
                return;

            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm == null)
                return;

            Bitmap bmpImage;
            if (showBinMode == ShowBinaryMode.ShowBinaryNone)
            {
                bmpImage = BitmapConverter.ToBitmap(_orinalImage);
                cameraForm.UpdateDisplay(bmpImage);
                return;
            }

            Rect windowArea = new Rect(0, 0, _orinalImage.Width, _orinalImage.Height);
            if (_inspWindow != null)
            {
                windowArea = _inspWindow.WindowArea;
            }

            Mat orgRoi = _orinalImage[windowArea];

            Mat grayImage = new Mat();
            if (orgRoi.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(orgRoi, grayImage, ColorConversionCodes.BGR2GRAY);
            else
                grayImage = orgRoi;

            Mat binaryMask = new Mat();
            Cv2.InRange(grayImage, lowerValue, upperValue, binaryMask);

            if (invert)
                binaryMask = ~binaryMask;

            // binaryMask는 ROI 사이즈이므로 fullBinaryMask로 확장
            Mat fullBinaryMask = Mat.Zeros(_orinalImage.Size(), MatType.CV_8UC1);
            binaryMask.CopyTo(new Mat(fullBinaryMask, windowArea));

            if (showBinMode == ShowBinaryMode.ShowBinaryOnly)
            {
                if (orgRoi.Type() == MatType.CV_8UC3)
                {
                    Mat colorBinary = new Mat();
                    Cv2.CvtColor(binaryMask, colorBinary, ColorConversionCodes.GRAY2BGR);
                    _previewImage = _orinalImage.Clone();
                    colorBinary.CopyTo(new Mat(_previewImage, windowArea));
                }
                else
                {
                    _previewImage = _orinalImage.Clone();
                    binaryMask.CopyTo(new Mat(_previewImage, windowArea));
                }

                bmpImage = BitmapConverter.ToBitmap(_previewImage);
                cameraForm.UpdateDisplay(bmpImage);
                return;
            }

            // 원본 이미지 복사본을 만들어 이진화된 부분에만 색을 덧씌우기
            Mat overlayImage;
            if (_orinalImage.Type() == MatType.CV_8UC1)
            {
                overlayImage = new Mat();
                Cv2.CvtColor(_orinalImage, overlayImage, ColorConversionCodes.GRAY2BGR);

                Mat colorOrinal = overlayImage.Clone();

                overlayImage.SetTo(new Scalar(0, 0, 255), fullBinaryMask); // 빨간색으로 마스킹

                // 원본과 합성 (투명도 적용)
                Cv2.AddWeighted(colorOrinal, 0.7, overlayImage, 0.3, 0, _previewImage);
            }
            else
            {
                overlayImage = _orinalImage.Clone();
                overlayImage.SetTo(new Scalar(0, 0, 255), fullBinaryMask); // 빨간색으로 마스킹

                // 원본과 합성 (투명도 적용)
                Cv2.AddWeighted(_orinalImage, 0.7, overlayImage, 0.3, 0, _previewImage);
            }


            bmpImage = BitmapConverter.ToBitmap(_previewImage);
            cameraForm.UpdateDisplay(bmpImage);
        }



        //#COLOR BINARY FILTER#15 기존 이진화 프리뷰에, 배경없이 이진화 이미지만 보이는 모드 추가
        public void SetColorBinary(Vec3b hsvMin, Vec3b hsvMax, bool invert, ShowColorBinaryMode showBinMode)
        {
            if (_orinalImage == null)
                return;

            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm == null)
                return;

            Bitmap bmpImage;
            if (showBinMode == ShowColorBinaryMode.ShowBinaryNone)
            {
                bmpImage = BitmapConverter.ToBitmap(_orinalImage);
                cameraForm.UpdateDisplay(bmpImage);
                return;
            }

            Mat hsvImage = new Mat();
            Cv2.CvtColor(_orinalImage, hsvImage, ColorConversionCodes.BGR2HSV);

            // 4️⃣ 이진화 처리 (hsvMin ~ hsvMax 사이의 값은 255(흰색), 나머지는 0(검은색))
            Mat binaryMask = new Mat();
            Cv2.InRange(hsvImage, new Scalar(hsvMin.Item0, hsvMin.Item1, hsvMin.Item2),
                                   new Scalar(hsvMax.Item0, hsvMax.Item1, hsvMax.Item2), binaryMask);

            if (invert)
                binaryMask = ~binaryMask;

            if (showBinMode == ShowColorBinaryMode.ShowBinaryOnly)
            {
                bmpImage = BitmapConverter.ToBitmap(binaryMask);
                cameraForm.UpdateDisplay(bmpImage);
                return;
            }

            // 컬러 강조를 위한 오버레이 이미지 생성 (빨간색 강조)
            Mat colorOverlay = new Mat(_orinalImage.Size(), _orinalImage.Type(), new Scalar(0, 0, 255));
            Mat overlayImage = _orinalImage.Clone();
            colorOverlay.CopyTo(overlayImage, binaryMask);

            // 원본과 강조 이미지 합성 (투명도 적용)
            Cv2.AddWeighted(_orinalImage, 0.7, overlayImage, 0.3, 0, _previewImage);

            bmpImage = BitmapConverter.ToBitmap(_previewImage);
            cameraForm.UpdateDisplay(bmpImage);
        }

        
        

        

                

      
    }
}

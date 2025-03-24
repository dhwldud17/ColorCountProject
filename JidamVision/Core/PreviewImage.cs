using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JidamVision.Property;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace JidamVision.Core
{
    enum ImageOperation
    {
        OpAdd = 0,         // 덧셈
        OpSubtract,        // 뺄셈
        OpMultiply,        // 곱셈
        OpDivide,          // 나눗셈
        OpMax,             // 최대값
        OpMin,             // 최소값
        OpAbs,             // 절댓값
        OpAbsDiff          // 절댓값 차이
    }

    // 비트 연산 (AND, OR, XOR 등)을 위한 열거형
    enum Bitwise
    {
        OnAnd = 0,         // AND
        OnOr,              // OR
        OnXor,             // XOR
        OnNot,             // NOT
        OnCompare          // 비교
    }

    // 이미지 필터링 (블러, 박스 필터 등)을 위한 열거형
    enum ImageFilter
    {
        FilterBlur = 0,           // 블러
        FilterBoxFilter,          // 박스 필터
        FilterMedianBlur,         // 미디안 블러
        FilterGaussianBlur,       // 가우시안 블러
        FilterBilateral           // 양방향 필터
    }

    // 가장자리 검출 (Sobel, Scharr, Laplacian, Canny 등)을 위한 열거형
    enum ImageEdge
    {
        FilterSobel = 0,          // Sobel 필터
        FilterScharr,             // Scharr 필터
        FilterLaplacian,          // Laplacian 필터
        FilterCanny               // Canny 엣지 검출
    }
    public class PreviewImage
    {
        private Mat _orinalImage = null;
        private Mat _previewImage = null;
        //private Mat _tempImage = null;

        public void SetImage(Mat image)
        {
            _orinalImage = image;
            _previewImage = new Mat();
            //_previewImage = null;
            //_tempImage = new Mat(image.Size(), MatType.CV_8UC1,new Scalar(0));
        }


        //#BINARY FILTER#15 기존 이진화 프리뷰에, 배경없이 이진화 이미지만 보이는 모드 추가
        public void SetBinary(int lowerValue, int upperValue, bool invert, ShowBinaryMode showBinMode)
        {
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

            Mat grayImage = new Mat();
            if (_orinalImage.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(_orinalImage, grayImage, ColorConversionCodes.BGR2GRAY);
            else
                grayImage = _orinalImage;

            Mat binaryMask = new Mat();
            //Cv2.Threshold(grayImage, binaryMask, lowerValue, upperValue, ThresholdTypes.Binary);
            Cv2.InRange(grayImage, lowerValue, upperValue, binaryMask);

            if (invert)
                binaryMask = ~binaryMask;


            if (showBinMode == ShowBinaryMode.ShowBinaryOnly)
            {
                bmpImage = BitmapConverter.ToBitmap(binaryMask);
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

                overlayImage.SetTo(new Scalar(0, 0, 255), binaryMask); // 빨간색으로 마스킹

                // 원본과 합성 (투명도 적용)
                Cv2.AddWeighted(colorOrinal, 0.7, overlayImage, 0.3, 0, _previewImage);
            }
            else
            {
                overlayImage = _orinalImage.Clone();
                overlayImage.SetTo(new Scalar(0, 0, 255), binaryMask); // 빨간색으로 마스킹

                // 원본과 합성 (투명도 적용)
                Cv2.AddWeighted(_orinalImage, 0.7, overlayImage, 0.3, 0, _previewImage);
            }


            bmpImage = BitmapConverter.ToBitmap(_previewImage);
            cameraForm.UpdateDisplay(bmpImage);
        }


        static void ApplyImageOperation(ImageOperation operation, Mat src1, string op_value, out Mat resultImage) // 이미지 연산 코드
                                                                                                                  // 아래 코드는 이미지 연산을 수행하는 코드로, 두 이미지를 연산하여 결과를 보여주는 방식
                                                                                                                  // 예시: 덧셈, 뺄셈, 곱셈, 나눗셈, 최대값, 최소값 등을 계산할 수 있음
        {
            // 공백으로 구분된 값이 3개인지 확인
            string[] values = op_value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            //if (string.IsNullOrWhiteSpace(op_value)) // 빈 값 처리
            //{
            //    // 유효하지 않은 입력이 있을 경우 오류 메시지 표시
            //    MessageBox.Show("연산값을 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //if (values.Length != 3)
            //{
            //    // 유효하지 않은 입력이 있을 경우 오류 메시지 표시
            //    MessageBox.Show("연산값은 공백으로 구분된 3개의 숫자를 입력해야 합니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //return;
            //}

            //// 숫자인지 확인
            //bool isValid = true;
            //foreach (string value in values)
            //{
            //    if (!int.TryParse(value, out _))  // 숫자가 아닌 값이 있을 경우
            //    {
            //        isValid = false;
            //        break;
            //    }
            //}

            //if (!isValid)
            //{
            //    // 숫자가 아닌 값이 있을 경우 오류 메시지 표시
            //    MessageBox.Show("모든 연산값은 숫자여야 합니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //return;
            //}

            // 각각의 값을 정수로 변환
            int value1 = Convert.ToInt32(values[0]);
            int value2 = Convert.ToInt32(values[1]);
            int value3 = Convert.ToInt32(values[2]);

            Mat src2 = new Mat(src1.Size(), MatType.CV_8UC3, new Scalar(value1, value2, value3)); //두 번째 이미지 소스(연산값 받아옴)
            Mat dst = new Mat();

            switch (operation)
            {
                case ImageOperation.OpAdd:
                    Cv2.Add(src1, src2, dst);  // 두 이미지를 더하기
                    break;
                case ImageOperation.OpSubtract:
                    Cv2.Subtract(src1, src2, dst);  // 두 이미지의 차이 구하기
                    break;
                case ImageOperation.OpMultiply:
                    Cv2.Multiply(src1, src2, dst);  // 두 이미지를 곱하기
                    break;
                case ImageOperation.OpDivide:
                    Cv2.Divide(src1, src2, dst);  // 두 이미지 나누기
                    break;
                case ImageOperation.OpMax:
                    Cv2.Max(src1, src2, dst);  // 두 이미지의 최대값 비교
                    break;
                case ImageOperation.OpMin:
                    Cv2.Min(src1, src2, dst);  // 두 이미지의 최소값 비교
                    break;
                case ImageOperation.OpAbs:
                    Cv2.Multiply(src1, src2, dst);
                    Cv2.Abs(dst); // 절대값 계산
                    break;
                case ImageOperation.OpAbsDiff:
                    Mat matMul = new Mat();
                    Cv2.Multiply(src1, src2, matMul);
                    Cv2.Absdiff(src1, matMul, dst); // 절대값 차이 계산
                    break;
            }
            resultImage = dst;



        }

        static void ApplyBitwiseOperation(Bitwise operation, Mat src1, out Mat resultImage)  // 이미지 Bitwise 연산 코드
                                                                                             // 두 이미지를 비트 연산(AND, OR, XOR, NOT 등)으로 결합하는 예시
        {
            Mat dst = new Mat();
            Mat src2 = src1.Flip(FlipMode.Y); //Y축 기준으로 반전한 이미지 
            switch (operation)
            {
                case Bitwise.OnAnd:
                    Cv2.BitwiseAnd(src1, src2, dst);  // AND 연산
                    break;
                case Bitwise.OnOr:
                    Cv2.BitwiseOr(src1, src2, dst);  // OR 연산
                    break;
                case Bitwise.OnXor:
                    Cv2.BitwiseXor(src1, src2, dst);  // XOR 연산
                    break;
                case Bitwise.OnNot:
                    Cv2.BitwiseNot(src1, dst);  // NOT 연산
                    break;
                case Bitwise.OnCompare:
                    // 비트 비교 연산 추가 가능
                    break;
            }
            resultImage = dst;
        }

        static void ApplyImageFiltering(ImageFilter filterType, Mat src, out Mat resultImage) // 블러링 효과 코드
                                                                                              // 다양한 필터를 사용하여 이미지에 흐림 효과를 적용하는 예시
        {
            Mat dst = new Mat();
            switch (filterType)
            {
                case ImageFilter.FilterBlur:
                    Cv2.Blur(src, dst, new OpenCvSharp.Size(5, 5));  // 블러 필터 적용
                    break;
                case ImageFilter.FilterBoxFilter:
                    Cv2.BoxFilter(src, dst, src.Depth(), new OpenCvSharp.Size(30, 30));  // 박스 필터 적용
                    break;
                case ImageFilter.FilterMedianBlur:
                    Cv2.MedianBlur(src, dst, 31);  // 미디안 블러 적용
                    break;
                case ImageFilter.FilterGaussianBlur:
                    Cv2.GaussianBlur(src, dst, new OpenCvSharp.Size(31, 31), 0);  // 가우시안 블러 적용
                    break;
                case ImageFilter.FilterBilateral:
                    Cv2.BilateralFilter(src, dst, 9, 75, 75);  // 양방향 필터 적용
                    break;
            }
            resultImage = dst;
        }

        static void ApplyEdgeDetection(ImageEdge edgeType, Mat src, out Mat resultImage)// 엣지(가장자리) 검출 코드
                                                                                        // Sobel, Scharr, Laplacian, Canny 필터를 사용해 이미지의 가장자리를 검출하는 예시
        {
            Mat dst = new Mat();
            switch (edgeType)
            {
                case ImageEdge.FilterSobel:
                    Cv2.Sobel(src, dst, MatType.CV_8U, 1, 1);  // Sobel 필터 적용
                    break;
                case ImageEdge.FilterScharr:
                    Cv2.Scharr(src, dst, MatType.CV_8U, 1, 0);  // Scharr 필터 적용
                    break;
                case ImageEdge.FilterLaplacian:
                    Cv2.Laplacian(src, dst, MatType.CV_8U);  // Laplacian 필터 적용
                    break;
                case ImageEdge.FilterCanny:
                    Cv2.Canny(src, dst, 100, 200);  // Canny 엣지 검출 적용
                    break;
            }
            resultImage = dst;
        }

        //필터 효과 기능
        public void ApplyFilter(String selected_filter1,int selected_filter2)
        {
            if (_orinalImage == null)
                return;
            var cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm == null)
                return;
            Mat filteredImage = new Mat();
            Bitmap bmpImage;

            switch (selected_filter1)
            {
                case "연산":
                    // 연산 관련 enum 값 매핑
                    ImageOperation operation = (ImageOperation)selected_filter2;
                    string op_values = "30 30 30";  //연산값 지정
                    ApplyImageOperation(operation, _orinalImage, op_values, out filteredImage);
                    break;
                case "비트연산(Bitwise)":
                    // 비트 연산 관련 enum 값 매핑
                    Bitwise bitwise = (Bitwise)selected_filter2;
                    ApplyBitwiseOperation(bitwise, _orinalImage, out filteredImage);
                    break;
                case "블러링":
                    ImageFilter filter = (ImageFilter)selected_filter2;
                    ApplyImageFiltering(filter,_orinalImage,out filteredImage);
                    break;
                case "Edge":
                    ImageEdge edge = (ImageEdge)selected_filter2;
                    ApplyEdgeDetection(edge, _orinalImage, out filteredImage);
                    break;
                
                default:
                    return;
            }

            _previewImage = filteredImage;
            bmpImage = BitmapConverter.ToBitmap(_previewImage);
            cameraForm.UpdateDisplay(bmpImage);
            //var cameraForm = MainForm.GetDockForm<CameraForm>();
            //if (cameraForm != null)
            //{
            //    Bitmap bmpImage  = BitmapConverter.ToBitmap(_previewImage);
            //    cameraForm.UpdateDisplay(bmpImage);
            //}

        }
    }
}

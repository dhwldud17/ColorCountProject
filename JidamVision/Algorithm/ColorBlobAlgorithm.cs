using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using JidamVision.Property;

namespace JidamVision.Algorithm
{
    // HSV 범위 구조체
    public struct HSVThreshold
    {
        public Scalar lower;
        public Scalar upper;
        public bool invert;
    }
    public class ColorBlobAlgorithm : InspAlgorithm
    {
        public HSVThreshold HSVThreshold { get; set; } = new HSVThreshold();
        // 픽셀 영역 필터링 (기본값 100)
       
       


        public ColorBlobAlgorithm()
        {
            InspectType = InspectType.InspColorBinary; // 새로운 타입 추가
        }

        // HSV 값을 바탕으로 색상 범위를 설정
        // 컬러 이진화 필터 함수
        private Mat ColorBlobFilter(Mat hsvImage)
        {
            // 설정된 HSV 범위 적용
            Scalar lowerBound = new Scalar(HSVThreshold.lower.Val0, HSVThreshold.lower.Val1, HSVThreshold.lower.Val2);
            Scalar upperBound = new Scalar(HSVThreshold.upper.Val0, HSVThreshold.upper.Val1, HSVThreshold.upper.Val2);

            Mat mask = new Mat();
            Cv2.InRange(hsvImage, lowerBound, upperBound, mask);

            // 반전 여부 처리
            if (HSVThreshold.invert)
                Cv2.BitwiseNot(mask, mask);

            return mask;
        }

        // 컬러 이진화 후 원하는 영역을 얻음 
        public override bool DoInspect()
        {
            IsInspected = false;

            if (_srcImage == null)
                return false;

            Mat hsvImage = new Mat();
            Cv2.CvtColor(_srcImage, hsvImage, ColorConversionCodes.BGR2HSV); // 이미지 HSV로 변환

            // 필터 적용
            Mat mask = ColorBlobFilter(hsvImage);

            // 빨간색 마스크 생성하여 원본 이미지와 합성
            Mat redMask = new Mat(_srcImage.Size(), _srcImage.Type(), new Scalar(0, 0, 255));
            Mat result = new Mat();
            Cv2.BitwiseAnd(redMask, redMask, result, mask);
            Cv2.BitwiseOr(_srcImage, result, _srcImage); // 원본 이미지에 빨간색 영역 추가


            // 윤곽선 찾기
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(mask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            IsInspected = true;
            return true;
        }
    }
}
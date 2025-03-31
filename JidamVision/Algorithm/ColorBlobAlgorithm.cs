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
        internal static readonly object Instance;
        internal static readonly object SetColor;

        public HSVThreshold HSVThreshold { get; set; } = new HSVThreshold();
        // 픽셀 영역 필터링 (기본값 100)

        private List<Rect> _referenceAreas; // 레퍼런스 이미지에서 추출된 영역
        private List<Rect> _findArea; // 검사 이미지에서 추출된 영역

        public ColorBlobAlgorithm()
        {
            InspectType = InspectType.InspColorBinary; // 새로운 타입 추가
            _referenceAreas = new List<Rect>();
            _findArea = new List<Rect>();
        }

        // HSV 값을 바탕으로 색상 범위를 설정
        // 컬러 이진화 필터 함수

        // 색상 비교 함수 (색상 매칭)
        private bool CompareColorMatch(Mat referenceImage, Mat testImage, Rect referenceRect, Rect testRect)
        {
            // 두 이미지의 해당 영역을 추출
            Mat referenceRegion = new Mat(referenceImage, referenceRect);
            Mat testRegion = new Mat(testImage, testRect);

            // 두 이미지의 평균 색상 계산 (HSV로 변환 후 평균 색상 계산)
            Mat referenceHSV = new Mat();
            Mat testHSV = new Mat();

            Cv2.CvtColor(referenceRegion, referenceHSV, ColorConversionCodes.BGR2HSV);
            Cv2.CvtColor(testRegion, testHSV, ColorConversionCodes.BGR2HSV);

            // 평균 색상 계산
            Scalar referenceMean = Cv2.Mean(referenceHSV);
            Scalar testMean = Cv2.Mean(testHSV);

            // 색상 차이 계산 (HSV의 H, S, V 값 차이)
            double hDiff = Math.Abs(referenceMean.Val0 - testMean.Val0);
            double sDiff = Math.Abs(referenceMean.Val1 - testMean.Val1);
            double vDiff = Math.Abs(referenceMean.Val2 - testMean.Val2);

            // 색상 차이가 일정 범위 이내이면 매칭 성공
            double maxH = 10; // 허용하는 색상 차이 범위 (Hue)
            double maxS = 50; // 허용하는 채도 차이 범위 (Saturation)
            double maxV = 50; // 허용하는 명도 차이 범위 (Value)

            return hDiff <= maxH && sDiff <= maxS && vDiff <= maxV;
        }


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
        private List<Rect> ProcessImage(Mat image)
        {
            Mat hsvImage = new Mat();
            Cv2.CvtColor(image, hsvImage, ColorConversionCodes.BGR2HSV); // 이미지 HSV로 변환

            // 필터 적용
            Mat mask = ColorBlobFilter(hsvImage);

            // 윤곽선 찾기
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(mask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            List<Rect> foundAreas = new List<Rect>();
            foreach (var contour in contours)
            {
                Rect rect = Cv2.BoundingRect(contour);
                foundAreas.Add(rect);
            }

            return foundAreas;
        }

        // 검사 이미지 설정
        public void SetSourceImage(Mat srcImage)
        {
            _srcImage = srcImage;
        }
        // 기준 이미지 설정 (티칭)
        public void SetReferenceImage(Mat refImage)
        {
            // 기준 이미지에서 검사 영역 추출
            _referenceAreas = ProcessImage(refImage);
        }
        // 기준 이미지와 검사 이미지 비교
        private bool CompareAreas(List<Rect> referenceAreas, List<Rect> detectedAreas)
        {
            if (referenceAreas.Count != detectedAreas.Count)
                return false;

            for (int i = 0; i < referenceAreas.Count; i++)
            {
                if (!referenceAreas[i].Equals(detectedAreas[i]))
                    return false;
            }

            return true;
        }
        // 컬러 이진화 후 원하는 영역을 얻음 
        public override bool DoInspect()
        {
            IsInspected = false;

            if (_srcImage == null || _referenceAreas == null || _referenceAreas.Count == 0)
                return false;
            
            // 검사 이미지에서 영역 추출
            _findArea = ProcessImage(_srcImage);

            // 기준 이미지와 검사 이미지 비교
            bool isMatch = true;

            for (int i = 0; i < _referenceAreas.Count; i++)
            {
                if (i >= _findArea.Count || !CompareColorMatch(_srcImage, _srcImage, _referenceAreas[i], _findArea[i]))
                {
                    isMatch = false;
                    break;
                }
            }
            //Mat hsvImage = new Mat();
            //Cv2.CvtColor(_srcImage, hsvImage, ColorConversionCodes.BGR2HSV); // 이미지 HSV로 변환

            //// 필터 적용
            //Mat mask = ColorBlobFilter(hsvImage);

            //// 빨간색 마스크 생성하여 원본 이미지와 합성
            //Mat redMask = new Mat(_srcImage.Size(), _srcImage.Type(), new Scalar(0, 0, 255));
            //Mat result = new Mat();
            //Cv2.BitwiseAnd(redMask, redMask, result, mask);
            //Cv2.BitwiseOr(_srcImage, result, _srcImage); // 원본 이미지에 빨간색 영역 추가


            //// 윤곽선 찾기
            //Point[][] contours;
            //HierarchyIndex[] hierarchy;
            //Cv2.FindContours(mask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            //// 검출된 영역 저장
            //foreach (var contour in contours)
            //{
            //    Rect rect = Cv2.BoundingRect(contour);
            //    _findArea.Add(rect);
            //}
            // OK / NG 판단
            IsDefect = !isMatch;
            ResultString = isMatch ? new List<string> { "OK" } : new List<string> { "NG" };
            IsInspected = true;
            return true;

        }

        public Mat GetOutput()
        {
            return _srcImage; // 빨간색 영역이 덮인 최종 이미지
        }

        public void SetImage(Mat image)
        {
            _srcImage = image;
        }
    }
}
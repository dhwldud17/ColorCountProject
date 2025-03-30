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
            bool isMatch = CompareAreas(_referenceAreas, _findArea);
            IsDefect = !isMatch;
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
            ResultString = isMatch ? new List<string> { "OK" } : new List<string> { "NG" };
            IsInspected = true;
            return true;
        }
        //결과값 보냄.
        // 검사 결과가 Rect정보로 출력이 가능하다면, 이 함수를 상속 받아서, 정보 반환
        public override int GetResultRect(out List<Rect> resultArea)
        {
            resultArea = _findArea;
            return _findArea.Count;
        }

        // 결과 초기화
        public override void ResetResult()
        {
            IsInspected = false;
            IsDefect = false;
            ResultString.Clear();
            _findArea.Clear();
        }
    }
}
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using JidamVision.Property;

namespace JidamVision.Algorithm
{
    // HSV 범위 구조체
    public struct HsvRange
    {
        public int HueLower, HueUpper; // Hue 범위
        public int SaturationLower, SaturationUpper; // Saturation 범위
        public int ValueLower, ValueUpper; // Value 범위
        public bool Invert;  // 결과 반전 여부

        public HsvRange(int hueL, int hueU, int satL, int satU, int valL, int valU, bool invert)
        {
            HueLower = hueL;
            HueUpper = hueU;
            SaturationLower = satL;
            SaturationUpper = satU;
            ValueLower = valL;
            ValueUpper = valU;
            Invert = invert;
        }
    }

    public class ColorBlobAlgorithm : InspAlgorithm
    {
        private Mat _srcImage; // 원본 이미지 저장

        public Mat SourceImage
        {
            get { return _srcImage; }
            set { _srcImage = value; }
        }
        private List<Rect> _findArea;

        public HsvRange ColorRange { get; set; } = new HsvRange();

        // 픽셀 영역 필터링 (기본값 100)
        public int AreaFilter { get; set; } = 100;

        public ColorBlobAlgorithm()
        {
            InspectType = InspectType.InspColorBinary; // 새로운 타입 추가
        }

        // HSV 값을 바탕으로 색상 범위를 설정
        public void SetColorRange(int hueL, int hueU, int satL, int satU, int valL, int valU, bool invert)
        {
            ColorRange = new HsvRange(hueL, hueU, satL, satU, valL, valU, invert);
        }

        // 컬러 이진화 후 원하는 영역을 얻음 
        public override bool DoInspect()
        {
            IsInspected = false;

            if (_srcImage == null)
                return false;

            Mat hsvImage = new Mat();
            Cv2.CvtColor(_srcImage, hsvImage, ColorConversionCodes.BGR2HSV); // 이미지 HSV로 변환

            // 범위 설정
            Scalar lowerBound = new Scalar(ColorRange.HueLower, ColorRange.SaturationLower, ColorRange.ValueLower);
            Scalar upperBound = new Scalar(ColorRange.HueUpper, ColorRange.SaturationUpper, ColorRange.ValueUpper);

            // 범위에 맞는 마스크 생성
            Mat mask = new Mat();
            Cv2.InRange(hsvImage, lowerBound, upperBound, mask);

            // 반전 여부 처리
            if (ColorRange.Invert)
                mask = ~mask;

            // 빨간색 마스크 생성하여 원본 이미지와 합성
            Mat redMask = new Mat(_srcImage.Size(), _srcImage.Type(), new Scalar(0, 0, 255));
            Mat result = new Mat();
            Cv2.BitwiseAnd(redMask, redMask, result, mask);
            Cv2.BitwiseOr(_srcImage, result, _srcImage); // 원본 이미지에 빨간색 영역 추가

            // 윤곽선 찾기
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(mask, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            _findArea = new List<Rect>();
            foreach (var contour in contours)
            {
                Rect rect = Cv2.BoundingRect(contour);
                if (rect.Width * rect.Height >= AreaFilter)  // 영역 필터 적용
                {
                    _findArea.Add(rect);
                }
            }

            // 검출된 전선 개수 출력
            int wireCount = _findArea.Count;
            Console.WriteLine($"검출된 전선 개수: {wireCount}");

            // 검출된 전선에 사각형 표시 (디버깅용)
            foreach (var rect in _findArea)
            {
                Cv2.Rectangle(_srcImage, rect, new Scalar(0, 255, 0), 2);  // 초록색 테두리 표시
            }

            IsInspected = true;
            return true;
        }
    }
}
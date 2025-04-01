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
    public class ColorBlobAlgorithm : BlobAlgorithm
    {
        internal static readonly object Instance;
        internal static readonly object SetColor;
        
        public HSVThreshold HSVThreshold { get; set; } = new HSVThreshold();
        // 픽셀 영역 필터링 (기본값 100)

        private List<Rect> _referenceAreas; // 레퍼런스 이미지에서 추출된 영역
        private List<Rect> _findArea; // 검사 이미지에서 추출된 영역

     public double BinaryArea { get;  set; } // 검출된 영역의 총 면적
        public BinaryThreshold BinThreshold { get; set; } = new BinaryThreshold();

        //픽셀 영역으로 이진화 필터
        public int AreaMin { get; set; } = 50;
        public int AreaMax { get; set; } = 500;

        public int WidthMin { get; set; } = 0;
        public int WidthMax { get; set; } = 0;

        public int HeightMin { get; set; } = 0;
        public int HeightMax { get; set; } = 0;
        public int ColorBlobCount { get; set; } = 0;
        public int OutColorBlobCount { get; set; } = 0;

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
            double totalArea = 0;

            foreach (var contour in contours)
            {
                Rect rect = Cv2.BoundingRect(contour);
                foundAreas.Add(rect);
                //**
                totalArea += rect.Width * rect.Height;
            }
            BinaryArea = totalArea;  // 이진화 영역의 총 면적 저장
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

        //해당 부분이진화한게 area값 해당 기준에 맞는지?
        public override bool DoInspect()
        {
            IsInspected = false;
            OutColorBlobCount = 0;

            if (_srcImage == null)
                return false;

            // HSV 변환
            Mat hsvImage = new Mat();
            Cv2.CvtColor(_srcImage, hsvImage, ColorConversionCodes.BGR2HSV);

            // HSV 범위 필터링 → 이진 마스크
            Mat mask = ColorBlobFilter(hsvImage);

            // 이진 마스크를 이용해 Blob 개수 세기
            // → 이 조건들은 사용자 설정값 (areaMin 등)으로 조정 가능
            int areaMin = 50;
            int areaMax = 0;  // 0이면 무제한
            int widthMin = 0, widthMax = 0, heightMin = 0, heightMax = 0;

            bool blobOk = ColorBlobFilter(mask, areaMin, areaMax, widthMin, widthMax, heightMin, heightMax);

            // BlobFilter 안에서 OutColorBlobCount가 설정됨
            OutColorBlobCount = OutColorBlobCount;

            // 검사 결과 판정
            if (ColorBlobCount > 0)
            {
                IsDefect = (OutColorBlobCount != ColorBlobCount);  // 개수 다르면 NG
            }
            else
            {
                IsDefect = !blobOk; // BlobFilter 실패 시 NG
            }

            // 디버깅용 결과 로그
            ResultString.Add($"[ColorBlob] Detected: {OutColorBlobCount}, Expected: {ColorBlobCount}");
            ResultString.Add(IsDefect ? "[Result] NG" : "[Result] OK");

            IsInspected = true;
            return true;
        }
        //public override bool DoInspect()
        //{
        //    IsInspected = false;

        //    if (_srcImage == null) //검사이미지 없으면 false반환
        //        return false;

        //    // 검사 이미지에서 영역 추출
        //    _findArea = ProcessImage(_srcImage);




        //    IsInspected = true;
        //    return true;

        //}

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
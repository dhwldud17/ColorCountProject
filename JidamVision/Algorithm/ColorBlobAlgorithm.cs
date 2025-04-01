using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using JidamVision.Property;
using JidamVision.Teach;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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

        public int CustomAreaMin { get; set; } = 100; // 원하는 기본값 설정 가능
        public int CustomAreaMax { get; set; } = 1000;

        public double BinaryArea { get;  set; } // 검출된 영역의 총 면적
  
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
        // 클래스 내부에서 이미지 인덱스 관리 (전역 변수 선언)
        int imageIndex = 0;
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
            _findArea.Clear();
            int findBlobCount = 0;
            //// 각 영역에 부여할 ID 값
             int rectId = 1;
            Console.WriteLine($"=== [이미지 번호: {imageIndex}] ===");
            imageIndex++;
            foreach (var contour in contours)
            {   
                Rect rect = Cv2.BoundingRect(contour);
                double area = Cv2.ContourArea(contour);
             Console.WriteLine($"[이미지 {imageIndex}, CABEL 번호: {InspWindow.window}] Area: {area}, Rect: {rect.X}, {rect.Y}, {rect.Width}, {rect.Height}");
                
                 rectId++;  // 다음 객체를 위해 ID 증가
                if (area <= 5)
                { //area가 5이하인 경우 같은 색깔 아니라고 판단.
                  
                    continue;
                }




                    findBlobCount++; //색깔이 같은 것으로 판단

                //어떤 번호가 같은지 나오도록.

                string blobRecInfo = $"[이미지 {imageIndex}, CABEL 번호: {rectId}] 통과한 rec: X:{rect.X}, Y:{rect.Y}, Size({rect.Width},{rect.Height})";
                Console.WriteLine(blobRecInfo);
                ResultString.Add(blobRecInfo);
                foundAreas.Add(rect);
               
            }
            OutBlobCount = findBlobCount;

            if (findBlobCount > 0)
            {
                string result = "NG";
                if (findBlobCount == 9)
                {
                    result = "OK";
                }
                string resultInfo = "";
                resultInfo = $"[{result}] match blob count [in : {BlobCount},out : {findBlobCount}]";
                Console.Write(resultInfo);
                ResultString.Add(resultInfo);
            }

            
            return _findArea;
        }

        // 검사 이미지 설정
        public void SetSourceImage(Mat srcImage)
        {
            _srcImage = srcImage;
        }
     
       
        
        // 컬러 이진화 후 원하는 영역을 얻음 

        //해당 부분이진화한게 area값 해당 기준에 맞는지?

        

        public override bool DoInspect()
        {
            ResetResult();
            OutBlobCount = 0;
            IsInspected = false;

            if (_srcImage == null) //검사이미지 없으면 false반환
                return false;

            // 검사 이미지에서 영역 추출
            List<Rect> detectedAreas = ProcessImage(_srcImage);
            _findArea = detectedAreas;  // 명확하게 업데이트





            IsInspected = true;

            return true;

        }
        public override int GetResultRect(out List<Rect> resultArea)
        {
            resultArea = null;

            //#ABSTRACT ALGORITHM#7 검사가 완료되지 않았다면, 리턴
            if (!IsInspected)
                return -1;

            if (_findArea is null || _findArea.Count <= 0)
                return -1;

            resultArea = _findArea;
            return resultArea.Count;
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
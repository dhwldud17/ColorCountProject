using JidamVision.Core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;

namespace JidamVision.Algorithm
{
    //#BINARY FILTER#1 이진화 필터를 위한 클래스
    

    //이진화 임계값 설정을 구조체로 만들기
    public struct BinaryThreshold
    {
        public int lower;
        public int upper;
        public bool invert;
    }

    public class BlobAlgorithm : InspAlgorithm
    {
        //이진화 필터로 찾은 영역
        private List<Rect> _findArea;

        public BinaryThreshold BinThreshold { get; set; } = new BinaryThreshold();

        //픽셀 영역으로 이진화 필터
        public int AreaFilter { get; set; } = 100;

        public BlobAlgorithm()
        {
            //#ABSTRACT ALGORITHM#5 각 함수마다 자신의 알고리즘 타입 설정
            InspectType = InspectType.InspBinary;
        }

        //#BINARY FILTER#2 이진화 후, 필터를 이용해 원하는 영역을 얻음 

        //#ABSTRACT ALGORITHM#6 
        //InspAlgorithm을 상속받아, 구현하고, 인자로 입력받던 것을 부모의 _srcImage 이미지 사용
        //검사 시작전 IsInspected = false로 초기화하고, 검사가 정상적으로 완료되면,IsInspected = true로 설정
        public override bool DoInspect()
        {
            IsInspected = false;

            if (_srcImage == null)
                return false;

            Mat grayImage = new Mat();
            if (_srcImage.Type() == MatType.CV_8UC3)
                Cv2.CvtColor(_srcImage, grayImage, ColorConversionCodes.BGR2GRAY);
            else
                grayImage = _srcImage;

            Mat binaryImage = new Mat();
            //Cv2.Threshold(grayImage, binaryMask, lowerValue, upperValue, ThresholdTypes.Binary);
            Cv2.InRange(grayImage, BinThreshold.lower, BinThreshold.upper, binaryImage);

            if (BinThreshold.invert)
                binaryImage = ~binaryImage;

            if(AreaFilter > 0)
            {
                if (!BlobFilter(binaryImage, AreaFilter))
                    return false;
            }

            IsInspected = true;

            return true;
        }

        //#BINARY FILTER#3 이진화 필터처리 함수
        private bool BlobFilter(Mat binImage, int areaFilter)
        {
            // 컨투어 찾기
            Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binImage, out contours, out hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

            // 필터링된 객체를 담을 리스트
            Mat filteredImage = Mat.Zeros(binImage.Size(), MatType.CV_8UC1);

            if(_findArea is null)
                _findArea = new List<Rect>();

            _findArea.Clear();

            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area < areaFilter)
                    continue;

                // 필터링된 객체를 이미지에 그림
                //Cv2.DrawContours(filteredImage, new Point[][] { contour }, -1, Scalar.White, -1);

                // RotatedRect 정보 계산
                //RotatedRect rotatedRect = Cv2.MinAreaRect(contour);
                Rect boundingRect = Cv2.BoundingRect(contour);

                _findArea.Add(boundingRect);

                // RotatedRect 정보 출력
                //Console.WriteLine($"RotatedRect - Center: {rotatedRect.Center}, Size: {rotatedRect.Size}, Angle: {rotatedRect.Angle}");

                // BoundingRect 정보 출력
                //Console.WriteLine($"BoundingRect - X: {boundingRect.X}, Y: {boundingRect.Y}, Width: {boundingRect.Width}, Height: {boundingRect.Height}");

            }

            return true;
        }


        //#BINARY FILTER#4 이진화 영역 반환
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
    }
}

﻿using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JidamVision.Algorithm
{
    //HUE, SATURATION, VALUE 범위에 맞는 픽셀만 남기고 나머지는 제거하는 방식
    //->이진화된 결과는 흑백(0, 255) 마스크 형태로 출력
    /*1.입력 이미지 → HSV 변환

      2.HUE, SATURATION, VALUE 범위 설정

      3.설정된 범위 내의 픽셀만 255(흰색), 나머지는 0(검은색)

      4.이진화된 마스크 출력

    --> 해당 HSV색상 범위를 이용해 특정 색상을 찾고, 해당 영역 사각형으로 반환
    */
    public struct ColorThreshold  //UI에서 이값 넣어서 보내주세요~ //BinaryInsProp에 btnFilter_Click 부분에서 알고리즘에 값 보내는 부분 참조 부탁
    {
        public Scalar lower; // HSV 최소값 (H,S,V)
        public Scalar upper; // HSV 최대값
        public bool invert;  // 결과 반전 여부
    }



    public class ColorBlobAlgorithm : InspAlgorithm
    {

        private List<Rect> _findArea;

        // ColorThreshold는 ColorBlobAlgorithm의 필드로 설정
        private ColorThreshold _colorRange;
        public ColorThreshold ColorRange
        {
            get { return _colorRange; }
            set { _colorRange = value; }
        }

        // 픽셀 영역 필터링 (기본값 100)
        public int AreaFilter { get; set; } = 100;

        public ColorBlobAlgorithm()
        {
            InspectType = InspectType.InspColorBinary; // 새로운 타입 추가
        }

        // HSV 값을 바탕으로 색상 범위를 설정
        public void SetColorRange(int hCenter, int sMin, int vMin)
        {
            // 색상 범위 설정 (HSV)
            _colorRange.lower = new Scalar(hCenter - 10, sMin - 50, vMin - 50); // 범위는 예시로 설정, 필요시 조정
            _colorRange.upper = new Scalar(hCenter + 10, sMin + 50, vMin + 50);
            _colorRange.invert = false; // 필요시 반전 여부 설정
        }

        public override bool DoInspect()
        {
            IsInspected = false;

            if (_srcImage == null)
                return false;

            Mat hsvImage = new Mat();
            Cv2.CvtColor(_srcImage, hsvImage, ColorConversionCodes.BGR2HSV);
            //입력된 컬러이미지(_srcImage)를 HSV로 변환


            Mat mask = new Mat();
            Cv2.InRange(hsvImage, _colorRange.lower, _colorRange.upper, mask);
            //입력된 HSV 값과 비교하여 마스크 생성. -> ColorRange.lower ~ ColorRange.upper 사이 색상만 남기고 나머지는 검정색으로 변환 
            //-> 우리가 찾고자 하는 영역은 흰색.(255)


            if (ColorRange.invert)
                mask = ~mask;



            //같은 색상인 부분 빨간색 픽셀로 표시되게 .색상은 바꿀수잇음

            // 빨간색 마스크 생성
            Mat redMask = new Mat(_srcImage.Size(), _srcImage.Type(), new Scalar(0, 0, 255)); // 빨간색
                                                                                              // 마스크를 원본 이미지에 적용
            Mat result = new Mat();
            Cv2.BitwiseAnd(redMask, redMask, result, mask); // 빨간색 영역 생성
            Cv2.BitwiseOr(_srcImage, result, _srcImage); // 원본 이미지와 합성

            //if (AreaFilter > 0)
            //{
            //    if (!BlobFilter(binaryImage, AreaFilter))
            //        return false;
            //}

            IsInspected = true;
            return true;
        }


       

       


    }
}

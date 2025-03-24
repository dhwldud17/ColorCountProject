using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JidamVision.Core
{
    //#MODEL#1 InspWindowType 정의
    public enum InspWindowType
    {
        None = 0,
        Global,
        Group,
        Base,
        Sub,
        ID,
    }

    public static class Define
    {
        //# SAVE ROI#4 전역적으로, ROI 저장 파일명을 설정
        //Define.cs 클래스 생성 먼저 할것
        public static readonly string ROI_IMAGE_NAME = "RoiImage.png";
    }
}

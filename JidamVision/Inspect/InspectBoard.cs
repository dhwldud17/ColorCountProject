using JidamVision.Algorithm;
using JidamVision.Property;
using JidamVision.Teach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JidamVision.Inspect
{
    public class InspectBoard
    {
        public InspectBoard()
        {
        }

        public bool Inspect(InspWindow window)
        {
            if (window is null)
                return false;

            if (window.InspWindowType == Core.InspWindowType.Group)
            {
                GroupWindow group = (GroupWindow)window;
                if (!InspectWindowList(group.Members))
                    return false;
            }
            else
            {
                if (!InspectWindow(window))
                    return false;
            }

            return true;
        }

        private bool InspectWindow(InspWindow window)
        {
            window.ResetInspResult();
            foreach (InspAlgorithm algo in window.AlgorithmList)
            {
                //if (algo.IsUse == false)
                //    continue;
                if (algo.InspectType != InspectType.InspColorBinary)
                    continue; // 다른 검사 유형은 건너뜀
                if (!algo.DoInspect())
                    return false;

                string resultInfo = string.Join("\r\n", algo.ResultString);

                InspResult inspResult = new InspResult
                {
                    ObjectID = window.UID, //검사 창(window)의 고유 ID를 저장.
                    InspType = algo.InspectType, // 어떤 검사 유형인지 저장.
                    IsDefect = algo.IsDefect, //불량(Defect) 여부 저장.
                    ResultInfos = resultInfo //검사 결과 문자열 저장.
                };

                ColorBlobAlgorithm colorblobAlgo = algo as ColorBlobAlgorithm;
                

                List<Rect> resultArea = new List<Rect>();
                int resultCnt = algo.GetResultRect(out resultArea);
                inspResult.ResultRectList = resultArea;

                window.AddInspResult(inspResult);
            }

            return true;
        }

        public bool InspectWindowList(List<InspWindow> windowList)
        {
            if (windowList.Count <= 0)
                return false;

            //ID 윈도우가 매칭알고리즘이 있고, 검사가 되었다면, 오프셋을 얻는다.
            //  Point alignOffset = new Point(0, 0);
            //우리는 오프셋 없음.


            //Base ROI에서 카운트 검사 비교
            //InspWindow baseWindow = windowList.Find(w => w.InspWindowType == Core.InspWindowType.Base);
            //if (baseWindow != null)
            //{
            //    int baseCount = GetBaseROICount(baseWindow); // Base ROI 개수 가져오기
            //    int expectedCount = 9; // 기준 개수  -> 레퍼런스 이미지 카운트한걸로 수정하기.

            //    if (baseCount != expectedCount)
            //    {
            //        Console.WriteLine($"[Base ROI] NG - 감지된 개수: {baseCount}, 기대값: {expectedCount}");
            //        return false;
            //    }
            //    Console.WriteLine("[Base ROI] OK");
            //}

            //  Cable colorblob 검사->area조건 넘은게 9개면 통과?(기존 1개만 검사 → 전체 검사)
            InspWindow cabelWindow = windowList.Find(w => w.InspWindowType == Core.InspWindowType.Cabel);
 
           
            if (cabelWindow != null)
            {
               ColorBlobAlgorithm colorblobAlgo = (ColorBlobAlgorithm)cabelWindow.FindInspAlgorithm(InspectType.InspColorBinary);
               
                if (colorblobAlgo != null && colorblobAlgo.IsUse)
                {
                    if (!InspectWindow(cabelWindow))
                        return false;

                    if (colorblobAlgo.IsInspected)
                    {

                        //컬러 이진화 후 추출된 영역 값
                        double areaValue = colorblobAlgo.BinaryArea;
                      
                      
                    }
                }
            }
         

            Console.WriteLine("전체 검사 OK");
            return true;



        }
    }
}
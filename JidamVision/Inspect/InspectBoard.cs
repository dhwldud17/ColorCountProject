using JidamVision.Algorithm;
using JidamVision.Property;
using JidamVision.Teach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

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

                if (!algo.DoInspect())
                    return false;

                string resultInfo = string.Join("\r\n", algo.ResultString);

                InspResult inspResult = new InspResult
                {
                    ObjectID = window.UID,
                    InspType = algo.InspectType,
                    IsDefect = algo.IsDefect,
                    ResultInfos = resultInfo
                };

                switch (algo.InspectType)
                {
                    case InspectType.InspMatch:
                        MatchAlgorithm matchAlgo = algo as MatchAlgorithm;
                        inspResult.ResultValue = $"{matchAlgo.OutScore}";
                        break;
                    case InspectType.InspBinary:
                        BlobAlgorithm blobAlgo = algo as BlobAlgorithm;
                        inspResult.ResultValue = $"{blobAlgo.OutBlobCount}/{blobAlgo.BlobCount}";
                        break;
                    case InspectType.InspColorBinary:
                        ColorBlobAlgorithm colorblobAlgo = algo as ColorBlobAlgorithm;
                       // inspResult.ResultValue = $"{colorblobAlgo.OutBlobCount}/{colorblobAlgo.BlobCount}";
                        break;
                }

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
            InspWindow baseWindow = windowList.Find(w => w.InspWindowType == Core.InspWindowType.Base);
            if (baseWindow != null)
            {
                int baseCount = GetBaseROICount(baseWindow); // Base ROI 개수 가져오기
                int expectedCount = 9; // 기준 개수  -> 레퍼런스 이미지 카운트한걸로 수정하기.

                if (baseCount != expectedCount)
                    return false; // NG (개수가 다름)
            }

            //Cable colorblob 검사 -> area조건 넘은게 9개면 통과?(기존 1개만 검사 → 전체 검사)
            List<InspWindow> cableWindows = windowList.FindAll(w => w.InspWindowType == Core.InspWindowType.Cable);
            foreach (InspWindow cableWindow in cableWindows)
            {
                ColorBlobAlgorithm colorblobAlgo = (ColorBlobAlgorithm)cableWindow.FindInspAlgorithm(InspectType.InspColorBinary);
                if (colorblobAlgo != null && colorblobAlgo.IsUse)
                {
                    if (!InspectWindow(cableWindow))
                        return false;

                    if (colorblobAlgo.IsInspected)
                    {
                        double areaValue = colorblobAlgo.BinaryArea;
                        double threshold = 100.0; // 예제 기준값
                        if (Math.Abs(areaValue - threshold) > 10) // 기준값과 차이가 크면 NG
                            return false;
                    }
                }
            }


            foreach (InspWindow window in windowList)
            {
                //모든 윈도우에 오프셋 반영
                window.SetInspOffset(alignOffset);
                // 검사 실행, NG가 하나라도 나오면 전체 NG 처리
                if (!InspectWindow(window))
                    return false;
            }

            return true;  //모두 통과시 OK


















































































































































        }
    }
}
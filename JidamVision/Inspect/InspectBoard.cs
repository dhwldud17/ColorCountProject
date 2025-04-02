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
        public Mat TargetImage { get; set; } // 🔹 targetImage를 저장할 필드 추가
        public bool Inspect(InspWindow window)
        {
            if (window is null)
                return false;

            if (window.InspWindowType == Core.InspWindowType.Group)
            {
                GroupWindow group = (GroupWindow)window;
                // if (!InspectWindowList(group.Members))
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
                inspResult.ResultValue = $"{colorblobAlgo.OutBlobCount}/{colorblobAlgo.BlobCount}";

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
                ModelTreeForm modelTreeForm = new ModelTreeForm();  // 객체 생성
                int baseCount = modelTreeForm.GetBaseRoi(baseWindow); // Base ROI 개수 가져오기
                int expectedCount = 9; // 기준 개수  -> 레퍼런스 이미지 카운트한걸로 수정하기.

                if (baseCount != expectedCount)
                {
                    Console.WriteLine($"[Base ROI] NG - 감지된 개수: {baseCount}, 기대값: {expectedCount}");
                    return false;
                }
                Console.WriteLine("[Base ROI] OK");
            }

            //  Cable colorblob 검사->area조건 넘은게 9개면 통과?(기존 1개만 검사 → 전체 검사)


            List<InspWindow> cableWindows = windowList.FindAll(w => w.InspWindowType == Core.InspWindowType.Cabel);

          //  int OKCount = 0;
            foreach (InspWindow cableWindow in cableWindows)
            {
                ColorBlobAlgorithm colorblobAlgo = (ColorBlobAlgorithm)cableWindow.FindInspAlgorithm(InspectType.InspColorBinary);
                if (colorblobAlgo != null && colorblobAlgo.IsUse)
                {
                    //if (!InspectWindow(cableWindow))
                    //    return false;
                   
                   
                  
                     Console.WriteLine($"\n[ColorBlob 검사] ROI ID: {cableWindow.UID}\n");
                    //cabel하나씩 실행
                    colorblobAlgo.DoInspect();
                    //여기에 위에 실행한 결과 받아올수있도록 코드 추가
                    if (colorblobAlgo.OutBlobCount == 9)
                    {
                      // OKCount++;
                        Console.WriteLine($"[ColorBlob 검사] OK - 감지된 개수: {colorblobAlgo.OutBlobCount}");
                    }
                    else
                    {
                        Console.WriteLine($"[ColorBlob 검사] NG - 감지된 개수: {colorblobAlgo.OutBlobCount}");
                    }


                }
            }


            Console.WriteLine("전체 검사 OK");
            return true;



        }
    }
}
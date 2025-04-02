using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JidamVision.Inspect
{
    public class InspectionManager
    {
        private static InspectionManager _instance;
        public static InspectionManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InspectionManager();
                return _instance;
            }
        }

        public List<InspectionResult> AllResults { get; private set; }

        private InspectionManager()
        {
            AllResults = new List<InspectionResult>();
        }

        // 1️⃣ 먼저 imageIndex만 저장
        public void AddPartialResult(int imageIndex)
        {
            if (!AllResults.Any(r => r.ImageIndex == imageIndex)) // 중복 방지
            {
                AllResults.Add(new InspectionResult(imageIndex, 0, new List<string>()));
            }
        }

        // 2️⃣ 특정 imageIndex의 wireCount만 업데이트
        public void UpdateWireCount(int imageIndex, int wireCount)
        {
            var result = AllResults.FirstOrDefault(r => r.ImageIndex == imageIndex);
            if (result != null)
            {
                result.WireCount = wireCount;
            }
        }

        // 3️⃣ 특정 imageIndex의 개별 cable 검사 결과 추가
        public void AddCableResult(int imageIndex, string cableResult)
        {
            var result = AllResults.FirstOrDefault(r => r.ImageIndex == imageIndex);
            if (result != null)
            {
                result.CableResults.Add(cableResult);
            }
        }
    }
}

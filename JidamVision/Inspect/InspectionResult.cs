using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JidamVision.Inspect
{
    public class InspectionResult
    {
        public int ImageIndex { get; set; }
        public int WireCount { get; set; }
        public List<string> CableResults { get; set; }

        public InspectionResult(int imageIndex, int wireCount, List<string> cableResults)
        {
            ImageIndex = imageIndex;
            WireCount = wireCount;
            CableResults = cableResults;
        }
    }
}

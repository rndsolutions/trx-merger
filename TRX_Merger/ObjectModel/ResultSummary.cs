using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRX_Merger.ObjectModel
{
    public class ResultSummary
    {
        public string Outcome { get; set; }
        public Counters Counters { get; set; }
        public List<RunInfo> RunInfos { get; set; }
    }
}

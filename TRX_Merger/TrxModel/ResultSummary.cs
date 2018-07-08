using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRX_Merger.TrxModel
{
    public class ResultSummary
    {
        public string Outcome { get; set; }
        public Counters Counters { get; set; }
		public ResultSummaryOutput Output { get; set; }
        public List<RunInfo> RunInfos { get; set; }
		public List<Collector> CollectorDataEntries { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRX_Merger.ObjectModel
{
    public class Counters
    {
        public int Total { get; set; }
        public int Еxecuted { get; set; }
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Timeout { get; set; }
        public int Aborted { get; set; }
        public int Inconclusive { get; set; }
        public int PassedButRunAborted { get; set; }
        public int NotRunnable { get; set; }
        public int Disconnected { get; set; }
        public int Warning { get; set; }
        public int NotExecuted { get; set; }
        public int Completed { get; set; }
        public int InProgress { get; set; }
        public int Pending { get; set; }
    }
}

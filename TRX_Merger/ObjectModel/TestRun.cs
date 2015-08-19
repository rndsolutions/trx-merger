using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRX_Merger.ObjectModel
{
    public class TestRun
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string RunUser { get; set; }
        public Times Times { get; set; }
        public List<UnitTestResult> Results { get; set; }
        public List<UnitTest> TestDefinitions { get; set; }
        public List<TestEntry> TestEntries { get; set; }
        public List<TestList> TestLists { get; set; }
        public ResultSummary ResultSummary { get; set; }
    }
}

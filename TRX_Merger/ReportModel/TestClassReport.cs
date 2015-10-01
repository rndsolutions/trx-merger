using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TRX_Merger.ReportModel
{
    public class TestClassReport
    {
        public TestClassReport(string testClass, List<UnitTestResultReport> tests)
        {
            TestClassName = testClass;

            Tests = tests;

            TotalTests = tests.Count;

            Passed = tests.Count(t => t.Result.Outcome == "Passed");

            Failed = tests.Count(t => t.Result.Outcome == "Failed");

            Aborted = tests.Count(t => t.Result.Outcome == "Aborted");

            Timeout = tests.Count(t => t.Result.Outcome == "Timeout");

            var durations = tests.Select(t => TimeSpan.Parse(t.Result.Duration)).ToList<TimeSpan>();
            Duration = new TimeSpan();
            durations.ForEach(d => Duration += d);

            Dll = tests[0].Dll;
        }

        public string TestClassName { get; set; }
        public string FriendlyTestClassName
        {
            get
            {
                return TestClassName.Split(new char[] { '.' }).Last();
            }         
        }

        public List<UnitTestResultReport> Tests { get; set; }

        public string Dll { get; set; }

        public int TotalTests { get; set; }
        
        public int Passed { get; set; }

        public int Failed { get; set; }

        public int Timeout { get; set; }
        
        public int Aborted { get; set; }

        public TimeSpan Duration { get; set; }
    }
}
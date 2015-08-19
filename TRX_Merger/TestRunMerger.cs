using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRX_Merger.ObjectModel;
using TRX_Merger.Utilities;

namespace TRX_Merger
{
    public static class TestRunMerger
    {

        public static void MergeTRXsAndSave(List<string> trxFiles, string outputFile)
        {
            Console.WriteLine("Deserializing trx files:");
            List<TestRun> runs = new List<TestRun>();
            foreach (var trx in trxFiles)
            {
                Console.WriteLine(trx);
                runs.Add(TRXSerializationUtils.DeserializeTRX(trx));
            }

            Console.WriteLine("Combining deserialized trx files...");
            var combinedTestRun = MergeTestRuns(runs);

            Console.WriteLine("Saving result...");
            var savedFile = TRXSerializationUtils.SerializeAndSaveTestRun(combinedTestRun, outputFile);

            Console.WriteLine("Operation completed:");
            Console.WriteLine("\tCombined trx files: " + trxFiles.Count);
            Console.WriteLine("\tResult trx file: " + savedFile); 
        }

        private static TestRun MergeTestRuns(List<TestRun> testRuns)
        {
            string name = "";
            string runUser = "";

            string startString = "";
            DateTime startDate = DateTime.MaxValue;

            string endString = "";
            DateTime endDate = DateTime.MinValue;




            List<UnitTestResult> allResults = new List<UnitTestResult>();
            List<UnitTest> allTestDefinitions = new List<UnitTest>();
            List<TestEntry> allTestEntries = new List<TestEntry>();
            List<TestList> allTestLists = new List<TestList>();


            var resultSummary = new ResultSummary
            {
                Counters = new Counters(),
                RunInfos = new List<RunInfo>(),
            };
            bool resultSummaryPassed = true;

            foreach (var tr in testRuns)
            {
                allResults = allResults.Concat(tr.Results).ToList();
                allTestDefinitions = allTestDefinitions.Concat(tr.TestDefinitions).ToList();
                allTestEntries = allTestEntries.Concat(tr.TestEntries).ToList();
                allTestLists = allTestLists.Concat(tr.TestLists).ToList();

                DateTime currStart = DateTime.Parse(tr.Times.Start);
                if (currStart < startDate)
                {
                    startDate = currStart;
                    startString = tr.Times.Start;
                }

                DateTime currEnd = DateTime.Parse(tr.Times.Finish);
                if (currEnd > endDate)
                {
                    endDate = currEnd;
                    endString = tr.Times.Finish;
                }


                resultSummaryPassed &= tr.ResultSummary.Outcome == "Passed";
                resultSummary.RunInfos = resultSummary.RunInfos.Concat(tr.ResultSummary.RunInfos).ToList();
                resultSummary.Counters.Aborted += tr.ResultSummary.Counters.Aborted;
                resultSummary.Counters.Completed += tr.ResultSummary.Counters.Completed;
                resultSummary.Counters.Disconnected += tr.ResultSummary.Counters.Disconnected;
                resultSummary.Counters.Еxecuted += tr.ResultSummary.Counters.Еxecuted;
                resultSummary.Counters.Failed += tr.ResultSummary.Counters.Failed;
                resultSummary.Counters.Inconclusive += tr.ResultSummary.Counters.Inconclusive;
                resultSummary.Counters.InProgress += tr.ResultSummary.Counters.InProgress;
                resultSummary.Counters.NotExecuted += tr.ResultSummary.Counters.NotExecuted;
                resultSummary.Counters.NotRunnable += tr.ResultSummary.Counters.NotRunnable;
                resultSummary.Counters.Passed += tr.ResultSummary.Counters.Passed;
                resultSummary.Counters.PassedButRunAborted += tr.ResultSummary.Counters.PassedButRunAborted;
                resultSummary.Counters.Pending += tr.ResultSummary.Counters.Pending;
                resultSummary.Counters.Timeout += tr.ResultSummary.Counters.Timeout;
                resultSummary.Counters.Total += tr.ResultSummary.Counters.Total;
                resultSummary.Counters.Warning += tr.ResultSummary.Counters.Warning;

            }

            resultSummary.Outcome = resultSummaryPassed ? "Passed" : "Failed";


            return new TestRun
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                RunUser = runUser,
                Times = new Times
                {
                    Start = startString,
                    Queuing = startString,
                    Creation = startString,
                    Finish = endString,
                },
                Results = allResults,
                TestDefinitions = allTestDefinitions,
                TestEntries = allTestEntries,
                TestLists = allTestLists,
                ResultSummary = resultSummary,
            };
        } 
 
    }
}

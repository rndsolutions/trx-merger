using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TRX_Merger.TrxModel;

namespace TRX_Merger.Utilities
{
    public static class TRXSerializationUtils
    {
        private static string ns = "{http://microsoft.com/schemas/VisualStudio/TeamTest/2010}";


        #region Serializers
        internal static string SerializeAndSaveTestRun(TestRun testRun, string targetPath)
        {

            try
            {

                Console.WriteLine("Started serializeAndSaveTestRun");
                XNamespace xmlns = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010";
                XDocument doc =
                    new XDocument(
                      new XElement("TestRun",
                            new XAttribute("id", testRun.Id),
                            new XAttribute("name", testRun.Name),
                            new XAttribute("runUser", testRun.RunUser),
                            new XElement("Times",
                                new XAttribute("creation", testRun.Times.Creation),
                                new XAttribute("queuing", testRun.Times.Queuing),
                                new XAttribute("start", testRun.Times.Start),
                                new XAttribute("finish", testRun.Times.Finish)),
                            new XElement("Results",
                                testRun.Results.Select(
                                    utr =>
                                        new XElement("UnitTestResult",
                                            new XAttribute("computerName", utr.ComputerName),
                                            new XAttribute("duration", utr.Duration),
                                            new XAttribute("endTime", utr.EndTime),
                                            new XAttribute("executionId", utr.ExecutionId),
                                            new XAttribute("outcome", utr.Outcome),
                                            new XAttribute("startTime", utr.StartTime),
                                            new XAttribute("testId", utr.TestId),
                                            new XAttribute("testListId", utr.TestListId),
                                            new XAttribute("testName", utr.TestName),
                                            new XAttribute("testType", utr.TestType),
                                            new XElement("Output",
                                                 utr.Output.StdOut == null ? null : new XElement("StdOut", utr.Output.StdOut),
                                                 utr.Output.ErrorInfo == null ? null :
                                                 new XElement("ErrorInfo",
                                                      utr.Output.ErrorInfo.Message == null ? null : new XElement("Message", utr.Output.ErrorInfo.Message),
                                                      utr.Output.ErrorInfo.StackTrace == null ? null : new XElement("StackTrace", utr.Output.ErrorInfo.StackTrace)
                                                      ))))),
                          new XElement("TestDefinitions",
                               testRun.TestDefinitions.Select(
                                    td => new XElement("UnitTest",
                                             new XAttribute("id", td.Id),
                                             new XAttribute("name", td.Name),
                                             new XAttribute("storage", td.Storage == null ? "" : td.Storage),
                                             new XElement("Execution",
                                                new XAttribute("id", td.Execution.Id)),
                                             new XElement("TestMethod",
                                                new XAttribute("adapterTypeName", td.TestMethod.AdapterTypeName == null ? "" : td.Storage),
                                                new XAttribute("className", td.TestMethod.ClassName),
                                                new XAttribute("codeBase", td.TestMethod.CodeBase),
                                                new XAttribute("name", td.TestMethod.Name))))),
                            new XElement("TestEntries",
                                 testRun.TestEntries.Select(
                                    te => new XElement("TestEntry",
                                              new XAttribute("testId", te.TestId),
                                              new XAttribute("executionId", te.ExecutionId),
                                              new XAttribute("testListId", te.TestListId)))),
                            new XElement("TestLists",
                                testRun.TestLists.Distinct().Select(
                                     tl => new XElement("TestList",
                                         new XAttribute("name", tl.Name),
                                         new XAttribute("id", tl.Id)))),
                            new XElement("ResultSummary",
                                new XAttribute("outcome", testRun.ResultSummary.Outcome),
                                new XElement("Counters",
                                    new XAttribute("aborted", testRun.ResultSummary.Counters.Aborted),
                                    new XAttribute("completed", testRun.ResultSummary.Counters.Completed),
                                    new XAttribute("disconnected", testRun.ResultSummary.Counters.Disconnected),
                                    new XAttribute("executed", testRun.ResultSummary.Counters.Еxecuted),
                                    new XAttribute("failed", testRun.ResultSummary.Counters.Failed),
                                    new XAttribute("inconclusive", testRun.ResultSummary.Counters.Inconclusive),
                                    new XAttribute("inProgress", testRun.ResultSummary.Counters.InProgress),
                                    new XAttribute("notExecuted", testRun.ResultSummary.Counters.NotExecuted),
                                    new XAttribute("notRunnable", testRun.ResultSummary.Counters.NotRunnable),
                                    new XAttribute("passed", testRun.ResultSummary.Counters.Passed),
                                    new XAttribute("passedButRunAborted", testRun.ResultSummary.Counters.PassedButRunAborted),
                                    new XAttribute("pending", testRun.ResultSummary.Counters.Pending),
                                    new XAttribute("timeout", testRun.ResultSummary.Counters.Timeout),
                                    new XAttribute("total", testRun.ResultSummary.Counters.Total),
                                    new XAttribute("warning", testRun.ResultSummary.Counters.Warning)),
                                new XElement("RunInfos",
                                    testRun.ResultSummary.RunInfos.Select(
                                        ri => new XElement("RunInfo",
                                            new XAttribute("computerName", ri.ComputerName),
                                            new XAttribute("outcome", ri.Outcome),
                                            new XAttribute("timestamp", ri.Timestamp),
                                            new XElement("Text", ri.Text)))))
                                 )
                    );

                Console.WriteLine("Finished parsing xml properties");
                doc.Root.SetDefaultXmlNamespace("http://microsoft.com/schemas/VisualStudio/TeamTest/2010");

                if (File.Exists(targetPath))
                    File.Delete(targetPath);

                doc.Save(targetPath);

                var savedFileInfo = new FileInfo(targetPath);

                return savedFileInfo.FullName;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "/n" + ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region Deserializers
        internal static TestRun DeserializeTRX(string trxPath)
        {
            TestRun testRun = new TestRun();

            using (Stream trxStream = new FileStream(trxPath, FileMode.Open, FileAccess.Read))
            {
                XDocument doc = XDocument.Load(trxStream);
                var run = doc.Root;

                testRun.Id = run.Attribute("id").Value;
                testRun.Name = run.Attribute("name").Value;
                testRun.RunUser = run.Attribute("runUser").Value;

                testRun.Times = DeserializeTimes(doc.Descendants(ns + "Times").FirstOrDefault());
                testRun.Results = DeserializeResults(doc.Descendants(ns + "UnitTestResult"));
                testRun.TestDefinitions = DeserializeTestDefinitions(doc.Descendants(ns + "UnitTest"));
                testRun.TestEntries = DeserializeTestEntries(doc.Descendants(ns + "TestEntry"));
                testRun.TestLists = DeserializeTestLists(doc.Descendants(ns + "TestList"));
                testRun.ResultSummary = DeserializeResultSummary(doc.Descendants(ns + "ResultSummary").FirstOrDefault());
            }
            return testRun;
        }

        private static ResultSummary DeserializeResultSummary(XElement resultSummary)
        {
            ResultSummary res = new ResultSummary
            {
                Outcome = resultSummary.GetAttributeValue("outcome"),
                Counters = DeserializeCounters(resultSummary),
                RunInfos = DeserializeRunInfos(resultSummary.Descendants(ns + "RunInfo"))
            };

            return res;
        }

        private static List<RunInfo> DeserializeRunInfos(IEnumerable<XElement> runInfos)
        {
            List<RunInfo> result = new List<RunInfo>();

            foreach (var rf in runInfos)
            {
                var computerName = rf.GetAttributeValue("computerName");
                var outcome = rf.GetAttributeValue("outcome");
                var timestamp = rf.GetAttributeValue("timestamp");

                result.Add(new RunInfo
                {
                    ComputerName = computerName,
                    Outcome = outcome,
                    Timestamp = timestamp,
                    Text = DeserializeText(rf),
                });
            }
            return result;
        }

        private static string DeserializeText(XElement rf)
        {
            var txt = rf.Descendants(ns + "Text").FirstOrDefault();
            if (txt == null)
                return null;

            return txt.Value;
        }

        private static Counters DeserializeCounters(XElement resultSummary)
        {
            var cc = resultSummary.Descendants(ns + "Counters").FirstOrDefault();
            if (cc == null)
                return null;

            return new Counters
            {
                Aborted = int.Parse(cc.GetAttributeValue("aborted")),
                Completed = int.Parse(cc.GetAttributeValue("completed")),
                Disconnected = int.Parse(cc.GetAttributeValue("disconnected")),
                Еxecuted = int.Parse(cc.GetAttributeValue("executed")),
                Failed = int.Parse(cc.GetAttributeValue("failed")),
                Inconclusive = int.Parse(cc.GetAttributeValue("inconclusive")),
                InProgress = int.Parse(cc.GetAttributeValue("inProgress")),
                NotExecuted = int.Parse(cc.GetAttributeValue("notExecuted")),
                NotRunnable = int.Parse(cc.GetAttributeValue("notRunnable")),
                Passed = int.Parse(cc.GetAttributeValue("passed")),
                PassedButRunAborted = int.Parse(cc.GetAttributeValue("passedButRunAborted")),
                Pending = int.Parse(cc.GetAttributeValue("pending")),
                Timeout = int.Parse(cc.GetAttributeValue("timeout")),
                Total = int.Parse(cc.GetAttributeValue("total")),
                Warning = int.Parse(cc.GetAttributeValue("warning")),
            };
        }

        private static List<TestList> DeserializeTestLists(IEnumerable<XElement> testList)
        {
            List<TestList> result = new List<TestList>();

            foreach (var tl in testList)
            {
                var name = tl.GetAttributeValue("name");
                var id = tl.GetAttributeValue("id");

                result.Add(new TestList
                {
                    Id = id,
                    Name = name,
                });
            }
            return result;
        }

        private static List<TestEntry> DeserializeTestEntries(IEnumerable<XElement> testEntries)
        {
            List<TestEntry> result = new List<TestEntry>();

            foreach (var def in testEntries)
            {
                var testId = def.GetAttributeValue("testId");
                var executionId = def.GetAttributeValue("executionId");
                var testListId = def.GetAttributeValue("testListId");

                result.Add(new TestEntry
                {
                    TestId = testId,
                    ExecutionId = executionId,
                    TestListId = testListId,
                });
            }
            return result;
        }

        private static List<UnitTest> DeserializeTestDefinitions(IEnumerable<XElement> testDefinitions)
        {
            List<UnitTest> result = new List<UnitTest>();

            foreach (var def in testDefinitions)
            {
                var name = def.GetAttributeValue("name");
                var storage = def.GetAttributeValue("storage");
                var id = def.GetAttributeValue("id");
                var Execution = DeserializeExecution(def);
                var TestMethod = DeserializeTestMethod(def);

                result.Add(new UnitTest
                {
                    Id = id,
                    Name = name,
                    Storage = storage,
                    Execution = Execution,
                    TestMethod = TestMethod
                });

            }

            return result;
        }

        private static Execution DeserializeExecution(XElement unitTest)
        {
            var exec = unitTest.Descendants(ns + "Execution").FirstOrDefault();
            if (exec == null)
                return null;

            return new Execution
            {
                Id = exec.GetAttributeValue("id"),
            };
        }

        private static TestMethod DeserializeTestMethod(XElement unitTest)
        {
            var tm = unitTest.Descendants(ns + "TestMethod").FirstOrDefault();
            if (tm == null)
                return null;

            return new TestMethod
            {
                CodeBase = tm.GetAttributeValue("codeBase"),
                AdapterTypeName = tm.GetAttributeValue("adapterTypeName"),
                ClassName = tm.GetAttributeValue("className"),
                Name = tm.GetAttributeValue("name"),
            };
        }

        private static List<UnitTestResult> DeserializeResults(IEnumerable<XElement> results)
        {
            List<UnitTestResult> result = new List<UnitTestResult>();

            foreach (var res in results)
            {

                var computerName = res.GetAttributeValue("computerName");
                var duration = res.GetAttributeValue("duration");
                var endTime = res.GetAttributeValue("endTime");
                var executionId = res.GetAttributeValue("executionId");
                var outcome = res.GetAttributeValue("outcome");
                var relativeResultsDirectory = res.GetAttributeValue("relativeResultsDirectory");
                var startTime = res.GetAttributeValue("startTime");
                var testId = res.GetAttributeValue("testId");
                var testListId = res.GetAttributeValue("testListId");
                var testName = res.GetAttributeValue("testName");
                var testType = res.GetAttributeValue("testType");
                var Output = new UnitTestResultOutput
                {
                    StdOut = DeserializeStdOut(res),
                    ErrorInfo = DeserializeErrorInfo(res),
                };
                result.Add(new UnitTestResult
                {
                    ComputerName = computerName,
                    Duration = string.IsNullOrEmpty(duration) ? "0:0:0" : duration,
                    EndTime = endTime,
                    ExecutionId = executionId,
                    Outcome = outcome,
                    Output = Output,
                    RelativeResultsDirectory = relativeResultsDirectory,
                    StartTime = startTime,
                    TestId = testId,
                    TestListId = testListId,
                    TestName = testName,
                    TestType = testType
                });
            }

            return result;
        }

        private static ErrorInfo DeserializeErrorInfo(XElement unitTestResult)
        {
            var err = unitTestResult.Descendants(ns + "ErrorInfo").FirstOrDefault();
            if (err == null)
                return null;

            return new ErrorInfo
            {
                Message = err.Descendants(ns + "Message").FirstOrDefault().Value,
                StackTrace = err.Descendants(ns + "StackTrace").FirstOrDefault().Value,
            };
        }

        private static string DeserializeStdOut(XElement unitTestResult)
        {
            var stdOut = unitTestResult.Descendants(ns + "StdOut").FirstOrDefault();
            if (stdOut == null)
                return null;

            return unitTestResult.Value;
        }

        private static Times DeserializeTimes(XElement xElement)
        {
            return new Times
            {
                Creation = xElement.Attribute("creation").Value,
                Finish = xElement.Attribute("finish").Value,
                Queuing = xElement.Attribute("queuing").Value,
                Start = xElement.Attribute("start").Value,
            };
        }
        #endregion

    }
}

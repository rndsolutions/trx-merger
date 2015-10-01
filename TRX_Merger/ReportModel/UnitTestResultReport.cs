using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using TRX_Merger.TrxModel;

namespace TRX_Merger.ReportModel
{
    public class UnitTestResultReport
    {
        public UnitTestResultReport(UnitTestResult result)
        {
            Result = result;

            if (!string.IsNullOrEmpty(Result.Output.StdOut))
            {
                if (Result.Output.StdOut.Contains("-> done")
                    || Result.Output.StdOut.Contains("-> error")
                    || Result.Output.StdOut.Contains("-> skipped"))
                {
                    //set cucumber output
                    cucumberStdOut = new List<KeyValuePair<string, string>>();
                    var rows = Result.Output.StdOut.Split(new char[] { '\n' });
                    for (int i = 1; i < rows.Length; i++)
                    {
                        if (rows[i].StartsWith("-> done"))
                        {
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i - 1], "success"));
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i], "success"));
                        }


                        if (rows[i].StartsWith("-> error"))
                        {
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i - 1], "danger"));
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i], "danger"));
                        }
                        if (rows[i].StartsWith("-> skipped"))
                        {
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i - 1], "warning"));
                            cucumberStdOut.Add(new KeyValuePair<string, string>(rows[i], "warning"));
                        }

                    }
                }
                else
                {
                    //set standard output
                    StdOutRows = Result.Output.StdOut.Split(new char[] { '\n' }).ToList();
                }
            }


            if (result.Output.ErrorInfo != null)
            {
                if (!string.IsNullOrEmpty(Result.Output.ErrorInfo.Message))
                {
                    //set MessageRows
                    ErrorMessageRows = Result.Output.ErrorInfo.Message.Split(new char[] { '\n' }).ToList();
                }

                if (!string.IsNullOrEmpty(Result.Output.ErrorInfo.StackTrace))
                {
                    //set StackTraceRows
                    ErrorStackTraceRows = Result.Output.ErrorInfo.StackTrace.Split(new char[] { '\n' }).ToList();
                }
            }

            ErrorImage = null;
        }


        public string TestId
        {
            get
            {
                var strings = ClassName.Split(new char[] { '.' }).ToList();
                strings.Add(Result.TestName);
                string id = "";
                foreach (var s in strings)
                    id += s;

                return id;
            }
        }

        private List<KeyValuePair<string, string>> cucumberStdOut;
        public List<KeyValuePair<string, string>> CucumberStdOut
        {
            get
            {
                return cucumberStdOut;
            }
        }

        public List<string> StdOutRows { get; set; }

        public List<string> ErrorMessageRows { get; set; }
        public List<string> ErrorStackTraceRows { get; set; }

        public string AsJson()
        {
            return System.Web.Helpers.Json.Encode(this);
        }
          
        public string FormattedStartTime
        { 
            get
            { 
                return DateTime.Parse(Result.StartTime).ToString("MM.dd.yyyy hh\\:mm\\:ss");
            }
        }

        public string FormattedEndTime
        {
            get
            {
                return DateTime.Parse(Result.EndTime).ToString("MM.dd.yyyy hh\\:mm\\:ss");
            }
        }

        public string FormattedDuration
        {
            get
            {
                return TimeSpan.Parse(Result.Duration).TotalSeconds.ToString("n2") + " sec.";
            }
        }

        public UnitTestResult Result { get; set; }
        public string Dll { get; set; }
        public string ClassName { get; set; }
        public string ErrorImage { get; set; }
    }
}
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRX_Merger.ReportModel;
using TRX_Merger.TrxModel;
using TRX_Merger.Utilities;

namespace TRX_Merger.ReportGenerator
{
    public static class TrxReportGenerator
    {
        public static void GenerateReport(string trxFilePath, string outputFile, string screenshotLocation, string reportTitle)
        {
            var testRun = TRXSerializationUtils.DeserializeTRX(trxFilePath);

            GenerateReport(testRun, outputFile, screenshotLocation, reportTitle);
        }

        public static void GenerateReport(TestRun run, string outputFile, string screenshotLocation, string reportTitle)
        {
            if (!string.IsNullOrEmpty(reportTitle))
                run.Name = reportTitle;

            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
            string template = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReportGenerator/trx_report_template.cshtml"));

            string result = Engine.Razor.RunCompile(
                template,
                "rawTemplate",
                null,
                new TestRunReport(run));

            //TODO: Implement screenshot logic here!

            if (File.Exists(outputFile))
            {
                Console.WriteLine("Deleting: " + outputFile);
                File.Delete(outputFile);
            }

            File.WriteAllText(outputFile, result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRX_Merger
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if(args.Length == 0
                || args.Contains("/h")
                || args.Contains("/help"))
            {
                DispalyHelp();
                return 1;
            }

            if(args.Where(a => a.StartsWith("/trx")).FirstOrDefault() == null)
            {
                Console.WriteLine("/trx parameter is required");
                return 1;
            }

            if (args.Where(a => a.StartsWith("/output")).FirstOrDefault() == null)
            {
                Console.WriteLine("/output parameter is required");
                return 1;
            }


            string trxArg = args.Where(a => a.StartsWith("/trx")).FirstOrDefault();
            string outputArg = args.Where(a => a.StartsWith("/output")).FirstOrDefault();
             
            string outputParam = ResolveOutputFileName(outputArg);
            if(outputParam.StartsWith("Error: "))
            {
                Console.WriteLine(outputParam);
                return 1;
            }

            var trxFiles = ResolveTrxFilePaths(trxArg, args.Contains("/r"));
            if (trxFiles.Count == 0)
            {
                Console.WriteLine("No trx files found!");
                return 1;
            }
            if (trxFiles.Count == 1)
            {
                if (trxFiles[0].StartsWith("Error: "))
                    Console.WriteLine(trxFiles[0]);
                else
                    Console.WriteLine("Error: Only one trx file has been passed. Please specify at least two trx files to be merged");

                return 1;
            }

            try
            {
                TestRunMerger.MergeTRXsAndSave(trxFiles, outputParam);
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                Console.WriteLine("Error: " + ex.Message);
                return 1;
            }
           
   
            return 0;
        }

        private static void DispalyHelp()
        {
            Console.WriteLine(
            @"
REQUIRED PARAMETERS:

/trx - parameter that determines which trx files will be merged.
	This parameter will accept one of the following:
		- file(s) name: looks for trx files in the current directory.File extension is required 
			example: /trx:testResults1.trx,testResults2.trx,testResults3.trx
		- file(s) path: full path to trx files.File extension is required 
			example: /trx:c:\TestResults\testResults1.trx,c:\TestResults\testResults2.trx,c:\TestResults\testResults3.trx
		- directory(s): directory containing trx files. it gets all trx files in the directory	
			example: /trx:c:\TestResults,c:\TestResults1
		- empty: gets all trx files in the current directory
			example: /trx
        - combination: you can pass files and directories at the same time:
            example: /trx:c:\TestResults,c:\TestResults1\testResults2.trx

/output - the name of the output trx file. File extension is required
	- name: saves the file in the current directory
		example: /output:combinedTestResults.trx
	- path and name: saves the file in specified directory.
		example: /output:c:\TestResults\combinedTestResults.trx

OPTIONAL PARAMETERS:

/r - recursive search in directories. When there is a directory in /trx param (ex: /trx:c:\TestResuts), and this parameter is passed, the rearch for trx files will be recursive
    example: /trx:c:\TestResults,c:\TestResults1\testResults2.trx /r /output:combinedTestResults.trx
            ");
        }

        private static string ResolveOutputFileName(string outputParam)
        { 
            var splitOutput = outputParam.Split(new char[] { ':' });

            if (splitOutput.Length == 1
                || !outputParam.EndsWith(".trx"))
                return "Error: /output parameter is in the correct format. Expected /output:<file name | directory and file name>. Execute /help for more information";

            return splitOutput[1]; 
        }

        private static List<string> ResolveTrxFilePaths(string trxParams, bool recursive)
        {
            List<string> paths = new List<string>();

            var splitTrx = trxParams.Split(new char[] { ':' });
             
            var searchOpts = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            if (splitTrx.Length == 1)
                return Directory.GetFiles(Directory.GetCurrentDirectory(), "*.trx", searchOpts).ToList();

            var args = trxParams.Substring(5,trxParams.Length - 5).Split(new char[] { ',' }).ToList(); 

            foreach (var a in args)
            {
                bool isTrxFile = File.Exists(a) && a.EndsWith(".trx");
                bool isDir = Directory.Exists(a);
                    
                if(!isTrxFile && !isDir)
                    return new List<string>
                    {
                        string.Format("Error: {0} is not a trx file or directory", a)
                    };

                if (isTrxFile)
                    paths.Add(a);

                if (isDir)
                    paths.AddRange(Directory.GetFiles(a, "*.trx", searchOpts).ToList());
                 
            }

            return paths;
        }
    }
}

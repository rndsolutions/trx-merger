# Welcome to the TRX-Merger

TRX-Merger is a command line tool that allows you to combine multiple trx files in a single trx file containing all the information from the trx files passed to it.

### Example:

You can start the Command-Line navigate to the folder where the TRX_Merger.exe is located and execute the following command:

>TRX_Merger.exe /trx:[trx file or directory 1],[trx file or directory 2],..,[trx file or directory N] /output:[output file path and name]

![](https://cloud.githubusercontent.com/assets/11598270/9352882/36e28792-466d-11e5-9f31-49c88978848a.png)



### REQUIRED PARAMETERS:

#### /trx - parameter that determines which trx files will be merged.

This parameter will accept one of the following:
- **file(s) name:** looks for trx files in the current directory.File extension is required 
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /trx:testResults1.trx,testResults2.trx,testResults3.trx
- **file(s) path:** full path to trx files.File extension is required 
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /trx:c:\TestResults\testResults1.trx,c:\TestResults\testResults2.trx,c:\TestResults\testResults3.trx
- **directory(s):** directory containing trx files. it gets all trx files in the directory
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /trx:c:\TestResults,c:\TestResults1
- **empty:** gets all trx files in the current directory
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /trx
- **combination:** you can pass files and directories at the same time:
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /trx:c:\TestResults,c:\TestResults1\testResults2.trx

#### /output - the name of the output trx file. File extension is required
- **name:** saves the file in the current directory
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /output:combinedTestResults.trx
- **path and name:** saves the file in specified directory.
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /output:c:\TestResults\combinedTestResults.trx

### OPTIONAL PARAMETERS:

#### /report - generates a html report from a trx file. REQUIRED if one trx is specified in /trx parameter and OPTIONAL otherwise.
- If one trx is passed to the utility, the report is for it, otherwise, the report is generated for the /output result. 
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /report:c:\Tests\report.html

#### /r - recursive search in directories. 
- When there is a directory in /trx param (ex: /trx:c:\TestResuts), and this parameter is passed, the rearch for trx files will be recursive
<br/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**example:** /trx:c:\TestResults,c:\TestResults1\testResults2.trx **/r** /output:combinedTestResults.trx

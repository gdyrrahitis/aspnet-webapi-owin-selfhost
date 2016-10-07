REM Create a 'GeneratedReports' folder if it does not exist
if not exist "%~dp0GeneratedReports" mkdir "%~dp0GeneratedReports"
 
REM Remove any previous test execution files to prevent issues overwriting
IF EXIST "%~dp0People.trx" del "%~dp0People.trx%"
 
REM Remove any previously created test output directories
CD %~dp0
FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"
 
REM Run the tests against the targeted output
call :RunOpenCoverUnitTestMetrics
 
REM Generate the report output based on the test results
if %errorlevel% equ 0 (
 call :RunReportGeneratorOutput
)
 
REM Launch the report
if %errorlevel% equ 0 (
 call :RunLaunchReport
)
@pause
exit /b %errorlevel%
 
:RunOpenCoverUnitTestMetrics
"%~dp0.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" ^
-register:user ^
-target:"C:\Program Files (x86)\NUnit.org\nunit-console\nunit3-console.exe" ^
-targetargs:"/testcontainer:\"%~dp0People.SelfHostedApi.Tests\bin\Debug\People.SelfHostedApi.Tests.dll\" /resultsfile:\"%~dp0People.trx\"" ^
-mergebyhash ^
-skipautoprops ^
-output:"%~dp0GeneratedReports\PeopleReport.xml"
@pause
exit /b %errorlevel%
 
:RunReportGeneratorOutput
"%~dp0packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe" ^
-reports:"%~dp0GeneratedReports\PeopleReport.xml" ^
-targetdir:"%~dp0GeneratedReports\ReportGenerator Output"
@pause
exit /b %errorlevel%
 
:RunLaunchReport
start "report" "%~dp0\GeneratedReports\ReportGenerator Output\index.htm"
@pause
exit /b %errorlevel%
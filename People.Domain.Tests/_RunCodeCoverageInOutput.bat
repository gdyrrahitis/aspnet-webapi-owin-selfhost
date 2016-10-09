"..\..\..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -target:"..\..\..\packages\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe" -targetargs:"People.Domain.Tests.dll" -filter:"+[People.Domain]People.Domain* -[*]People.Domain.Migrations* -[*]People.Domain.Context.BaseDbContext* -[*]People.Domain.Entities*" -excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" -register:user -output:"_CodeCoverageResult.xml"
@pause

"..\..\..\packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe" "-reports:_CodeCoverageResult.xml" "-targetdir:_CodeCoverageReport"
@pause

:RunLaunchReport
start "report" "_CodeCoverageReport\index.htm"
exit /b %errorlevel%
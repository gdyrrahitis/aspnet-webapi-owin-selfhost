"..\..\..\packages\OpenCover.4.6.519\OpenCover.Console.exe" 
-target:"..\..\..\packages\NUnit.Runners.3.4.1\tools\nunit-console.exe" 
-targetargs:"/nologo People.SelfHostedApi.Tests.dll /noshadow" -filter:"+[People.SelfHostedApi]People.SelfHostedApi*" 
-excludebyattribute:"System.CodeDom.Compiler.GeneratedCodeAttribute" 
-register:user -output:"_CodeCoverageResult.xml"

"..\..\..\packages\ReportGenerator.2.4.5.0\ReportGenerator.exe" "-reports:_CodeCoverageResult.xml" "-targetdir:_CodeCoverageReport"
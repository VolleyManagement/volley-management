
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var srcPath        = "./../";
var srcDir         = Directory(srcPath);
var buildDir       = srcDir + Directory("bin/") + Directory(configuration);
var webBuildDir    = srcDir + Directory("VolleyManagement.UI/bin");
var testsDir       = srcDir + Directory("bin/UnitTests/") + Directory(configuration);
var testResultsDir = srcDir + Directory("TestResults/");

// Define files
var slnPath = srcPath + "VolleyManagement.sln";

string testResultsFile;
if (AppVeyor.IsRunningOnAppVeyor)
{
    testResultsFile = testResultsDir.Path.FullPath 
                + string.Format("TestResults_AppVeyor_{0}.trx", EnvironmentVariable("APPVEYOR_JOB_ID"));
}
else
{
    testResultsFile = testResultsDir.Path.FullPath + "TestResults.trx";
}

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
    {
        CleanDirectory(buildDir);
        CleanDirectory(webBuildDir);
        CleanDirectory(testsDir);
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        NuGetRestore(slnPath);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(()=>
    {
        MSBuild(slnPath, configurator =>
            configurator.SetConfiguration(configuration)
        );
    });

Task("UnitTests")
    .IsDependentOn("Build")
    .Does(()=>
    {
        MSTest(
            testsDir.Path.FullPath + "/*.UnitTests.dll",
            new MSTestSettings{
                ResultsFile = testResultsFile,
                WorkingDirectory = testsDir
            });

        if (AppVeyor.IsRunningOnAppVeyor)
        {
            AppVeyor.UploadTestResults(testResultsFile, AppVeyorTestResultsType.MSTest);
        }
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("UnitTests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
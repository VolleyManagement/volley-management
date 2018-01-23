#tool nuget:?package=MSBuild.SonarQube.Runner.Tool

#addin "Cake.Incubator"
#addin "Cake.Sonar"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var sonarToken = HasArgument("sonar-token") 
    ? Argument<string>("sonar-token") 
    : EnvironmentVariable("SONAR_TOKEN");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var srcPath        = "./../";
var srcDir         = Directory(srcPath);
var buildDir       = srcDir + Directory("bin/") + Directory(configuration);
var webBuildDir    = srcDir + Directory("VolleyManagement.UI/bin");
var testsDir       = srcDir + Directory("bin/UnitTests/") + Directory(configuration);

// Define files
var slnPath = srcPath + "VolleyManagement.sln";

ConvertableFilePath testResultsFile;
if (BuildSystem.IsRunningOnAppVeyor)
{
    testResultsFile = testsDir 
                + File(string.Format("TestResults_AppVeyor_{0}.trx", AppVeyor.Environment.JobId));
}
else
{
    testResultsFile = testsDir + File("TestResults.trx");
}

// Variables
var canRunSonar = sonarToken != null;
SonarEndSettings sonarEndSettings;

//////////////////////////////////////////////////////////////////////
// TASKS
// Are atomic and small. Next region is responsible for setting order
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() => {
        CleanDirectory(buildDir);
        CleanDirectory(webBuildDir);
        CleanDirectory(testsDir);
    });

Task("Restore-NuGet-Packages")
    .Does(() => {
        NuGetRestore(slnPath);
    });

Task("Build")
    .Does(() => {
        MSBuild(slnPath, configurator =>
            configurator.SetConfiguration(configuration)
        );
    });

Task("UnitTests")
    .Does(() => {
        MSTest(
            testsDir.Path.FullPath + "/*.UnitTests.dll",
            new MSTestSettings{
                ResultsFile = testResultsFile.Path.GetFilename().FullPath,
                WorkingDirectory = testsDir
            });

        if (BuildSystem.IsRunningOnAppVeyor)
        {
            AppVeyor.UploadTestResults(testResultsFile, AppVeyorTestResultsType.MSTest);
        }
    });

Task("SonarBegin")
    .WithCriteria(() => canRunSonar)
    .Does(() => {
        var settings = new SonarBeginSettings{
            Url = "https://sonarcloud.io",
            Key = "volley-management",
            Organization = "volleymanagement",
            Login = sonarToken,
            VsTestReportsPath = testResultsFile
        };

        if (BuildSystem.IsRunningOnAppVeyor
            && AppVeyor.Environment.PullRequest.IsPullRequest)
        {
            settings.ArgumentCustomization = 
                args => args.Append("/d:\"sonar.analysis.mode=preview\"")
                            .Append($"/d:\"sonar.github.pullRequest={AppVeyor.Environment.PullRequest.Number}\"")
                            .Append("/d:\"sonar.github.repository=YourRepositoryUrl\"")
                            .AppendSecret($"/d:\"sonar.github.oauth={EnvironmentVariable("GITHUB_SONAR_PR_TOKEN")}\"");
        }

        sonarEndSettings = settings.GetEndSettings();
        SonarBegin(settings);
    });

Task("SonarEnd")
    .WithCriteria(() => canRunSonar)
    .Does(() => {
        SonarEnd(sonarEndSettings);
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Sonar")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Build")
    .IsDependentOn("UnitTests")
    .IsDependentOn("SonarEnd");

Task("Default")
    .IsDependentOn("Sonar");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
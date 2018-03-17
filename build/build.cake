#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#tool nuget:?package=JetBrains.dotCover.CommandLineTools

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
var localDev = Argument<bool>("local-dev", false);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var rootPath       = "./../";
var rootDir        = Directory(rootPath);
var buildDir       = rootDir + Directory("bin/") + Directory(configuration);
var webBuildDir    = rootDir + Directory("src/VolleyManagement.Backend/VolleyManagement.UI/bin");
var testsDir       = rootDir + Directory("bin/UnitTests/") + Directory(configuration);

// Define files
var slnPath = rootPath + "src/VolleyManagement.sln";

ConvertableFilePath testResultsFile;
ConvertableFilePath codeCoverageResultsFile;
if (BuildSystem.IsRunningOnAppVeyor)
{
    testResultsFile = testsDir 
                + File($"TestResults_AppVeyor_{AppVeyor.Environment.JobId}.trx");
    codeCoverageResultsFile = testsDir
        + File($"CodeCoverageResults_AppVeyor_{AppVeyor.Environment.JobId}.html");
}
else
{
    testResultsFile = testsDir + File("TestResults.trx");
    codeCoverageResultsFile = testsDir + File("CodeCoverageResults.html");
}

// Variables
var isCiForMasterOrPr = BuildSystem.IsRunningOnAppVeyor 
                   && (AppVeyor.Environment.Repository.Branch == "master" // master branch
                    || AppVeyor.Environment.PullRequest.IsPullRequest);
var canRunSonar =   sonarToken != null //Has Sonar token
                && (isCiForMasterOrPr || localDev);
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
        var testsPath = testsDir.Path.FullPath + "/*.UnitTests.dll";
        var msTestSettings = new MSTestSettings {
            ResultsFile = testResultsFile.Path.GetFilename().FullPath,
            WorkingDirectory = testsDir
        };

        var dotCoverSettings = new DotCoverAnalyseSettings {
            ReportType = DotCoverReportType.HTML,
            WorkingDirectory = testsDir,
            TargetWorkingDir = testsDir
        };
        
        DotCoverAnalyse(
            (ICakeContext c) => { c.MSTest(testsPath, msTestSettings); },
            codeCoverageResultsFile,
            dotCoverSettings);        

        if (BuildSystem.IsRunningOnAppVeyor)
        {
            AppVeyor.UploadTestResults(testResultsFile, AppVeyorTestResultsType.MSTest);
        }
    });

Task("SonarBegin")
    .WithCriteria(() => canRunSonar)
    .Does(() => {
        var settings = new SonarBeginSettings {
            Url = "https://sonarcloud.io",
            Key = "volley-management",
            Organization = "volleymanagement",
            Login = sonarToken,
            VsTestReportsPath = testResultsFile,
            DotCoverReportsPath = codeCoverageResultsFile
        };

        if (BuildSystem.IsRunningOnAppVeyor
            && AppVeyor.Environment.PullRequest.IsPullRequest)
        {
            settings.Version = AppVeyor.Environment.Build.Version;
            settings.ArgumentCustomization = 
                args => args.Append("/d:\"sonar.analysis.mode=preview\"")
                            .Append($"/d:\"sonar.github.pullRequest={AppVeyor.Environment.PullRequest.Number}\"")
                            .Append("/d:\"sonar.github.repository=VolleyManagement/volley-management\"")
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
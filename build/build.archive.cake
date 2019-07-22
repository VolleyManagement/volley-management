﻿#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#tool nuget:?package=JetBrains.dotCover.CommandLineTools
#tool "nuget:?package=xunit.runner.console"

#addin "Cake.Incubator"
#addin "Cake.Sonar"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var sonarToken = HasArgument("sonar-token") ?
    Argument<string>("sonar-token") :
    EnvironmentVariable("SONAR_TOKEN");
var localDev = Argument<bool>("local-dev", false);

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var rootPath    = "./../";
var rootDir     = Directory(rootPath);
var buildDir    = rootDir + Directory("bin/") + Directory(configuration);
var testsDir    = rootDir + Directory("tests/bin/");
var webBuildDir = rootDir + Directory("src/VolleyManagement.Backend/VolleyManagement.UI/bin");
var utsDir      = testsDir + Directory("UnitTests/") + Directory(configuration);
var specsDir    = testsDir + Directory("Specs/") + Directory(configuration);
var domainDir   = testsDir + Directory("DomainTests/") + Directory(configuration);

// Define files
var slnPath = rootPath + "src/VolleyManagement.sln";

ConvertableFilePath utResults;
ConvertableFilePath specResults;
ConvertableFilePath domainUTResults;

ConvertableFilePath utCoverageResults;
ConvertableFilePath specCoverageResults;
ConvertableFilePath domainUTCoverageResults;

ConvertableFilePath combinedCoverageResults;

// Variables
var isCiForMasterOrPr = BuildSystem.IsRunningOnAppVeyor &&
    (AppVeyor.Environment.Repository.Branch == "master" // master branch
        ||
        AppVeyor.Environment.PullRequest.IsPullRequest);
var canRunSonar = sonarToken != null //Has Sonar token
    && (isCiForMasterOrPr || localDev);
var canRunIntegrationTests = localDev || isCiForMasterOrPr;
SonarEndSettings sonarEndSettings;

Information($"Sonar token exists: {sonarToken != null}");
Information($"BuildSystem.IsRunningOnAppVeyor : {BuildSystem.IsRunningOnAppVeyor}");
Information($"AppVeyor.Environment.Repository.Branch: {AppVeyor.Environment.Repository.Branch}");
Information($"AppVeyor.Environment.PullRequest.IsPullRequest: {AppVeyor.Environment.PullRequest.IsPullRequest}");
Information($"canRunSonar: {canRunSonar}");

var suffix = BuildSystem.IsRunningOnAppVeyor ? $"_AppVeyor_{AppVeyor.Environment.JobId}" : string.Empty;

utResults = utsDir + File($"UT_Results{suffix}.xml");
utCoverageResults = utsDir + File($"UT_Coverage{suffix}.dcvr");

specResults = specsDir + File($"IT_Results{suffix}.xml");
specCoverageResults = specsDir + File($"IT_Coverage{suffix}.dcvr");

domainUTResults = domainDir + File($"DomainUT_Results{suffix}.xml");
domainUTCoverageResults = domainDir + File($"DomainUT_Coverage{suffix}.dcvr");

combinedCoverageResults = testsDir + File($"CoverageResults{suffix}.html");

var coverageResultsToMerge = new List<FilePath>();
coverageResultsToMerge.Add(utCoverageResults);

var utResultsToMerge = new List<FilePath>();
utResultsToMerge.Add(utResults);
utResultsToMerge.Add(domainUTResults);


if(canRunIntegrationTests)
{
    coverageResultsToMerge.Add(specCoverageResults);
    utResultsToMerge.Add(specResults);
}

//////////////////////////////////////////////////////////////////////
// TASKS
// Are atomic and small. Next region is responsible for setting order
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does (() => {
        CleanDirectory(buildDir);
        CleanDirectory(webBuildDir);
        CleanDirectory(testsDir);   
    });

Task("Restore-NuGet-Packages")
    .Does (() => {
        NuGetRestore (slnPath);
    });

Task("Build")
    .Does(() => {
        MSBuild (slnPath, configurator =>
            configurator.SetConfiguration (configuration)
        );
    });

Task("UnitTests")
    .Does(() => {
        var testsPath = utsDir.Path.FullPath + "/*/*.UnitTests.dll";

        var xUnitSettings = new XUnit2Settings {
            WorkingDirectory = testsDir,
            ReportName = utResults.Path.GetFilenameWithoutExtension().FullPath,
            XmlReport = true,
            OutputDirectory = utResults.Path.GetDirectory().FullPath
        };

        var dotCoverSettings = new DotCoverCoverSettings{
                WorkingDirectory = utsDir,
                TargetWorkingDir = utsDir
            };

        SetCoverageFilter(dotCoverSettings);        

        DotCoverCover(
            (ICakeContext c) => { c.XUnit2 (testsPath, xUnitSettings); },
            utCoverageResults,
            dotCoverSettings
        );

        if (BuildSystem.IsRunningOnAppVeyor) {
            AppVeyor.UploadTestResults(utResults, AppVeyorTestResultsType.XUnit);    
            utResultsToMerge.Append(utResults);
            utResultsToMerge.Append(",");
        }
    });

Task("IntegrationTests")
    .WithCriteria(() => canRunIntegrationTests)
    .Does(() => {        
        var testsPath = specsDir.Path.FullPath + "/*/*.Specs.dll";
        var xUnitSettings = new XUnit2Settings {
            WorkingDirectory = specsDir,
            ReportName = specResults.Path.GetFilenameWithoutExtension().FullPath,
            XmlReport = true,
            OutputDirectory = specResults.Path.GetDirectory().FullPath
        };

        var dotCoverSettings = new DotCoverCoverSettings {
            WorkingDirectory = specsDir,
            TargetWorkingDir = specsDir
        };

        dotCoverSettings.WithProcessFilter("-:sqlservr.exe");
        SetCoverageFilter(dotCoverSettings);        

        DotCoverCover (
            (ICakeContext c) => { c.XUnit2(testsPath, xUnitSettings); },
            specCoverageResults,
            dotCoverSettings);

        if (BuildSystem.IsRunningOnAppVeyor) {
            AppVeyor.UploadTestResults(specResults, AppVeyorTestResultsType.XUnit);
            utResultsToMerge.Append(specResults);
            utResultsToMerge.Append(",");
        }
    });

Task("GenerateCoverageReport")
    .Does(() => {
        var combinedSnapshotPath = combinedCoverageResults.Path.GetDirectory().CombineWithFilePath(File("Coverage.dcvr"));
        DotCoverMerge(coverageResultsToMerge, combinedSnapshotPath, new DotCoverMergeSettings());        

        DotCoverReport(combinedSnapshotPath, combinedCoverageResults, new DotCoverReportSettings{
            ReportType = DotCoverReportType.HTML
        });
    });

Task("SonarBegin")
    .WithCriteria(() => canRunSonar)
    .Does(() => {
        var settings = new SonarBeginSettings {
            Url = "https://sonarcloud.io",
            Key = "volley-management",
            Organization = "volleymanagement",
            Login = sonarToken,
            XUnitReportsPath = string.Join(",", utResultsToMerge),
            DotCoverReportsPath = combinedCoverageResults,
            Exclusions = "src/VolleyManagement.WebClient/**"
        };

        if (BuildSystem.IsRunningOnAppVeyor &&
            AppVeyor.Environment.PullRequest.IsPullRequest) {
            settings.Version = AppVeyor.Environment.Build.Version;
            settings.ArgumentCustomization =
                args => args.Append($"/d:\"sonar.pullrequest.key={AppVeyor.Environment.PullRequest.Number}\"")
                .Append($"/d:\"sonar.pullrequest.branch={AppVeyor.Environment.PullRequest.Title}\"")
                .Append($"/d:\"sonar.pullrequest.base={AppVeyor.Environment.Repository.Branch}\"");
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
    .IsDependentOn("IntegrationTests")
    .IsDependentOn("GenerateCoverageReport")
    .IsDependentOn("SonarEnd");

Task("Default")
    .IsDependentOn("Sonar");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

//////////////////////////////////////////////////////////////////////
// Helper Methods
//////////////////////////////////////////////////////////////////////

public static void SetCoverageFilter(DotCoverCoverSettings settings)
{
    settings.WithFilter("+:VolleyManagement*");
    settings.WithFilter("-:*.UnitTests");
    settings.WithFilter("-:*.Specs");
    settings.WithFilter("-:*.Domain.UnitTests");
}
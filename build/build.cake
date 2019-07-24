//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var rootPath        = "./../";
var rootDir         = Directory(rootPath);
var artifcatsDir    = rootDir + Directory("artifacts/") + Directory(configuration);
var buildDir        = artifcatsDir + Directory("build/");

// Define files
var sourcePath = rootPath + "src/";

Information($"BuildSystem.IsRunningOnAppVeyor : {BuildSystem.IsRunningOnAppVeyor}");
Information($"AppVeyor.Environment.Repository.Branch: {AppVeyor.Environment.Repository.Branch}");
Information($"AppVeyor.Environment.PullRequest.IsPullRequest: {AppVeyor.Environment.PullRequest.IsPullRequest}");

//////////////////////////////////////////////////////////////////////
// TASKS
// Are atomic and small. Next region is responsible for setting order
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does (() => {
        CleanDirectory(artifcatsDir);
    });

Task("Build")
    .Does(() =>
{
    var settings = new DotNetCoreBuildSettings
     {
         Framework = "netcoreapp3.0",
         Configuration = configuration,
         OutputDirectory = buildDir,
         NoIncremental = true
     };

     DotNetCoreBuild(sourcePath, settings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

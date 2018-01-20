
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var srcPath     = "./../";
var srcDir      = Directory(srcPath);
var buildDir    = srcDir + Directory("bin/") + Directory(configuration);
var webBuildDir = srcDir + Directory("VolleyManagement.UI/bin");
var testsDir    = srcDir + Directory("bin/UnitTests/") + Directory(configuration);

// Define files
var slnPath = srcPath + "VolleyManagement.sln";

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
        // There is a known issue with Cake+MSTest+VS 2017. One more point to move to xUnit :)
        MSTest(testsDir.Path.FullPath + "/*.UnitTests.dll");
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("IIS")
    .IsDependentOn("UnitTests");

Task("Default")
    .IsDependentOn("IIS");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
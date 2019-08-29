using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using FluentAssertions;
using Microsoft.Build.Construction;
using NetArchTest.Rules;
using NetArchTest.Rules.Policies;
using Xunit;

namespace VolleyM.Architecture.UnitTests
{
    public class PackageStructureTests
    {
        [Fact]
        public void Test1()
        {
            var architecturePolicy = Policy.Define("Package Naming",
                "All assemblies should be named according to the guidelines: " +
                            "https://github.com/VolleyManagement/volley-management/wiki/Solution-Architecture#volleymanagementapi")
                //.For(Types.)
                ;
            var slnPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../src/VolleyManagement.sln");
            var file = new FileInfo(slnPath);

            var sln = SolutionFile.Parse(file.FullName);

            var projects = sln.ProjectsInOrder.Where(p => p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat).ToList();
            projects.Should().HaveCount(8);
        }

        [Fact]
        public void AllAssembliesAddedToTheArchitectureTests()
        {
            var assemblies = TypesFixture.AllAssemblyNames();
            var failedAssemblies = new List<string>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assembly));
                }
                catch (Exception)
                {
                    failedAssemblies.Add(assembly);
                }
            }

            failedAssemblies.Should().BeEmpty("all projects should be referenced by Architecture.UnitTests");
        }
    }
}

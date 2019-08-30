using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
        private const string ROOT_NS = "VolleyM";

        private const string DOMAIN_NS = "Domain";

        private static readonly string[] AllowedLayers = {
            DOMAIN_NS,
            "Infrastructure",
            "API"
        };

        private static readonly string[] BoundedContexts = {
            "Contributors",
            "Teams",
            "Players",
            "Tournaments",
            "TournamentCalendar",
            //Not context but allowed
            "Contracts"
        };

        [Fact]
        public void AllProjectsFollowPackageNaming()
        {
            var architecturePolicy = Policy.Define("Package Naming",
                "All assemblies should be named according to the guidelines: " +
                            "https://github.com/VolleyManagement/volley-management/wiki/Solution-Architecture#volleymanagementapi")
                .For(TypesFixture.AllProjectTypes)
                .Add(t =>
                    t.ThatNotCompilerGenerated()
                    .Should()
                        .ResideInNamespace(ROOT_NS))
                .Add(t =>
                    t.ThatNotCompilerGenerated()
                    .Should()
                        .ResideInAllowedNamespace($"{ROOT_NS}", AllowedLayers),
                    "Allowed Layers", string.Empty)
                .Add(t =>
                    t.ThatNotCompilerGenerated()
                        .And().ResideInNamespace($"{ROOT_NS}.{DOMAIN_NS}")
                        .Should()
                        .ResideInAllowedNamespace($"{ROOT_NS}.{DOMAIN_NS}", BoundedContexts),
                    "Allowed Domains", string.Empty);

            var result = architecturePolicy.Evaluate();
            result.AssertHasNoViolations();
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

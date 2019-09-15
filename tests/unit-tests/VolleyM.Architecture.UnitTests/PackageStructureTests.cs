using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using FluentAssertions;
using NetArchTest.Rules.Policies;
using Xunit;

namespace VolleyM.Architecture.UnitTests
{
    public class PackageStructureTests
    {
        private static readonly string FULL_DOMAIN_NS = $"{PackageNamingConstants.ROOT_NS}.{PackageNamingConstants.DOMAIN_NS}";

        [Fact(DisplayName = nameof(AllProjectsUseAllowedLayers))]
        public void AllProjectsUseAllowedLayers()
        {
            var architecturePolicy = Policy.Define("Package Naming",
                "All assemblies should be named according to the guidelines: " +
                            "https://github.com/VolleyManagement/volley-management/wiki/Solution-Architecture#volleymanagementapi")
                .For(TypesFixture.AllProjectTypes)
                .Add(t =>
                    t.ThatNotCompilerGenerated()
                    .Should()
                        .ResideInNamespace(PackageNamingConstants.ROOT_NS))
                .Add(t =>
                    t.ThatNotCompilerGenerated()
                    .Should()
                        .ResideInAllowedNamespace($"{PackageNamingConstants.ROOT_NS}", PackageNamingConstants.AllowedLayers),
                    "Allowed Layers", string.Empty);

            var result = architecturePolicy.Evaluate();
            result.AssertHasNoViolations();
        }

        [Fact(DisplayName = nameof(DomainProjectsUseAllowedContexts))]
        public void DomainProjectsUseAllowedContexts()
        {
            var domainAssemblies = AssembliesFixture.GetDomainAssemblies();

            var allowedVmAssemblies = new[]
            {
                $"{FULL_DOMAIN_NS}.Contracts",
                $"{PackageNamingConstants.ROOT_NS}.Infrastructure.Bootstrap"
            };

            foreach (var domainAssembly in domainAssemblies)
            {
                domainAssembly.AssertContextNameIsAllowed(PackageNamingConstants.BoundedContexts);
                var references = domainAssembly.GetReferencedAssemblies();

                var filtered = references
                    .Where(NotSystemDependency)
                    .Where(NotMicrosoftDependency)
                    .Where(p => NotDependency(p, PackageNamingConstants.SIMPLE_INJECTOR_NS))
                    .Where(p => NotDependency(p, PackageNamingConstants.AUTOMAPPER_NS))
                    .Where(p => NotDependency(p, PackageNamingConstants.SERILOG_NS))
                    .Where(p => NotDependency(p, $"{PackageNamingConstants.ROOT_NS}.Infrastructure.Bootstrap"))
                    .Where(a => NotDependency(a, allowedVmAssemblies));

                filtered.Should().BeEmpty("{0} assembly should reference only allowed assemblies",
                    domainAssembly.GetName().Name);
            }
        }

        [Fact(DisplayName = nameof(ApiProjectsUseAllowedContexts))]
        public void ApiProjectsUseAllowedContexts()
        {
            var apiAssemblies = AssembliesFixture.GetApiAssemblies();

            foreach (var domainAssembly in apiAssemblies)
            {
                domainAssembly.AssertContextNameIsAllowed(PackageNamingConstants.BoundedContexts);
            }
        }

        [Fact(DisplayName = nameof(AllAssembliesAddedToTheArchitectureTests))]
        public void AllAssembliesAddedToTheArchitectureTests()
        {
            var assemblies = AssembliesFixture.AllAssemblyNames;
            var missingAssemblies = new List<string>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assembly));
                }
                catch (Exception)
                {
                    missingAssemblies.Add(assembly);
                }
            }

            missingAssemblies.Should().BeEmpty("all projects should be referenced by Architecture.UnitTests");
        }

        private static bool NotSystemDependency(AssemblyName assembly)
            => !StartsWith(assembly, PackageNamingConstants.SYSTEM_NS);
        private static bool NotMicrosoftDependency(AssemblyName assembly)
            => NotDependency(assembly, PackageNamingConstants.AllowedMicrosoftReferences);
        private static bool NotDependency(AssemblyName assembly, string packageName)
            => !StartsWith(assembly, packageName);

        private static bool NotDependency(AssemblyName assembly, IEnumerable<string> allowed)
            => !allowed.Any(dep => StartsWith(assembly, dep));
        private static bool StartsWith(AssemblyName assembly, string packageName)
                => assembly.Name.StartsWith(packageName, StringComparison.OrdinalIgnoreCase);
    }
}

using Microsoft.Build.Construction;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;

namespace VolleyM.Architecture.UnitTests
{
    internal class AssembliesFixture
    {
        private static readonly AssembliesFixture Instance = new AssembliesFixture();

        private Lazy<ImmutableList<string>> AllAssemblyNamesLazy { get; }
            = new Lazy<ImmutableList<string>>(GetAllAssemblyNames, LazyThreadSafetyMode.ExecutionAndPublication);

        private Lazy<ImmutableList<Assembly>> AllAssembliesLazy { get; }
            = new Lazy<ImmutableList<Assembly>>(GetAllAssemblies, LazyThreadSafetyMode.ExecutionAndPublication);

        internal static ImmutableList<string> AllAssemblyNames => Instance.AllAssemblyNamesLazy.Value;
        internal static ImmutableList<Assembly> AllAssemblies => Instance.AllAssembliesLazy.Value;

        internal static IEnumerable<Assembly> GetDomainAssemblies()
            => AllAssemblies.Where(a => a.FullName.StartsWith($"{PackageNamingConstants.ROOT_NS}.{PackageNamingConstants.DOMAIN_NS}"));
        internal static IEnumerable<Assembly> GetApiAssemblies()
            => AllAssemblies.Where(a => a.FullName.StartsWith($"{PackageNamingConstants.ROOT_NS}.{PackageNamingConstants.API_NS}"));

        internal static IEnumerable<Assembly> GetInfrastructureAssemblies()
            => AllAssemblies.Where(a => a.FullName.StartsWith($"{PackageNamingConstants.ROOT_NS}.{PackageNamingConstants.INFRASTRUCTURE_NS}"));

        private static ImmutableList<string> GetAllAssemblyNames()
        {
            var slnPath = Path.Combine(GetRepositoryRootPath(), "src/VolleyManagement.sln");
            var file = new FileInfo(slnPath);

            var sln = SolutionFile.Parse(file.FullName);

            var testsSolutionFolder = sln.ProjectsInOrder.Single(p =>
                p.ProjectType == SolutionProjectType.SolutionFolder &&
                string.Compare(p.ProjectName, "tests", StringComparison.OrdinalIgnoreCase) == 0);
            var projects = sln.ProjectsInOrder.Where(p =>
                p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat &&
                p.ParentProjectGuid != testsSolutionFolder.ProjectGuid).ToList();

            var instanceAllAssemblyNames = projects.Select(p => p.ProjectName).ToList();
            return instanceAllAssemblyNames.ToImmutableList();
        }

        private static ImmutableList<Assembly> GetAllAssemblies()
        {
            var assemblies = new List<Assembly>();

            foreach (var assemblyName in AllAssemblyNames)
            {
                assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName)));
            }

            return assemblies.ToImmutableList();
        }

        private static string GetRepositoryRootPath()
        {
            static bool IsRoot(DirectoryInfo di) => di.Parent == null;

            static bool HasGitFolder(DirectoryInfo di)
            {
                return di.EnumerateDirectories()
                    .Any(cd => cd.Name.Equals(".git", StringComparison.OrdinalIgnoreCase));
            }

            var repoDir = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (!HasGitFolder(repoDir) || IsRoot(repoDir))
            {
                repoDir = repoDir?.Parent;
            }

            return repoDir?.FullName;
        }
    }
}
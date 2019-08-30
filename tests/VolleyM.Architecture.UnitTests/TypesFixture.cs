using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using Microsoft.Build.Construction;
using NetArchTest.Rules;

namespace VolleyM.Architecture.UnitTests
{
    internal class TypesFixture
    {
        private static readonly TypesFixture Instance = new TypesFixture();

        private List<string> _allAssemblyNames { get; set; }
        private List<Assembly> _allAssemblies { get; set; }


        internal static Types AllProjectTypes()
        {
            if (Instance._allAssemblies == null)
            {
                var assemblies = new List<Assembly>();

                foreach (var assemblyName in AllAssemblyNames())
                {
                    assemblies.Add(AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName)));
                }

                Instance._allAssemblies = assemblies;
            }
            
            return Types.InAssemblies(Instance._allAssemblies);
        }

        internal static List<string> AllAssemblyNames()
        {
            if (Instance._allAssemblyNames == null)
            {

                var slnPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../src/VolleyManagement.sln");
                var file = new FileInfo(slnPath);

                var sln = SolutionFile.Parse(file.FullName);

                var testsSolutionFolder = sln.ProjectsInOrder.Single(p =>
                    p.ProjectType == SolutionProjectType.SolutionFolder &&
                    string.Compare(p.ProjectName, "tests", StringComparison.OrdinalIgnoreCase) == 0);
                var projects = sln.ProjectsInOrder.Where(p => p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat && p.ParentProjectGuid != testsSolutionFolder.ProjectGuid).ToList();

                Instance._allAssemblyNames = projects.Select(p => p.ProjectName).ToList();
            }

            return Instance._allAssemblyNames;
        }
    }
}
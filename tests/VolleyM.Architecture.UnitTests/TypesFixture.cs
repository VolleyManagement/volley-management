using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Construction;
using NetArchTest.Rules;

namespace VolleyM.Architecture.UnitTests
{
    internal static class TypesFixture
    {
        internal static Types AllProjectTypes()
        {
            return null;

        }

        internal static List<string> AllAssemblyNames()
        {
            var slnPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../../../src/VolleyManagement.sln");
            var file = new FileInfo(slnPath);

            var sln = SolutionFile.Parse(file.FullName);

            var testsSolutionFolder = sln.ProjectsInOrder.Single(p =>
                p.ProjectType == SolutionProjectType.SolutionFolder &&
                string.Compare(p.ProjectName, "tests", StringComparison.OrdinalIgnoreCase) == 0);
            var projects = sln.ProjectsInOrder.Where(p => p.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat && p.ParentProjectGuid != testsSolutionFolder.ProjectGuid).ToList();

            var result = projects.Select(p => p.ProjectName).ToList();

            return result;
        }
    }
}
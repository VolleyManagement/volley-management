﻿namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Services.ContributorService;
    using UI.Areas.Mvc.ViewModels.ContributorsTeam;

    /// <summary>
    /// Comparer for contributor team objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ContributorTeamViewModelComparer : IComparer<ContributorsTeamViewModel>, IComparer
    {
        /// <summary>
        /// Compares two contributor team objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of contributor team.</returns>
        public int Compare(ContributorsTeamViewModel x, ContributorsTeamViewModel y)
        {
            if (IsEqual(x, y))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// Compares two objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of objects.</returns>
        public int Compare(object x, object y)
        {
            var firstViewModel = x as ContributorsTeamViewModel;
            var secondViewModel = y as ContributorsTeamViewModel;

            if (firstViewModel == null)
            {
                return -1;
            }
            else if (secondViewModel == null)
            {
                return 1;
            }

            return Compare(firstViewModel, secondViewModel);
        }

        /// <summary>
        /// Finds out whether two contributor team objects have the same properties.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given contributor have the same properties.</returns>
        private bool IsEqual(ContributorsTeamViewModel x, ContributorsTeamViewModel y)
        {
            return x.Id == y.Id &&
               x.Name == y.Name &&
               x.CourseDirection == y.CourseDirection &&
               x.Contributors.SequenceEqual(y.Contributors, new ContributorEqualityComparer());
        }
    }
}

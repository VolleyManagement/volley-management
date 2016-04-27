namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using VolleyManagement.UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents a comparer for <see cref="StandingsViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class StandingsViewModelComparer : IComparer<StandingsViewModel>, IComparer
    {
        /// <summary>
        /// Compares two <see cref="StandingsViewModel"/> objects.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsViewModel"/> x and y.</returns>
        public int Compare(StandingsViewModel x, StandingsViewModel y)
        {
            return AreEqual(x, y) ? 0 : 1;
        }

        /// <summary>
        /// Compares two <see cref="StandingsViewModel"/> objects (non-generic implementation).
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of <see cref="StandingsViewModel"/> x and y.</returns>
        public int Compare(object x, object y)
        {
            StandingsViewModel firstStandingsViewModel = x as StandingsViewModel;
            StandingsViewModel secondStandingsViewModel = y as StandingsViewModel;

            if (firstStandingsViewModel == null)
            {
                return -1;
            }
            else if (secondStandingsViewModel == null)
            {
                return 1;
            }

            return Compare(firstStandingsViewModel, secondStandingsViewModel);
        }

        /// <summary>
        /// Finds out whether two <see cref="StandingsViewModel"/> objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if given <see cref="StandingsViewModel"/> objects are equal.</returns>
        internal bool AreEqual(StandingsViewModel x, StandingsViewModel y)
        {
            return x.TournamentId == y.TournamentId
                && x.TournamentName == y.TournamentName
                && x.Standings.SequenceEqual(y.Standings, new StandingsEntryViewModelEqualityComparer())
                && x.PivotTable.TeamsStandings.SequenceEqual(y.PivotTable.TeamsStandings, new PivotTeamStandingsViewModelEqualityComparer())
                && AreResultTablesEquals(x.PivotTable, y.PivotTable);
        }

        private bool AreResultTablesEquals(PivotTableViewModel tableExpected, PivotTableViewModel tableActual)
        {
            if (tableExpected.AllGameResults.Length != tableActual.AllGameResults.Length)
            {
                return false;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (tableExpected[i, j].Count == 0 && tableActual[i, j].Count == 0)
                    {
                        continue;
                    }

                    if (!tableExpected[i, j].SequenceEqual(tableActual[i, j], new PivotGameResultsViewModelEqualityComparer()))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

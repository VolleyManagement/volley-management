namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using FluentAssertions;
    using Xunit;
    using UI.Areas.Mvc.ViewModels.GameReports;

    /// <summary>
    /// Represents an equality comparer for <see cref="PivotTableViewModel"/> objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class PivotTableEqualityComparer
    {
        public static bool AreResultTablesEquals(List<PivotGameResultViewModel>[] expected, PivotTableViewModel actual, string messagePrefix = "")
        {
            if (expected != null || actual != null)
            {
                expected.Should().NotBeNull($"{messagePrefix} One of the results table is null");
                actual.Should().NotBeNull($"{messagePrefix} One of the results table is null");

                for (var i = 0; i < expected.Length; i++)
                {
                    var pos = GetPosition(i, expected.Length);
                    var actualCell = actual[pos.Row, pos.Col];

                    if (expected[i] != null || actualCell != null)
                    {
                        expected[i].Should().NotBeNull($"{messagePrefix}Pos:({pos.Row},{pos.Col}) One of the results cell is null");
                        actualCell.Should().NotBeNull($"{messagePrefix}Pos:({pos.Row},{pos.Col}) One of the results cell is null");

                        actualCell.Count.Should().Be(expected[i].Count, $"{messagePrefix}Pos:({pos.Row},{pos.Col}) Number of cell results do not match");

                        for (var j = 0; j < expected[i].Count; j++)
                        {
                            PivotGameResultsViewModelEqualityComparer.AreEqual(
                                expected[i][j],
                                actualCell[j],
                                $"{messagePrefix}Pos:({pos.Row},{pos.Col})ItemAt:{j}: ");
                        }
                    }
                }

            }

            return true;
        }

        private static (int Row, int Col) GetPosition(int i, int count)
        {
            var size = (int)Math.Sqrt(count);

            return (i / size, i % size);
        }
    }
}
namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Xunit;
    using UI.Areas.Mvc.ViewModels.Tournaments;
    using FluentAssertions;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class TournamentViewModelComparer
    {
        public static void AssertAreEqual(TournamentViewModel expected, TournamentViewModel actual)
        {
            if (expected == null && actual == null) return;

            Assert.False(expected == null || actual == null, "Instance should not be null.");

            actual.Id.Should().Be(expected.Id, "Ids should be equal.");
            actual.Name.Should().Be(expected.Name, $"[Id:{expected.Id}]Name should be equal.");
            actual.Description.Should().Be(expected.Description, $"[Id:{expected.Id}]Description should be equal.");
            actual.Location.Should().Be(expected.Location, $"[Id:{expected.Id}]Location should be equal.");
            actual.Season.Should().Be(expected.Season, $"[Id:{expected.Id}]Season should be equal.");
            actual.Scheme.Should().Be(expected.Scheme, $"[Id:{expected.Id}]Scheme should be equal.");
            actual.RegulationsLink.Should().Be(expected.RegulationsLink, $"[Id:{expected.Id}]RegulationsLink should be equal.");
            actual.IsNew.Should().Be(expected.IsNew, $"[Id:{expected.Id}]IsNew should be equal.");
            actual.IsDivisionsCountMin.Should().Be(expected.IsDivisionsCountMin, $"[Id:{expected.Id}]IsDivisionsCountMin should be equal.");
            actual.IsDivisionsCountMax.Should().Be(expected.IsDivisionsCountMax, $"[Id:{expected.Id}]IsDivisionsCountMax should be equal.");
            actual.IsTransferEnabled.Should().Be(expected.IsTransferEnabled, $"[Id:{expected.Id}]IsTransferEnabled should be equal.");
            actual.ApplyingPeriodStart.Should().Be(expected.ApplyingPeriodStart, $"[Id:{expected.Id}]ApplyingPeriodStart should be equal.");
            actual.ApplyingPeriodEnd.Should().Be(expected.ApplyingPeriodEnd, $"[Id:{expected.Id}]ApplyingPeriodEnd should be equal.");
            actual.IsArchived.Should().Be(expected.IsArchived, $"[Id:{expected.Id}]IsArchived should be equal.");
            actual.GamesEnd.Date.Should().Be(expected.GamesEnd.Date, $"[Id:{expected.Id}]GamesEnd should be equal.");
            actual.GamesStart.Date.Should().Be(expected.GamesStart.Date, $"[Id:{expected.Id}]GamesStart should be equal.");
            actual.TransferEnd?.Date.Should().Be(expected.TransferEnd.GetValueOrDefault().Date, $"[Id:{expected.Id}]TransferEnd should be equal.");
            actual.TransferStart?.Date.Should().Be(expected.TransferStart.GetValueOrDefault().Date, $"[Id:{expected.Id}]TransferStart should be equal.");
            actual.Authorization.Should().Be(expected.Authorization, $"[Id:{expected.Id}]Authorization should be equal.");

            actual.SeasonsList.Count.Should().Be(expected.SeasonsList.Count, $"[Id:{expected.Id}]Number of SeasonList items should be equal.");
            foreach (var season in expected.SeasonsList)
            {
                Assert.True(actual.SeasonsList.TryGetValue(season.Key, out var actualValue), $"[Id:{expected.Id}][SeasonKey:{season.Key}]Season list should have item with expected key.");
                actualValue.Should().Be(season.Value,
                    $"[Id:{expected.Id}][SeasonKey:{season.Key}]Season value should be equal.");
            }

            actual.Divisions.Count.Should().Be(expected.Divisions.Count, $"[Id:{expected.Id}]Number of Divisions items should be equal.");

            Assert.Equal(expected.Divisions, actual.Divisions, new DivisionViewModelComparer());
        }
    }
}

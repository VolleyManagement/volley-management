namespace VolleyManagement.UnitTests.Mvc.ViewModels
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.Tournaments;

    /// <summary>
    /// Comparer for tournament objects.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class TournamentViewModelComparer
    {
        public static void AssertAreEqual(TournamentViewModel expected, TournamentViewModel actual)
        {
            if (expected == null && actual == null) return;

            Assert.IsFalse(expected == null || actual == null, "Instance should not be null.");

            Assert.AreEqual(expected.Id, actual.Id, "Ids should be equal.");
            Assert.AreEqual(expected.Name, actual.Name, $"[Id:{expected.Id}]Name should be equal.");
            Assert.AreEqual(expected.Description, actual.Description, $"[Id:{expected.Id}]Description should be equal.");
            Assert.AreEqual(expected.Season, actual.Season, $"[Id:{expected.Id}]Season should be equal.");
            Assert.AreEqual(expected.Scheme, actual.Scheme, $"[Id:{expected.Id}]Scheme should be equal.");
            Assert.AreEqual(expected.RegulationsLink, actual.RegulationsLink, $"[Id:{expected.Id}]RegulationsLink should be equal.");
            Assert.AreEqual(expected.IsNew, actual.IsNew, $"[Id:{expected.Id}]IsNew should be equal.");
            Assert.AreEqual(expected.IsDivisionsCountMin, actual.IsDivisionsCountMin, $"[Id:{expected.Id}]IsDivisionsCountMin should be equal.");
            Assert.AreEqual(expected.IsDivisionsCountMax, actual.IsDivisionsCountMax, $"[Id:{expected.Id}]IsDivisionsCountMax should be equal.");
            Assert.AreEqual(expected.IsTransferEnabled, actual.IsTransferEnabled, $"[Id:{expected.Id}]IsTransferEnabled should be equal.");
            Assert.AreEqual(expected.ApplyingPeriodStart, actual.ApplyingPeriodStart, $"[Id:{expected.Id}]ApplyingPeriodStart should be equal.");
            Assert.AreEqual(expected.ApplyingPeriodEnd, actual.ApplyingPeriodEnd, $"[Id:{expected.Id}]ApplyingPeriodEnd should be equal.");
            Assert.AreEqual(expected.GamesEnd.Date, actual.GamesEnd.Date, $"[Id:{expected.Id}]GamesEnd should be equal.");
            Assert.AreEqual(expected.GamesStart.Date, actual.GamesStart.Date, $"[Id:{expected.Id}]GamesStart should be equal.");
            Assert.AreEqual(expected.TransferEnd?.Date, actual.TransferEnd?.Date, $"[Id:{expected.Id}]TransferEnd should be equal.");
            Assert.AreEqual(expected.TransferStart?.Date, actual.TransferStart?.Date, $"[Id:{expected.Id}]TransferStart should be equal.");
            Assert.AreEqual(expected.Authorization, actual.Authorization, $"[Id:{expected.Id}]Authorization should be equal.");

            Assert.AreEqual(expected.SeasonsList.Count, actual.SeasonsList.Count, $"[Id:{expected.Id}]Number of SeasonList items should be equal.");
            foreach (var season in expected.SeasonsList)
            {
                Assert.IsTrue(actual.SeasonsList.TryGetValue(season.Key, out var actualValue), $"[Id:{expected.Id}][SeasonKey:{season.Key}]Season list should have item with expected key.");
                Assert.AreEqual(
                    season.Value,
                    actualValue,
                    $"[Id:{expected.Id}][SeasonKey:{season.Key}]Season value should be equal.");
            }

            Assert.AreEqual(expected.Divisions.Count, actual.Divisions.Count, $"[Id:{expected.Id}]Number of Divisions items should be equal.");
            for (var i = 0; i < expected.Divisions.Count; i++)
            {
                DivisionViewModelEqualityComparer.AssertAreEqual(
                    expected.Divisions[i],
                    actual.Divisions[i],
                    $"[Id:{expected.Id}][Div#{i}]");
            }
        }
    }
}

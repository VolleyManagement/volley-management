namespace VolleyManagement.UnitTests.Mvc.ViewModels.Comparers
{
    using Xunit;
    using UI.Areas.Mvc.ViewModels.GameResults;
    using FluentAssertions;


    public static class ScoreViewModelComparer
    {
        public static void AssertEqual(ScoreViewModel expected, ScoreViewModel actual, string messagePrefix = "")
        {
            actual.Home.Should().Be(expected.Home, $"{messagePrefix}Home score should be equal.");
            actual.Away.Should().Be(expected.Away, $"{messagePrefix}Away score should be equal.");
            actual.IsTechnicalDefeat.Should().Be(expected.IsTechnicalDefeat, $"{messagePrefix}IsTechnicalDefeat should be equal.");
        }
    }
}
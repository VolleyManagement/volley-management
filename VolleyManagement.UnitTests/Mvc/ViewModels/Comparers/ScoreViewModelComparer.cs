namespace VolleyManagement.UnitTests.Mvc.ViewModels.Comparers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using UI.Areas.Mvc.ViewModels.GameResults;

    public static class ScoreViewModelComparer
    {
        public static void AssertAreEqual(ScoreViewModel expected, ScoreViewModel actual, string messagePrefix = "")
        {
            Assert.AreEqual(expected.Home, expected.Home, $"{messagePrefix}Home score should be equal.");
            Assert.AreEqual(expected.Away, expected.Away, $"{messagePrefix}Away score should be equal.");
            Assert.AreEqual(expected.IsTechnicalDefeat, expected.IsTechnicalDefeat, $"{messagePrefix}IsTechnicalDefeat should be equal.");
        }
    }
}
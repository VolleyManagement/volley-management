namespace VolleyManagement.UnitTests.WebApi.ViewModels.Schedule
{
    using System.Diagnostics.CodeAnalysis;
    using Xunit;
    using VolleyManagement.UI.Areas.WebAPI.ViewModels.Schedule;
    using FluentAssertions;

    [ExcludeFromCodeCoverage]
    internal static class DivisionTitleViewModelComparer
    {
        public static void AssertAreEqual(DivisionTitleViewModel expected, DivisionTitleViewModel actual, string messagePrefix = "")
        {
            actual.Id.Should().Be(expected.Id, $"{messagePrefix}Division id do not match");
            actual.Name.Should().Be(expected.Name, $"{messagePrefix}Division name do not match");

            actual.Rounds.Count.Should().Be(expected.Rounds.Count, $"{messagePrefix}Number of Round entries does not match.");

            var expectedEnumerator = expected.Rounds.GetEnumerator();
            var actualEnumerator = actual.Rounds.GetEnumerator();

            while (expectedEnumerator.MoveNext() && actualEnumerator.MoveNext())
            {
                actualEnumerator.Current.Should().Be(expectedEnumerator.Current,
                    $"{messagePrefix}Round name does not match");
            }
        }
    }
}
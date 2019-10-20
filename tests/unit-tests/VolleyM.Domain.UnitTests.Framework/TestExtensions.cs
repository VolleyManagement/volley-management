using FluentAssertions;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
    public static class TestExtensions
    {
        public static void ShouldBeError<T>(
            this Result<T> actualResult,
            Error expected,
            string because = "error should be reported",
            params object[] becauseArgs) where T : class
        {
            actualResult.Should().NotBeSuccessful("error is expected");
            actualResult.Error.Should().BeEquivalentTo(expected, because, becauseArgs);
        }
    }
}
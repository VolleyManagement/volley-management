using FluentAssertions;
using LanguageExt;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
    public static class TestExtensions
    {
        public static void ShouldBeError<T>(
            this Either<Error, T> actualResult,
            Error expected,
            string because = "error should be reported",
            params object[] becauseArgs)
        {
            actualResult.IsLeft.Should().BeTrue("error is expected");
            actualResult.IfLeft(e => e.Should().BeEquivalentTo(expected, because, becauseArgs));
        }

        public static void ShouldBeError<T>(
            this Either<Error, T> actualResult,
            ErrorType expectedType,
            string because = "error should be reported",
            params object[] becauseArgs)
        {
            actualResult.IsLeft.Should().BeTrue("error is expected");
            actualResult.IfLeft(e => e.Type.Should().BeEquivalentTo(expectedType, because, becauseArgs));
        }

        public static void ShouldBeEquivalent<T>(
            this Either<Error, T> actualResult,
            T expected,
            string because = "",
            params object[] becauseArgs)
        {
            actualResult.IsRight.Should().BeTrue("successful result is expected");
            actualResult.IfRight(v => v.Should().BeEquivalentTo(expected, because, becauseArgs));
        }
    }
}
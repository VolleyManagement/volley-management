using FluentAssertions;
using FluentAssertions.Execution;
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
			using (new AssertionScope())
			{
				actualResult.IsLeft.Should().BeTrue("error is expected");

				object actual = GetTransformedActual(actualResult);

				var expectation = new { Value = default(T), Error = expected };
				PerformAssertion<T>(because, becauseArgs, actual, expectation);
			}
		}

		public static void ShouldBeError<T>(
			this Either<Error, T> actualResult,
			ErrorType expectedType,
			string because = "error should be reported",
			params object[] becauseArgs)
		{
			using (new AssertionScope())
			{
				actualResult.IsLeft.Should().BeTrue("error is expected");

				object actual = GetTransformedActual(actualResult);

				var expectation = new { Value = default(T), Error = new { Type = expectedType } };
				PerformAssertion<T>(because, becauseArgs, actual, expectation);
			}
		}

		public static void ShouldBeEquivalent<T>(
			this Either<Error, T> actualResult,
			object expected,
			string because = "",
			params object[] becauseArgs)
		{
			using (new AssertionScope())
			{
				actualResult.IsRight.Should().BeTrue("successful result is expected");

				object actual = GetTransformedActual(actualResult);

				var expectation = new { Value = expected, Error = (Error)null };
				PerformAssertion<T>(because, becauseArgs, actual, expectation);
			}
		}

		private static void PerformAssertion<T>(string because, object[] becauseArgs, object actual, object expectation)
		{
			actual.Should().BeEquivalentTo(expectation, because, becauseArgs);
		}

		private static object GetTransformedActual<T>(Either<Error, T> actualResult)
		{
			var either = actualResult.ToArr()[0];

			var actual = new {Value = either.Right, Error = either.Left};
			return actual;
		}
	}
}
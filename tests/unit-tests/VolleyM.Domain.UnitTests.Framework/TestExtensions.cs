using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
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

				object actual = GetActual(actualResult);

				PerformAssertion<T, Error>(actual, expected, because, becauseArgs);
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

				object actual = GetActual(actualResult);

				PerformAssertion<T, object>(actual, new { Type = expectedType }, because, becauseArgs);
			}
		}

		public static void ShouldBeEquivalent<T>(
			this Either<Error, T> actualResult,
			object expected,
			string because = "",
			params object[] becauseArgs)
		{
			ShouldBeEquivalent(
				actualResult,
				expected,
				opts => opts,
				because,
				becauseArgs);
		}

		public static void ShouldBeEquivalent<TActual, TExpectation>(
			this Either<Error, TActual> actualResult,
			TExpectation expected,
			Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
			string because = "",
			params object[] becauseArgs)
		{
			using (new AssertionScope())
			{
				actualResult.IsRight.Should().BeTrue("successful result is expected");

				object actual = GetActual(actualResult);
				PerformAssertion<TActual, TExpectation>(actual, expected, config, because, becauseArgs);
			}
		}

		private static void PerformAssertion<TActual, TExpectation>(object actual, TExpectation expectation, string because = "", params object[] becauseArgs)
		{
			PerformAssertion<TActual, TExpectation>(actual, expectation, opts => opts, because, becauseArgs);
		}

		private static void PerformAssertion<TActual, TExpectation>(
			object actual,
			TExpectation expectation,
			Func<EquivalencyAssertionOptions<TExpectation>, EquivalencyAssertionOptions<TExpectation>> config,
			string because = "",
			params object[] becauseArgs)
		{
			switch (actual)
			{
				case TActual value:
					value.Should().BeEquivalentTo(expectation, config, because, becauseArgs);
					break;
				case Error error:
					error.Should().BeEquivalentTo(expectation, because, becauseArgs);
					break;
			}
		}

		private static object GetActual<T>(Either<Error, T> actualResult)
		{
			object actual;
			switch (actualResult.Case)
			{
				case (Error error):
					actual = error;
					break;
				case (T result):
					actual = result;
					break;
				default:
					throw new InvalidOperationException();
			}

			return actual;
		}
	}
}
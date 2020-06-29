using FluentAssertions;
using FluentAssertions.Execution;
using LanguageExt;
using LanguageExt.SomeHelp;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
	public static class TestExtensions
	{
		public static EitherAssertions<TLeft, TRight> Should<TLeft, TRight>(this Either<TLeft, TRight> either)
		{
			return new EitherAssertions<TLeft, TRight>(either);
		}

		public static void ShouldBeError<T>(
			this Either<Error, T> actualResult,
			Error expected,
			string because = "error should be reported",
			params object[] becauseArgs)
		{
			//actualResult.IsLeft.Should().BeTrue("error is expected");
			//actualResult.IfLeft(e => e.Should().BeEquivalentTo(expected, because, becauseArgs));

			var p = (Either<Error, T>)expected;

			actualResult.Should().BeEquivalentTo(p, because, becauseArgs);
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
			object expected,
			string because = "",
			params object[] becauseArgs)
		{
			//actualResult.IsRight.Should().BeTrue("successful result is expected");
			//actualResult.IfRight(v => v.Should().BeEquivalentTo(expected, because, becauseArgs));

			//var p = (Either<Error, T>)expected;


			actualResult.Should().BeEquivalentTo(expected, because, becauseArgs);
		}
	}

	public class EitherAssertions<TLeft, TRight>
	{
		private readonly Either<TLeft, TRight> _instance;

		public EitherAssertions(Either<TLeft, TRight> instance)
		{
			_instance = instance;
		}

		public AndConstraint<EitherAssertions<TLeft1, TRight1>> BeError<TLeft1, TRight1>(
			Error expected,
			string because = "error should be reported",
			params object[] becauseArgs) where TLeft1 : Error
		{
			Execute.Assertion
				.BecauseOf(because, becauseArgs)
				.ForCondition(_instance.IsLeft)
				.FailWith("error should be reported")
				.Then
				.Given(() => _instance.Match<Error>(r => default(Error), l => (Error) l))
				.ForCondition(left =>
				{
					left.Should().Be(expected);
					return true;
				});

			return new AndConstraint<EitherAssertions<TLeft1, TRight1>>(this);
		}
	}
}
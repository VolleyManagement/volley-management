using FluentAssertions;
using FluentAssertions.Equivalency;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
	public class VersionEquivalencyStep : IEquivalencyStep
	{
		public bool CanHandle(IEquivalencyValidationContext context,
			IEquivalencyAssertionOptions config)
		{
			return context.RuntimeType == typeof(Version);
		}

		public bool Handle(IEquivalencyValidationContext context, IEquivalencyValidator
			structuralEqualityValidator, IEquivalencyAssertionOptions config)
		{
			var version = context.Subject as Version;
			if (context.Expectation?.ToString() == "<some-version>")
			{
				version.Should().NotBeNull(context.Because, context.BecauseArgs);
			}
			else
			{
				context.Subject.Should().Be(context.Expectation, context.Because, context.BecauseArgs);
			}

			return true;
		}

	}
}
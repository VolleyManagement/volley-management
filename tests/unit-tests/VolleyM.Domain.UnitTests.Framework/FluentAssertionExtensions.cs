using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using VolleyM.Domain.Contracts;

namespace VolleyM.Domain.UnitTests.Framework
{
    public static class FluentAssertionExtensions
    {
        public static AndConstraint<ObjectAssertions> BeSuccessful(this ObjectAssertions node,
            string because = "",
            params object[] becauseArgs)
        {
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(CheckResultReportsSuccess(node.Subject))
                .FailWith("Expected {context:object} to report success{reason}, but was {0}.", node.Subject);
            return new AndConstraint<ObjectAssertions>(node);
        }

        private static bool CheckResultReportsSuccess(object subject)
        {
            if (subject is IResult result)
            {
                return result.IsSuccessful;
            }

            return false;
        }
    }
}
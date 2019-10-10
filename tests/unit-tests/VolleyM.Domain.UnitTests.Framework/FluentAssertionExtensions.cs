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
            ExecuteAssertion(node, false, because, becauseArgs);
            return new AndConstraint<ObjectAssertions>(node);
        }

        public static AndConstraint<ObjectAssertions> NotBeSuccessful(this ObjectAssertions node,
            string because = "",
            params object[] becauseArgs)
        {
            ExecuteAssertion(node, true, because, becauseArgs);
            return new AndConstraint<ObjectAssertions>(node);
        }

        private static void ExecuteAssertion(ObjectAssertions node, bool revertCondition, string because, object[] becauseArgs)
        {
            var reportMsg = revertCondition ? "error" : "success";
            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .ForCondition(CheckResult(node.Subject, revertCondition))
                .FailWith("Expected {context:object} to report {1}{reason}, but was {0}.", node.Subject, reportMsg);
        }

        private static bool CheckResult(object subject, bool revertCondition)
        {
            var result = CheckResultReportsSuccess(subject);
            if (revertCondition)
            {
                result = !result;
            }

            return result;
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
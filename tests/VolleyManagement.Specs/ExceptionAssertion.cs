namespace VolleyManagement.Specs
{
    using FluentAssertions;
    using System;

    public static class ExceptionAssertion
    {
        public static void AssertExceptionsAreEqual(Exception actual, Exception expected)
        {
            actual.Should().NotBeNull("Exception should not be null");
            actual.Should().BeOfType(expected.GetType(), $"Exception should be of {expected.GetType()}");
            actual.Message.Should().Be(expected.Message, $"Exception should have message \n\"{expected.Message}\".");
        }
    }
}

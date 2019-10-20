using System.Collections.Generic;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Contributors.UnitTests
{
    public interface IContributorsTestFixture : ITestFixture
    {
        void MockSeveralContributorsExist(List<ContributorDto> testData);
    }
}
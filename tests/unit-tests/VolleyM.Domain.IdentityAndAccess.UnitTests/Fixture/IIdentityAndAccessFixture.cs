﻿using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.IdentityAndAccess.UnitTests.Fixture
{
    public interface IIdentityAndAccessFixture : ITestFixture
    {
        void ConfigureUserExists(TenantId tenant, UserId id, User user);

        void ConfigureUserDoesNotExist(TenantId tenant, UserId id);

        void VerifyUserCreated(User user);
    }
}
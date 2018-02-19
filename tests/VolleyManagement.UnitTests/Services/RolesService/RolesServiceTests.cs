using System.Collections;
using VolleyManagement.Contracts.Authorization;

namespace VolleyManagement.UnitTests.Services.RolesService
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;
    using VolleyManagement.Data.Contracts;
    using VolleyManagement.Data.Queries.Common;
    using VolleyManagement.Data.Queries.User;
    using VolleyManagement.Domain.Dto;
    using VolleyManagement.Domain.RolesAggregate;
    using VolleyManagement.Services.Authorization;

    /// <summary>
    /// Tests <see cref="IRolesService"/> implementation
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RolesServiceTests
    {
        #region Fields

        private Mock<IQuery<ICollection<Role>, GetAllCriteria>> _getAllQueryMock;
        private Mock<IQuery<Role, FindByIdCriteria>> _getByIdQueryMock;
        private Mock<IQuery<List<UserInRoleDto>, FindByRoleCriteria>> _getUsersByRoleQueryMock;
        private Mock<IQuery<List<UserInRoleDto>, GetAllCriteria>> _getUserInRolesQueryMock;
        private Mock<IRoleRepository> _roleRepositoryMock;
        private Mock<IUnitOfWork> _uowMock;

        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            _getAllQueryMock = new Mock<IQuery<ICollection<Role>, GetAllCriteria>>();
            _getByIdQueryMock = new Mock<IQuery<Role, FindByIdCriteria>>();
            _getUsersByRoleQueryMock = new Mock<IQuery<List<UserInRoleDto>, FindByRoleCriteria>>();
            _getUserInRolesQueryMock = new Mock<IQuery<List<UserInRoleDto>, GetAllCriteria>>();
            _roleRepositoryMock = new Mock<IRoleRepository>();
            _uowMock = new Mock<IUnitOfWork>();

            _roleRepositoryMock.Setup(r => r.UnitOfWork).Returns(_uowMock.Object);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void GetAllRoles_PreexistingRolesStored_AllRolesReturned()
        {
            // Arange
            MockAllRoles(GetDefaultRoles());
            var service = BuildSUT();
            var expectedResult = GetDefaultRoles();

            // Act
            var actualResult = service.GetAllRoles();

            // Assert
            CollectionAssert.AreEqual(expectedResult, actualResult as ICollection, new RoleComparer());
        }

        [TestMethod]
        public void GetRole_GetExistingRole_RoleReturned()
        {
            // Arange
            const int ROLE_ID = 1;
            MockSingleRole(new Role { Id = ROLE_ID, Name = "Admin" });
            var service = BuildSUT();
            var expectedResult = new Role { Id = ROLE_ID, Name = "Admin" };

            // Act
            var actualResult = service.GetRole(ROLE_ID);

            // Assert
            TestHelper.AreEqual(expectedResult, actualResult, new RoleComparer());
        }

        [TestMethod]
        public void GetUsersInRole_RoleHasMembers_UsersReturned()
        {
            // Arange
            const int ROLE_ID = 1;
            var usersForRole = GetMembersForRole(ROLE_ID);
            MockUsersForRole(usersForRole);
            var service = BuildSUT();
            var expectedResult = GetMembersForRole(ROLE_ID);

            // Act
            var actualResult = service.GetUsersInRole(ROLE_ID);

            // Assert
            CollectionAssert.AreEqual(expectedResult, actualResult as ICollection, new UserInRoleComparer());
        }

        [TestMethod]
        public void GetAllUsersWithRoles_RoleHasMembers_AllUsersReturned()
        {
            // Arange
            const int ROLE_ID = 1;
            const int OTHER_ROLE_ID = 2;
            var usersForRole = GetUsersForSeveralRoles(ROLE_ID, OTHER_ROLE_ID);
            MockUserInRoles(usersForRole);
            var service = BuildSUT();

            var expectedResult = GetUsersForSeveralRoles(ROLE_ID, OTHER_ROLE_ID);

            // Act
            var actualResult = service.GetAllUsersWithRoles();

            // Assert
            CollectionAssert.AreEqual(expectedResult, actualResult as ICollection, new UserInRoleComparer());
        }

        [TestMethod]
        public void ChangeRoleMembership_UsersToAddExist_RoleUpdatedInRepository()
        {
            // Arrange
            const int ROLE_ID = 1;
            var usersToAdd = new[] { 2, 3, 4 }; // 3 new users
            var usersToRemove = new int[0]; // No removal of users
            var service = BuildSUT();

            // Act
            service.ChangeRoleMembership(ROLE_ID, usersToAdd, usersToRemove);

            // Assert
            _roleRepositoryMock.Verify(
                                 r => r.AddUserToRole(ROLE_ID, It.IsIn(usersToAdd)),
                                 Times.Exactly(usersToAdd.Length));
            _uowMock.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void ChangeRoleMembership_UsersToDeleteExist_RoleUpdatedInRepository()
        {
            // Arrange
            const int ROLE_ID = 1;
            var usersToAdd = new int[0]; // No new users
            var usersToRemove = new[] { 2, 3, 4 }; // 3 users to remove
            var service = BuildSUT();

            // Act
            service.ChangeRoleMembership(ROLE_ID, usersToAdd, usersToRemove);

            // Assert
            _roleRepositoryMock.Verify(
                                 r => r.RemoveUserFromRole(ROLE_ID, It.IsIn(usersToRemove)),
                                 Times.Exactly(usersToRemove.Length));
            _uowMock.Verify(u => u.Commit(), Times.Once);
        }

        [TestMethod]
        public void ChangeRoleMembership_RoleUnchanged_ChangesAreNotCommited()
        {
            // Arrange
            const int ROLE_ID = 1;
            var usersToAdd = new int[0]; // No new users
            var usersToRemove = new int[0]; // No removal of users
            var service = BuildSUT();

            // Act
            service.ChangeRoleMembership(ROLE_ID, usersToAdd, usersToRemove);

            // Assert
            _uowMock.Verify(u => u.Commit(), Times.Never);
            _uowMock.Verify(u => u.CommitAsync(), Times.Never);
        }

        #endregion

        #region Private
        private static List<Role> GetDefaultRoles()
        {
            return new List<Role>
            {
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }
            };
        }

        private static List<UserInRoleDto> GetMembersForRole(int roleId)
        {
            var result = new List<UserInRoleDto>
                       {
                           new UserInRoleDto { UserId = 1, UserName = "admin" },
                           new UserInRoleDto { UserId = 2, UserName = "user" },
                           new UserInRoleDto { UserId = 3, UserName = "guest" }
                       };

            result.ForEach(u => u.RoleIds.Add(roleId));

            return result;
        }

        private static List<UserInRoleDto> GetUsersForSeveralRoles(params int[] roleIds)
        {
            var result = new List<UserInRoleDto>();
            foreach (var roleId in roleIds)
            {
                result.AddRange(GetMembersForRole(roleId));
            }

            return result;
        }

        private RolesService BuildSUT()
        {
            return new RolesService(
                _getAllQueryMock.Object,
                _getByIdQueryMock.Object,
                _getUsersByRoleQueryMock.Object,
                _getUserInRolesQueryMock.Object,
                _roleRepositoryMock.Object);
        }

        private void MockAllRoles(List<Role> testData)
        {
            _getAllQueryMock.Setup(q => q.Execute(It.IsAny<GetAllCriteria>())).Returns(testData);
        }

        private void MockSingleRole(Role role)
        {
            _getByIdQueryMock.Setup(q => q.Execute(It.IsAny<FindByIdCriteria>())).Returns(role);
        }

        private void MockUsersForRole(List<UserInRoleDto> usersForRole)
        {
            _getUsersByRoleQueryMock.Setup(q => q.Execute(It.IsAny<FindByRoleCriteria>())).Returns(usersForRole);
        }

        private void MockUserInRoles(List<UserInRoleDto> usersForRole)
        {
            _getUserInRolesQueryMock.Setup(q => q.Execute(It.IsAny<GetAllCriteria>())).Returns(usersForRole);
        }

        #endregion
    }
}
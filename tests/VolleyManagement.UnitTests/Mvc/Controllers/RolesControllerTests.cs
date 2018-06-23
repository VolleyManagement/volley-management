namespace VolleyManagement.UnitTests.Mvc.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using Moq;
    using Contracts.Authorization;
    using Domain.Dto;
    using Domain.RolesAggregate;
    using FluentAssertions;
    using UI.Areas.Admin.Controllers;
    using UI.Areas.Admin.Models;
    using Comparers;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class RolesControllerTests
    {
        #region Fields

        private Mock<IRolesService> _rolesServiceMock;
        private Mock<IAuthorizationService> _authServiceMock;

        #endregion

        #region Init

        public RolesControllerTests()
        {
            _authServiceMock = new Mock<IAuthorizationService>();
            _rolesServiceMock = new Mock<IRolesService>();
        }

        #endregion

        #region Tests

        [Fact]
        public void Index_DefaultRoles_AllRolesReturned()
        {
            // Arrange
            var roles = GetDefaultRoles();
            _rolesServiceMock.Setup(r => r.GetAllRoles()).Returns(roles);
            var expected = GetDefaultRoleViewModels();
            var service = BuildSUT();

            // Act
            var actionResult = service.Index();

            // Assert
            var actual = GetModel<List<RoleViewModel>>(actionResult);
            Assert.Equal(expected, actual, new RoleViewModelComparer());

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [Fact]
        public void Details_RoleWithUsers_DetailsModelReturned()
        {
            // Arrange
            const int ROLE_ID = 1;
            var role = GetAnyRole(ROLE_ID);
            MockGetRole(ROLE_ID, role);
            var users = GetUsersForRole(ROLE_ID);
            MockGetUsersInRole(ROLE_ID, users);

            var expected = new RoleDetailsViewModel(role) { Users = new List<string> { "sa", "superuser" } };

            var service = BuildSUT();

            // Act
            var actionResult = service.Details(ROLE_ID);

            // Assert
            var actual = GetModel<RoleDetailsViewModel>(actionResult);
            AreDetailsModelsEqual(actual, expected);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [Fact]
        public void Edit_BeforeViewDisplayed_EditModelReturned()
        {
            // Arrange
            const int ROLE_ID = 1;
            const int OTHER_ROLE_ID = 2;
            var role = GetAnyRole(ROLE_ID);
            MockGetRole(ROLE_ID, role);
            var users = GetUserInRoles(ROLE_ID, OTHER_ROLE_ID);
            MockGetAllUsersWithRoles(users);

            var expected = new RoleEditViewModel(role) {
                UsersInRole = GetDefaultUsersInRole(),
                UsersOutsideRole = GetDefaultUsersOutsideRole()
            };

            var service = BuildSUT();

            // Act
            var actionResult = service.Edit(ROLE_ID);

            // Assert
            var actual = GetModel<RoleEditViewModel>(actionResult);
            AreEditModelsEqual(actual, expected);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        [Fact]
        public void Edit_ChangeMembershipSuccessful_RedirectedToIndex()
        {
            // Arrange
            var modifiedRolesModel = new ModifiedRoleViewModel {
                RoleId = 1,
                IdsToAdd = new[] { 1, 2 },
                IdsToDelete = new[] { 3, 4 }
            };

            var service = BuildSUT();

            // Act
            var actionResult = service.Edit(modifiedRolesModel);

            // Assert
            // ToDo: detailed arguments check
            _rolesServiceMock.Verify(
                r => r.ChangeRoleMembership(It.IsAny<int>(), It.IsAny<int[]>(), It.IsAny<int[]>()),
                Times.Once);

            AssertValidRedirectResult(actionResult);

            VerifyCheckAccess(AuthOperations.AdminDashboard.View, Times.Once());
        }

        #endregion

        #region Test Data

        private static List<Role> GetDefaultRoles()
        {
            return new List<Role>
                       {
                           new Role { Id = 1, Name = "Admin" },
                           new Role { Id = 2, Name = "User" }
                       };
        }

        private static List<RoleViewModel> GetDefaultRoleViewModels()
        {
            return new List<RoleViewModel>
                       {
                           new RoleViewModel { Id = 1, Name = "Admin" },
                           new RoleViewModel { Id = 2, Name = "User" }
                       };
        }

        private static Role GetAnyRole(int roleId)
        {
            return new Role { Id = roleId, Name = "Admin" };
        }

        private static List<UserInRoleDto> GetUsersForRole(int roleId)
        {
            return new List<UserInRoleDto>
                            {
                                new UserInRoleDto { UserId = 1, UserName = "sa", RoleIds = { roleId, 2 } },
                                new UserInRoleDto
                                    {
                                        UserId = 2,
                                        UserName = "superuser",
                                        RoleIds = { roleId, 3 }
                                    }
                            };
        }

        private static List<UserInRoleDto> GetUserInRoles(int oneRoleId, int otherRoleId)
        {
            return new List<UserInRoleDto>
                       {
                           new UserInRoleDto
                               {
                                   UserId = 1,
                                   UserName = "sa",
                                   RoleIds = { oneRoleId, 3 }
                               },
                           new UserInRoleDto
                               {
                                   UserId = 2,
                                   UserName = "superuser",
                                   RoleIds = { oneRoleId, 4 }
                               },
                           new UserInRoleDto
                               {
                                   UserId = 3,
                                   UserName = "captain1",
                                   RoleIds = { otherRoleId, 3 }
                               },
                           new UserInRoleDto
                               {
                                   UserId = 4,
                                   UserName = "user",
                                   RoleIds = { otherRoleId, 4 }
                               },
                           new UserInRoleDto
                               {
                                   UserId = 5,
                                   UserName = "guest",
                                   RoleIds = { 4, 5 }
                               }
                       };
        }

        private static List<UserViewModel> GetDefaultUsersInRole()
        {
            return new List<UserViewModel>
                       {
                           new UserViewModel { Id = 1, Name = "sa" },
                           new UserViewModel { Id = 2, Name = "superuser" }
                       };
        }

        private static List<UserViewModel> GetDefaultUsersOutsideRole()
        {
            return new List<UserViewModel>
                       {
                           new UserViewModel { Id = 3, Name = "captain1" },
                           new UserViewModel { Id = 4, Name = "user" },
                           new UserViewModel { Id = 5, Name = "guest" }
                       };
        }

        #endregion

        #region Custom assertions

        private static void AreDetailsModelsEqual(RoleDetailsViewModel actual, RoleDetailsViewModel expected)
        {
            actual.Id.Should().Be(expected.Id, "Role ID does not match");
            actual.Name.Should().Be(expected.Name, "Role Names are different");
            actual.Users.Should().BeEquivalentTo(expected.Users, "Users lists are different");
        }

        private static void AreEditModelsEqual(RoleEditViewModel actual, RoleEditViewModel expected)
        {
            actual.Id.Should().Be(expected.Id, "Role ID does not match");
            actual.Name.Should().Be(expected.Name, "Role Names are different");
            TestHelper.AreEqual(
                expected.UsersInRole,
                actual.UsersInRole,
                new UserViewModelComparer(),
                "Users in Role lists are different");
            TestHelper.AreEqual(
                expected.UsersOutsideRole,
                actual.UsersOutsideRole,
                new UserViewModelComparer(),
                "Users outside Role lists are different");
        }

        private static void AssertValidRedirectResult(ActionResult actionResult)
        {
            var result = (RedirectToRouteResult)actionResult;
            Assert.False(result.Permanent, "Redirect should not be permanent");
            result.RouteValues.Count.Should().Be(1, "Redirect should forward to Roles.Index action");
            result.RouteValues["action"].Should().Be("Index", "Redirect should forward to Roles.Index action");
        }

        private static void AssertModelStateError(ModelStateDictionary modelState, string errorMessage)
        {
            Assert.False(modelState.IsValid, "Edit action should report error");
            modelState[string.Empty].Errors[0].ErrorMessage.Should().Be(
                errorMessage,
                "Edit action should report error message provided by exception");
        }

        #endregion

        #region Helpers

        private static T GetModel<T>(ActionResult actionResult)
        {
            return (T)((ViewResult)actionResult).Model;
        }

        #endregion

        #region Mocks

        private void MockGetAllUsersWithRoles(List<UserInRoleDto> users)
        {
            _rolesServiceMock.Setup(r => r.GetAllUsersWithRoles()).Returns(users);
        }

        private void MockGetUsersInRole(int roleId, List<UserInRoleDto> users)
        {
            _rolesServiceMock.Setup(r => r.GetUsersInRole(roleId)).Returns(users);
        }

        private void MockGetRole(int roleId, Role role)
        {
            _rolesServiceMock.Setup(r => r.GetRole(roleId)).Returns(role);
        }

        #endregion

        #region Private

        private RolesController BuildSUT()
        {
            return new RolesController(_rolesServiceMock.Object, _authServiceMock.Object);
        }

        private void VerifyCheckAccess(AuthOperation operation, Times times)
        {
            _authServiceMock.Verify(tr => tr.CheckAccess(operation), times);
        }

        #endregion
    }
}
﻿namespace VolleyManagement.UnitTests.Services.AuthService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Contracts.Authorization;
    using Contracts.Exceptions;
    using Data.Contracts;
    using Data.Queries.Common;
    using Domain.RolesAggregate;
    using VolleyManagement.Services.Authorization;

    /// <summary>
    /// Tests <see cref="IAuthorizationService"/> implementation
    /// </summary>
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class AuthorizationServiceTests
    {
        #region Fields

        private const byte AREA_1_ID = 1;
        private const byte AREA_2_ID = 2;
        private const int OPERATION_1_ID = 1;
        private const int OPERATION_2_ID = 2;
        private const int OPERATION_3_ID = 3;
        private const int OPERATION_4_ID = 4;

        private const byte BYTE_SIZE_SHIFT = 8;

        private Mock<IQuery<ICollection<AuthOperation>, FindByUserIdCriteria>> _getByIdQueryMock;
        private Mock<ICurrentUserService> _currentUserService;
        private Type[] _authOperationsAreas = typeof(AuthOperations).GetNestedTypes();

        #endregion

        #region Init

        [TestInitialize]
        public void TestInit()
        {
            _getByIdQueryMock = new Mock<IQuery<ICollection<AuthOperation>, FindByUserIdCriteria>>();
            _currentUserService = new Mock<ICurrentUserService>();
        }

        #endregion

        #region Service tests

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void CheckAccess_OperationNotPermitted_AuthorizationExceptionThrown()
        {
            // Arrange
            var allowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID),
                Tuple.Create(AREA_1_ID, OPERATION_3_ID)
            };

            MockAllowedOperations(allowedOperations);

            var operationToCheck = Tuple.Create(AREA_1_ID, OPERATION_4_ID);
            var service = BuildSUT();

            // Act
            service.CheckAccess(operationToCheck);
        }

        [TestMethod]
        [ExpectedException(typeof(AuthorizationException))]
        public void CheckAccess_NoOperationsPermitted_AuthorizationExceptionThrown()
        {
            // Arrange
            var operationToCheck = Tuple.Create(AREA_1_ID, OPERATION_1_ID);
            var allowedOperations = new List<AuthOperation>();
            MockAllowedOperations(allowedOperations);

            var service = BuildSUT();

            // Act
            service.CheckAccess(operationToCheck);
        }

        [TestMethod]
        public void CheckAccess_OperationPermitted_AuthorizationExceptionNotThrown()
        {
            // Arrange
            var allowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID),
                Tuple.Create(AREA_1_ID, OPERATION_3_ID)
            };

            MockAllowedOperations(allowedOperations);

            var operationToCheck = Tuple.Create(AREA_1_ID, OPERATION_2_ID);
            var service = BuildSUT();

            // Act
            service.CheckAccess(operationToCheck);
        }

        [TestMethod]
        public void GetAllowedOperations_AllAllowedOperationsSpecified_AllAllowed()
        {
            // Arrange
            var allAllowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID),
                Tuple.Create(AREA_1_ID, OPERATION_3_ID)
            };
            MockAllowedOperations(allAllowedOperations);

            var requestedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID),
                Tuple.Create(AREA_1_ID, OPERATION_3_ID)
            };

            var service = BuildSUT();

            // Act
            var allowedOperations = service.GetAllowedOperations(requestedOperations);

            // Assert
            foreach (var item in requestedOperations)
            {
                Assert.IsTrue(allowedOperations.IsAllowed(item));
            }
        }

        [TestMethod]
        public void GetAllowedOperations_NotAllAllowedOperationsSpecified_OnlySpecifiedAllowed()
        {
            // Arrange
            var operationToCheck = Tuple.Create(AREA_1_ID, OPERATION_1_ID);
            var allAllowedOperations = new List<AuthOperation>()
            {
                operationToCheck,
                Tuple.Create(AREA_1_ID, OPERATION_2_ID),
                Tuple.Create(AREA_1_ID, OPERATION_3_ID)
            };
            MockAllowedOperations(allAllowedOperations);

            var requestedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_2_ID),
                Tuple.Create(AREA_1_ID, OPERATION_3_ID)
            };

            var service = BuildSUT();

            // Act
            var allowedOperations = service.GetAllowedOperations(requestedOperations);

            // Assert
            Assert.IsFalse(allowedOperations.IsAllowed(operationToCheck));
            foreach (var item in requestedOperations)
            {
                Assert.IsTrue(allowedOperations.IsAllowed(item));
            }
        }

        [TestMethod]
        public void GetAllowedOperations_NotAllowedOperationsSpecified_NoOperationsAllowed()
        {
            // Arrange
            var allAllowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID)
            };
            MockAllowedOperations(allAllowedOperations);

            var requestedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_3_ID),
                Tuple.Create(AREA_1_ID, OPERATION_4_ID)
            };

            var service = BuildSUT();

            // Act
            var allowedOperations = service.GetAllowedOperations(requestedOperations);

            // Assert
            foreach (var item in allAllowedOperations)
            {
                Assert.IsFalse(allowedOperations.IsAllowed(item));
            }

            foreach (var item in requestedOperations)
            {
                Assert.IsFalse(allowedOperations.IsAllowed(item));
            }
        }

        [TestMethod]
        public void GetAllowedOperations_EmptyListSpecified_NoOneAllowed()
        {
            // Arrange
            var allAllowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID)
            };
            MockAllowedOperations(allAllowedOperations);

            var requestedOperations = new List<AuthOperation>();
            var service = BuildSUT();

            // Act
            var allowedOperations = service.GetAllowedOperations(requestedOperations);

            // Assert
            foreach (var item in allAllowedOperations)
            {
                Assert.IsFalse(allowedOperations.IsAllowed(item));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllowedOperations_NullListSpecified_ArgumentNullExceptionThrown()
        {
            // Arrange
            var allAllowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID)
            };
            MockAllowedOperations(allAllowedOperations);

            List<AuthOperation> requestedOperations = null;
            var service = BuildSUT();

            // Act
            var allowedOperations = service.GetAllowedOperations(requestedOperations);
        }

        [TestMethod]
        public void GetAllowedOperations_OneAllowedOperationSpecified_OnlySpecifiedAllowed()
        {
            // Arrange
            var allAllowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID),
                Tuple.Create(AREA_1_ID, OPERATION_3_ID)
            };
            MockAllowedOperations(allAllowedOperations);

            var requestedOperation = Tuple.Create(AREA_1_ID, OPERATION_2_ID);
            var service = BuildSUT();

            // Act
            var allowedOperations = service.GetAllowedOperations(requestedOperation);

            // Assert
            Assert.IsFalse(allowedOperations.IsAllowed(Tuple.Create(AREA_1_ID, OPERATION_1_ID)));
            Assert.IsFalse(allowedOperations.IsAllowed(Tuple.Create(AREA_1_ID, OPERATION_3_ID)));
            Assert.IsTrue(allowedOperations.IsAllowed(requestedOperation));
        }

        [TestMethod]
        public void GetAllowedOperations_NotAllowedOperationSpecified_NoOneAllowed()
        {
            // Arrange
            var allAllowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID)
            };
            MockAllowedOperations(allAllowedOperations);
            var requestedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_3_ID),
                Tuple.Create(AREA_1_ID, OPERATION_4_ID)
            };

            var service = BuildSUT();

            // Act
            var allowedOperations = service.GetAllowedOperations(requestedOperations);

            // Assert
            foreach (var item in allAllowedOperations)
            {
                Assert.IsFalse(allowedOperations.IsAllowed(item));
            }

            foreach (var item in requestedOperations)
            {
                Assert.IsFalse(allowedOperations.IsAllowed(item));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetAllowedOperations_NullSpecified_ArgumentNullExceptionThrown()
        {
            // Arrange
            var allAllowedOperations = new List<AuthOperation>()
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID)
            };
            MockAllowedOperations(allAllowedOperations);

            AuthOperation requestedOperations = null;
            var service = BuildSUT();

            // Act
            var allowedOperations = service.GetAllowedOperations(requestedOperations);
        }

        #endregion

        #region AllowedOperations tests

        [TestMethod]
        public void IsAllowed_OperationNotPermitted_OperationIsNotAllowed()
        {
            // Arrange
            var operationToCheck = Tuple.Create(AREA_1_ID, OPERATION_3_ID);
            var allowedOperations = new AllowedOperations(new List<AuthOperation>
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID)
            });

            // Act
            bool result = allowedOperations.IsAllowed(operationToCheck);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAllowed_NoOperationsPermitted_OperationIsNotAllowed()
        {
            // Arrange
            var operationToCheck = Tuple.Create(AREA_1_ID, OPERATION_3_ID);
            var allowedOperations = new AllowedOperations(new List<AuthOperation>());

            // Act
            bool result = allowedOperations.IsAllowed(operationToCheck);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAllowed_OperationPermitted_OperationIsAllowed()
        {
            // Arrange
            var operationToCheck = Tuple.Create(AREA_1_ID, OPERATION_1_ID);
            var allowedOperations = new AllowedOperations(new List<AuthOperation>
            {
                Tuple.Create(AREA_1_ID, OPERATION_1_ID),
                Tuple.Create(AREA_1_ID, OPERATION_2_ID)
            });

            // Act
            bool result = allowedOperations.IsAllowed(operationToCheck);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region AuthOperations tests

        [TestMethod]
        public void AuthOperations_AllAreas_AllAreasHasDifferentId()
        {
            // Arrange
            var processedAreas = new Dictionary<byte, string>();
            string errors = string.Empty;

            // Act
            foreach (var area in _authOperationsAreas)
            {
                var areaOperations = area.GetFields();
                var areaName = area.Name;
                foreach (var operation in areaOperations)
                {
                    short operationId = (AuthOperation)operation.GetValue(null);
                    var areaId = (byte)(operationId >> BYTE_SIZE_SHIFT);
                    if (processedAreas.ContainsKey(areaId))
                    {
                        if (processedAreas[areaId] != areaName)
                        {
                            errors += string.Format("{0} → {1}; \n", areaName, operation.Name);
                        }
                    }
                    else
                    {
                        processedAreas.Add(areaId, areaName);
                    }
                }
            }

            // Assert
            if (!string.IsNullOrEmpty(errors))
            {
                throw new AssertFailedException(errors);
            }
        }

        [TestMethod]
        public void AuthOperations_AllAreas_AllOperationsInAreaHasDifferentId()
        {
            // Arrange
            string errors = string.Empty;

            // Act
            foreach (var area in _authOperationsAreas)
            {
                var areaOperations = area.GetFields();
                var areaName = area.Name;
                var processed = new List<int>();
                foreach (var operation in areaOperations)
                {
                    var operationId = (AuthOperation)operation.GetValue(null);
                    var operationKeyId = (byte)((operationId << BYTE_SIZE_SHIFT) >> BYTE_SIZE_SHIFT);

                    if (processed.Contains(operationKeyId))
                    {
                        errors += string.Format("{0} → {1}; \n", areaName, operation.Name);
                    }
                    else
                    {
                        processed.Add(operationKeyId);
                    }
                }
            }

            // Assert
            if (!string.IsNullOrEmpty(errors))
            {
                throw new AssertFailedException(errors);
            }
        }

        [TestMethod]
        public void AuthOperations_AllAreas_AllOperationsInAreaHasSameAreaId()
        {
            // Arrange
            string errors = string.Empty;

            // Act
            foreach (var area in _authOperationsAreas)
            {
                var areaOperations = area.GetFields();
                var areaName = area.Name;
                byte? areaId = null;
                foreach (var operation in areaOperations)
                {
                    short operationId = (AuthOperation)operation.GetValue(null);
                    var operationAreaId = (byte)(operationId >> BYTE_SIZE_SHIFT);

                    if (areaId.HasValue && areaId.Value != operationAreaId)
                    {
                        errors += string.Format("{0} → {1}; \n", areaName, operation.Name);
                    }
                    else
                    {
                        areaId = operationAreaId;
                    }
                }
            }

            // Assert
            if (!string.IsNullOrEmpty(errors))
            {
                throw new AssertFailedException(errors);
            }
        }

        [TestMethod]
        public void AuthOperation_SameAreaIdOperationId_OperationsEquals()
        {
            // Arrange
            AuthOperation operation1 = Tuple.Create(AREA_1_ID, OPERATION_1_ID);
            AuthOperation operation2 = Tuple.Create(AREA_1_ID, OPERATION_1_ID);

            // Act
            bool result = operation1 == operation2;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AuthOperation_DifferenAreaIdSameOperationId_OperationsNotEquals()
        {
            // Arrange
            AuthOperation operation1 = Tuple.Create(AREA_1_ID, OPERATION_1_ID);
            AuthOperation operation2 = Tuple.Create(AREA_2_ID, OPERATION_1_ID);

            // Act
            bool result = operation1 == operation2;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AuthOperation_SameAreaIdDifferentOperationId_OperationsNotEquals()
        {
            // Arrange
            AuthOperation operation1 = Tuple.Create(AREA_1_ID, OPERATION_1_ID);
            AuthOperation operation2 = Tuple.Create(AREA_1_ID, OPERATION_2_ID);

            // Act
            bool result = operation1 == operation2;

            // Assert
            Assert.IsFalse(result);
        }

        #endregion

        #region Private

        private AuthorizationService BuildSUT()
        {
            return new AuthorizationService(_currentUserService.Object, _getByIdQueryMock.Object);
        }

        private void MockAllowedOperations(List<AuthOperation> operations)
        {
            _getByIdQueryMock.Setup(q => q.Execute(It.IsAny<FindByUserIdCriteria>())).Returns(operations);
        }

        #endregion
    }
}

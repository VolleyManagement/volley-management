﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using LanguageExt;
using NSubstitute;
using SimpleInjector;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.FeatureManagement;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.IdentityAndAccess.RolesAggregate;
using VolleyM.Domain.IDomainFrameworkTestFixture;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Framework.UnitTests.FeatureManagement
{
	[Binding]
	[Scope(Feature = "Feature Toggle Decorator")]
	public class FeatureToggleDecoratorSteps
	{
		private enum HandlerType
		{
			SampleHandler
		}

		private HandlerType _handlerType;
		private Either<Error, Unit> _actualResult;

		private readonly Container _container;
		private IAuthorizationService _authorizationService;
		private IFeatureManager _featureManager;

		public FeatureToggleDecoratorSteps(Container container)
		{
			_container = container;
		}

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_REGISTER_DEPENDENCIES_ORDER)]
		public void RegisterDependenciesForScenario()
		{
			RegisterHandlers();

			_authorizationService = Substitute.For<IAuthorizationService>();
			_container.RegisterInstance(_authorizationService);

			_featureManager = Substitute.For<IFeatureManager>();
			_container.RegisterInstance(_featureManager);
		}

		[Given(@"I have a handler")]
		public void GivenIHaveAHandler()
		{
			// do nothing
		}

		[Given(@"feature toggle exists")]
		public void GivenFeatureToggleExists()
		{
			_handlerType = HandlerType.SampleHandler;
			SetPermissionForHandler();
		}

		[Given(@"feature toggle is enabled")]
		public void GivenFeatureToggleIsEnabled()
		{
			var featureData = GetFeatureData(_handlerType);

			MockFeatureService(featureData, true);
		}

		[Given(@"feature toggle is disabled")]
		public void GivenFeatureToggleIsDisabled()
		{
			var featureData = GetFeatureData(_handlerType);

			MockFeatureService(featureData, false);
		}

		[When(@"I call decorated handler")]
		public async Task WhenICallDecoratedHandler()
		{
			_actualResult = await ResolveAndCallHandler(_handlerType).ToEither();
		}

		[Then(@"handler result should be returned")]
		public void ThenHandlerResultShouldBeReturned()
		{
			_actualResult.ShouldBeEquivalent(Unit.Default);
		}

		[Then(@"feature disabled error should be returned")]
		public void ThenFeatureDisabledErrorShouldBeReturned()
		{
			_actualResult.ShouldBeError(ErrorType.FeatureDisabled);
		}

		private EitherAsync<Error, Unit> ResolveAndCallHandler(HandlerType handlerType)
		{
			return handlerType switch
			{
				HandlerType.SampleHandler => ResolveAndCallSpecificHandler(new SampleHandler.Request()),
				_ => throw new NotSupportedException()
			};
		}
		private EitherAsync<Error, Unit> ResolveAndCallSpecificHandler<T>(T request) where T : IRequest<Unit>
		{
			var handler = _container.GetInstance<IRequestHandler<T, Unit>>();

			return handler.Handle(request);
		}

		private void RegisterHandlers()
		{
			FrameworkDomainComponentDependencyRegistrar.RegisterCommonServices(_container,
				new List<Assembly> { Assembly.GetAssembly(GetType()) });
		}

		private (string Context, string Action) GetFeatureData(HandlerType handlerType)
		{
			return handlerType switch
			{
				HandlerType.SampleHandler => (nameof(IDomainFrameworkTestFixture), nameof(SampleHandler)),
				_ => throw new NotSupportedException()
			};
		}
		private void MockFeatureService((string Context, string Action) featureData, bool isEnabled)
		{
			_featureManager.IsEnabledAsync(featureData.Action, featureData.Context).Returns(Task.FromResult(isEnabled));
		}
		private void SetPermissionForHandler()
		{
			_authorizationService
				.CheckAccess(
					new Permission(nameof(IDomainFrameworkTestFixture), _handlerType.ToString()))
				.Returns(true);
		}
	}
}
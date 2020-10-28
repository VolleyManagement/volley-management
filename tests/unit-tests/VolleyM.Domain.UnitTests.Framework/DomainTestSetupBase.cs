using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using BoDi;
using NSubstitute;
using Serilog;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using TechTalk.SpecFlow;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.Crosscutting;
using VolleyM.Domain.Contracts.FeatureManagement;
using VolleyM.Domain.Framework;
using VolleyM.Domain.Framework.EventBroker;
using VolleyM.Domain.IdentityAndAccess;
using VolleyM.Domain.IdentityAndAccess.Handlers;
using VolleyM.Domain.UnitTests.Framework.Common;
using VolleyM.Domain.UnitTests.Framework.Transforms.Common;
using VolleyM.Infrastructure.Bootstrap;

namespace VolleyM.Domain.UnitTests.Framework
{
	public abstract class DomainTestSetupBase
	{
		private readonly IObjectContainer _objectContainer;
		private readonly FeatureContext _featureContext;

		#region Scenario variables

		protected Container Container { get; private set; }

		private ITestFixture BaseTestFixture { get; set; }

		private IAuthFixture AuthFixture { get; set; }

		private Scope _scope;

		protected DomainTestSetupBase(IObjectContainer objectContainer, FeatureContext featureContext)
		{
			_objectContainer = objectContainer;
			_featureContext = featureContext;
		}

		#endregion

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_INIT_CONTAINER_ORDER)]
		public void InitializeContainer()
		{
			Container = new Container();
			ConfigureContainer();

			RegisterContainerInSpecFlow(Container);
			RegisterSpecFlowTransforms();

			RegisterMinimalInfrastructureDependencies(Container);
			RegisterAssemblyBootstrappers();

			RegisterFeatureService(Container);
			RegisterNullEventPublisher(Container);

			BaseTestFixture = CreateAndRegisterTestFixtureIfNeeded(TestRunFixtureBase.Target);
			AuthFixture = CreateAndRegisterAuthFixtureIfNeeded();

			AuthFixture?.ConfigureTestUserRole(Container);
			BaseTestFixture.RegisterScenarioDependencies(Container);
		}

		private void RegisterMinimalInfrastructureDependencies(Container container)
		{
			// Need to register some components as SimpleInjector performs a Verify during first resolve.
			// Should go before Assembly Bootstrappers
			container.RegisterInstance(Substitute.For<IRequestHandler<CreateUser.Request, User>>());
			container.RegisterInstance(Substitute.For<IRequestHandler<GetUser.Request, User>>());
			container.RegisterInstance(Substitute.For<IApplicationInfo>());
		}

		private void ConfigureContainer()
		{
			Container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();
			// Allow some tests to override existing registrations to replace it with test doubles
			Container.Options.AllowOverridingRegistrations = true;
			// should be last
			Container.Options.LifestyleSelectionBehavior = new VolleyMLifestyleSelectionBehavior(Container.Options);
		}

		private void RegisterNullEventPublisher(Container container)
		{
			container.Register<IEventPublisher, TestSpyEventPublisher>(Lifestyle.Singleton);
		}

		private void RegisterFeatureService(Container container)
		{
			var featureService = Substitute.For<IFeatureManager>();
			featureService.IsEnabledAsync(Arg.Any<string>(), Arg.Any<string>())
				.Returns(Task.FromResult(true));

			container.RegisterInstance(featureService);
		}

		private void RegisterSpecFlowTransforms()
		{
			Container.Register<ISpecFlowTransformFactory, SpecFlowTransformFactory>(Lifestyle.Scoped);
			Container.Register<SpecFlowTransform>(Lifestyle.Scoped);
			var serviceTypes = new List<Type>
			{
				typeof(VersionTransform),
				typeof(TenantIdTransform)
			};

			serviceTypes.AddRange(GetAssemblyTransformTypes());

			Container.Collection.Register<ISpecFlowTransform>(serviceTypes);

			// 2. Make transforms aware of the context
			
		}

		[BeforeScenario(Order = Constants.BEFORE_SCENARIO_STEPS_BASE_ORDER)]
		public void ScenarioSetup()
		{
			_scope = ThreadScopedLifestyle.BeginScope(Container);

			AuthFixture?.ConfigureTestUser(Container, GetFeatureTenantId());

			BaseTestFixture.ScenarioSetup();
		}

		private TenantId GetFeatureTenantId()
		{
			var feature = _featureContext.FeatureInfo.Title;
			feature = feature.ToLowerInvariant().Replace(' ', '-');

			return new TenantId($"F:{feature}");
		}

		[AfterScenario(Order = Constants.AFTER_SCENARIO_TEST_FRAMEWORK_ORDER)]
		public void AfterEachScenario()
		{
			BaseTestFixture.ScenarioTearDown();

			Log.Warning("Scope is about to be disposed. Feature={FeatureTitle}, {ThreadId}; ", _featureContext.FeatureInfo.Title, Thread.CurrentThread.ManagedThreadId);
			_scope.Dispose();
			_scope = null;
			Container.Dispose();
			Container = null;
		}

		/// <summary>
		/// Indicates whether default Authentication should be configured or test fixture will handle it.
		/// </summary>
		/// <remarks>Almost all Domain specific tests need this</remarks>
		protected abstract bool RequiresAuthorizationFixture { get; }

		protected abstract IEnumerable<IAssemblyBootstrapper> GetAssemblyBootstrappers(TestTarget testTarget);

		/// <summary>
		/// Returns interface type of concrete ITestFixture
		/// </summary>
		protected virtual Type GetConcreteTestFixtureType => typeof(NoOpTestFixture);

		protected virtual ITestFixture CreateTestFixture(TestTarget testTarget)
		{
			return new NoOpTestFixture();
		}

		/// <summary>
		/// Provides list of transformations used in the Feature file bindings for the cases where SpecFlow cannot handle it
		/// </summary>
		/// <returns></returns>
		protected virtual List<ISpecFlowTransform> GetAssemblyTransforms()
		{
			return new List<ISpecFlowTransform>();
		}

		/// <summary>
		/// Provides list of transformations used in the Feature file bindings for registering in the DI
		/// </summary>
		/// <returns></returns>
		protected virtual List<Type> GetAssemblyTransformTypes()
		{
			return new List<Type>();
		}

		private TenantId CurrentTenantProvider()
		{
			return Container.GetInstance<ICurrentUserProvider>().Tenant;
		}

		#region Test Configuration

		private void RegisterAssemblyBootstrappers()
		{
			var mce = new MapperConfigurationExpression();
			mce.ConstructServicesUsing(Container.GetInstance);
			var bootstrappers = GetBootstrappers().ToList();

			Log.Debug("Registering common bootstrappers");
			var assembliesToRegister = bootstrappers
				.Where(b => b.HasDomainComponents)
				.Select(GetAssemblyFromBootstrapper)
				.ToList();

			var componentsRegistrars = bootstrappers
				.Select(b => b.DomainComponentDependencyRegistrar)
				.Where(r => r != null)
				.ToList();

			foreach (var registrar in componentsRegistrars)
			{
				registrar.RegisterCommonDependencies(Container, assembliesToRegister);
			}

			Log.Debug("Registering individual bootstrappers");
			foreach (var bootstrapper in GetBootstrappers())
			{
				bootstrapper.RegisterDependencies(Container, TestRunFixtureBase.Configuration);
				bootstrapper.RegisterMappingProfiles(mce);
			}

			var mc = new MapperConfiguration(mce);
			mc.AssertConfigurationIsValid();

			Container.RegisterSingleton<IMapper>(() => new Mapper(mc, Container.GetInstance));
		}

		private IEnumerable<IAssemblyBootstrapper> GetBootstrappers() =>
			new List<IAssemblyBootstrapper>(GetAssemblyBootstrappers(TestRunFixtureBase.Target)) {
				new DomainFrameworkAssemblyBootstrapper()
			};

		private ITestFixture CreateAndRegisterTestFixtureIfNeeded(TestTarget target)
		{
			var result = CreateTestFixture(target);

			var fixtureType = GetConcreteTestFixtureType;
			if (fixtureType != typeof(NoOpTestFixture))
			{
				_objectContainer.RegisterInstanceAs(result, fixtureType);
			}

			return result;
		}

		private IAuthFixture CreateAndRegisterAuthFixtureIfNeeded()
		{
			if (!RequiresAuthorizationFixture) return null;

			var fixture = new AuthFixture();

			_objectContainer.RegisterInstanceAs(fixture, typeof(IAuthFixture));

			return fixture;
		}

		private void RegisterContainerInSpecFlow(Container container)
		{
			_objectContainer.RegisterInstanceAs(container);
		}
		private static Assembly GetAssemblyFromBootstrapper(IAssemblyBootstrapper bootstrapper)
		{
			return Assembly.GetAssembly(bootstrapper.GetType());
		}

		#endregion
	}
}
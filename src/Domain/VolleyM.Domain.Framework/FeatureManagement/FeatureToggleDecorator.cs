using LanguageExt;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Contracts.FeatureManagement;
using VolleyM.Domain.Framework.HandlerMetadata;

namespace VolleyM.Domain.Framework.FeatureManagement
{
	public class FeatureToggleDecorator<TRequest, TResponse>
        : DecoratorBase<IRequestHandler<TRequest, TResponse>>, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IFeatureManager _featureManager;
        private readonly HandlerMetadataService _handlerMetadataService;

        public FeatureToggleDecorator(IRequestHandler<TRequest, TResponse> decoratee, IFeatureManager featureManager, HandlerMetadataService handlerMetadataService)
            : base(decoratee)
        {
            _featureManager = featureManager;
            _handlerMetadataService = handlerMetadataService;
        }

        public EitherAsync<Error, TResponse> Handle(TRequest request)
        {
            var featureEnabled = GetFeatureInfo(RootInstance)
                .MapAsync(async fi => await _featureManager.IsEnabledAsync(fi.Action, fi.Context)).Map(p => p.Match(
	                Right: enabled => enabled
		                ? (Either<Error, Unit>)Unit.Default
		                : Error.FeatureDisabled(),
	                Left: e => e
                ))
                .ToAsync(); ;

            return featureEnabled.Bind(_ => Decoratee.Handle(request));
        }


        private Either<Error, (string Context, string Action)> GetFeatureInfo(
            IRequestHandler<TRequest, TResponse> handler)
        {
            return _handlerMetadataService.GetHandlerMetadata(handler)
                .Map(m => (m.Context, m.Action));
        }
    }
}
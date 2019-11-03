using Esquio.Abstractions;
using LanguageExt;
using System.Threading.Tasks;
using VolleyM.Domain.Contracts;
using VolleyM.Domain.Framework.Authorization;
using VolleyM.Domain.Framework.HandlerMetadata;

namespace VolleyM.Domain.Framework.FeatureManagement
{
    public class FeatureToggleDecorator<TRequest, TResponse>
        : DecoratorBase<IRequestHandler<TRequest, TResponse>>, IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IFeatureService _featureService;
        private readonly HandlerMetadataService _handlerMetadataService;

        public FeatureToggleDecorator(IRequestHandler<TRequest, TResponse> decoratee, IFeatureService featureService, HandlerMetadataService handlerMetadataService)
            : base(decoratee)
        {
            _featureService = featureService;
            _handlerMetadataService = handlerMetadataService;
        }

        public async Task<Either<Error, TResponse>> Handle(TRequest request)
        {
            var isEnabled = await GetFeatureInfo(RootInstance)
                .MapAsync(async fi => await _featureService.IsEnabledAsync(fi.Action, fi.Context));

            return await isEnabled.MatchAsync<Either<Error, TResponse>>(
                Left: e => e,
                RightAsync: async enabled => enabled
                        ? await Decoratee.Handle(request)
                        : Error.FeatureDisabled());
        }


        private Either<Error, (string Context, string Action)> GetFeatureInfo(
            IRequestHandler<TRequest, TResponse> handler)
        {
            return _handlerMetadataService.GetHandlerMetadata(handler)
                .Map(m => (m.Context, m.Action));
        }
    }
}
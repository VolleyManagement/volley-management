namespace VolleyManagement.UnitTests.Services.RequestsService
{
    using System.Diagnostics.CodeAnalysis;
    using VolleyManagement.Domain.RequestsAggregate;

    /// <summary>
    /// Request domain object builder
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class RequestBuilder
    {
        private Request _request;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestBuilder"/> class
        /// </summary>
        public RequestBuilder()
        {
            _request = new Request
            {
                Id = 1,
                UserId = 1,
                PlayerId = 1
            };
        }

        /// <summary>
        /// Sets request test Id
        /// </summary>
        /// <param name="id">Test request Id</param>
        /// <returns>Request builder object</returns>
        public RequestBuilder WithId(int id)
        {
            _request.Id = id;
            return this;
        }

        /// <summary>
        /// Sets request test user's Id
        /// </summary>
        /// <param name="userId">Test user's Id</param>
        /// <returns>Request builder object</returns>
        public RequestBuilder WithUserId(int userId)
        {
            _request.UserId = userId;
            return this;
        }

        /// <summary>
        /// Sets request test player's Id
        /// </summary>
        /// <param name="playerId">Test player's Id</param>
        /// <returns>Request builder object</returns>
        public RequestBuilder WithPlayerId(int playerId)
        {
            _request.PlayerId = playerId;
            return this;
        }

        /// <summary>
        /// Builds test request
        /// </summary>
        /// <returns>Test request</returns>
        public Request Build()
        {
            return _request;
        }
    }
}

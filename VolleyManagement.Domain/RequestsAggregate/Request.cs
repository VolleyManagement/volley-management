namespace VolleyManagement.Domain.RequestsAggregate
{
    using System;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Request domain class.
    /// </summary>
    public class Request
    {
        private int _userId;
        private int _playerId;

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of request.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets user's Id.
        /// </summary>
        /// <value>Id of user.</value>
        public int UserId
        {
            get
            {
                return this._userId;
            }

            set
            {
                if (RequestValidation.ValidateUserId(value))
                {
                    throw new ArgumentException(Resources.ValidationUserId);
                }

                this._userId = value;
            }
        }

        /// <summary>
        /// Gets or sets player's Id.
        /// </summary>
        /// <value>Id of player.</value>
        public int PlayerId
        {
            get
            {
                return this._playerId;
            }

            set
            {
                if (RequestValidation.ValidatePlayerId(value))
                {
                    throw new ArgumentException(Resources.ValidationPlayerId);
                }

                this._playerId = value;
            }
        }
    }
}

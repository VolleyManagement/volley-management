namespace VolleyManagement.Dal.Contracts
{
    /// <summary>
    /// Container for constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Constant defined message if Entity's id is invalid
        /// </summary>
        public const string NOT_SUPPORTED_ID_MESSAGE = "Id is invalid for this Entity";

        /// <summary>
        /// Constant defined message if Entity with request id does not exist
        /// </summary>
        public const string DOESNT_EXIST_ID_MESSAGE = "Entity with request Id does not exist";

        /// <summary>
        /// Constant defined key for Entity's id
        /// </summary>
        public const string ENTITY_ID_KEY = "EntityId";
    }
}

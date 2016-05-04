namespace VolleyManagement.Domain.RolesAggregate
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Contains all operations under authorization control
    /// </summary>
    public static class AuthOperations
    {
        #region Constants

        private const byte TOURNAMENTS = 0x01;

        #endregion

        /// <summary>
        /// Contains tournaments operations
        /// </summary>
        public static class Tournaments
        {
            /// <summary>
            /// Create tournament operation
            /// </summary>
            public static readonly AuthOperation Create = Tuple.Create(TOURNAMENTS, 1);

            /// <summary>
            /// Edit tournament operation
            /// </summary>
            public static readonly AuthOperation Edit = Tuple.Create(TOURNAMENTS, 2);

            /// <summary>
            /// Delete tournament operation
            /// </summary>
            public static readonly AuthOperation Delete = Tuple.Create(TOURNAMENTS, 3);

            /// <summary>
            /// Manage tournament teams operation
            /// </summary>
            public static readonly AuthOperation ManageTeams = Tuple.Create(TOURNAMENTS, 4);
        }
    }
}

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
        private const byte TEAMS = 0x02;

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
            /// List of possible operations with tournaments
            /// </summary>
            public static readonly List<AuthOperation> TournamentOperations = new List<AuthOperation>
            {
                Create,
                Edit,
                Delete
            };
        }

        /// <summary>
        /// Contains teams operations
        /// </summary>
        public static class Teams
        {
            /// <summary>
            /// Create team operation
            /// </summary>
            public static readonly AuthOperation Create = Tuple.Create(TEAMS, 1);

            /// <summary>
            /// List of possible operations with teams
            /// </summary>
            public static readonly AuthOperation Edit = Tuple.Create(TEAMS, 2);

            /// <summary>
            /// Delete team operation
            /// </summary>
            public static readonly AuthOperation Delete = Tuple.Create(TEAMS, 3);

            /// <summary>
            /// list of team operatations
            /// </summary>
            public static readonly List<AuthOperation> TeamOperations = new List<AuthOperation>
            {
                Create,
                Edit,
                Delete
            };
        }
    }
}

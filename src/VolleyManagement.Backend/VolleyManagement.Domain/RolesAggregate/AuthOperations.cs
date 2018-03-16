﻿namespace VolleyManagement.Domain.RolesAggregate
{
    using System;

    /// <summary>
    /// Contains all operations under authorization control
    /// </summary>
    public static class AuthOperations
    {
        #region Constants

        private const byte TOURNAMENTS = 0x01;
        private const byte TEAMS = 0x02;
        private const byte GAMES = 0x03;
        private const byte PLAYERS = 0x04;
        private const byte ADMINDASHBOARD = 0x05;
        private const byte USERS = 0x06;
        private const byte FEEDBACKS = 0x07;
        private const byte REQUESTS = 0x08;
        private const byte TOURNAMENTREQUESTS = 0x09;

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

            /// <summary>
            /// See archived tournaments list operation
            /// </summary>
            public static readonly AuthOperation ViewArchived = Tuple.Create(TOURNAMENTS, 5);

            /// <summary>
            /// Archive Tournament operation
            /// </summary>
            public static readonly AuthOperation Archive = Tuple.Create(TOURNAMENTS, 6);
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
        }

        /// <summary>
        /// Contains games operations
        /// </summary>
        public static class Games
        {
            /// <summary>
            /// Create game operation
            /// </summary>
            public static readonly AuthOperation Create = Tuple.Create(GAMES, 1);

            /// <summary>
            /// Edit game operation
            /// </summary>
            public static readonly AuthOperation Edit = Tuple.Create(GAMES, 2);

            /// <summary>
            /// Delete game from tournament operation
            /// </summary>
            public static readonly AuthOperation Delete = Tuple.Create(GAMES, 3);

            /// <summary>
            /// Swap rounds in tournament operation
            /// </summary>
            public static readonly AuthOperation SwapRounds = Tuple.Create(GAMES, 4);

            /// <summary>
            /// Edit game result operation
            /// </summary>
            public static readonly AuthOperation EditResult = Tuple.Create(GAMES, 5);
        }

        /// <summary>
        /// Contains players operations
        /// </summary>
        public static class Players
        {
            /// <summary>
            /// Create player operation
            /// </summary>
            public static readonly AuthOperation Create = Tuple.Create(PLAYERS, 1);

            /// <summary>
            /// Edit player operation
            /// </summary>
            public static readonly AuthOperation Edit = Tuple.Create(PLAYERS, 2);

            /// <summary>
            /// Delete player operation
            /// </summary>
            public static readonly AuthOperation Delete = Tuple.Create(PLAYERS, 3);
        }

        /// <summary>
        /// Contains feedbacks operations
        /// </summary>
        public static class Feedbacks
        {
            /// <summary>
            /// Create feedback operation
            /// </summary>
            public static readonly AuthOperation Create = Tuple.Create(FEEDBACKS, 1);

            /// <summary>
            /// Edit feedback operation
            /// </summary>
            public static readonly AuthOperation Edit = Tuple.Create(FEEDBACKS, 2);

            /// <summary>
            /// Delete feedback operation
            /// </summary>
            public static readonly AuthOperation Delete = Tuple.Create(FEEDBACKS, 3);

            /// <summary>
            /// Read feedback
            /// </summary>
            public static readonly AuthOperation Read = Tuple.Create(FEEDBACKS, 4);

            /// <summary>
            /// Answer on feedback
            /// </summary>
            public static readonly AuthOperation Reply = Tuple.Create(FEEDBACKS, 5);

            /// <summary>
            /// Close feedback
            /// </summary>
            public static readonly AuthOperation Close = Tuple.Create(FEEDBACKS, 6);
        }

        /// <summary>
        /// Contains administrators operations
        /// </summary>
        public static class AdminDashboard
        {
            /// <summary>
            /// View admin page operation
            /// </summary>
            public static readonly AuthOperation View = Tuple.Create(ADMINDASHBOARD, 1);
        }

        /// <summary>
        /// Contains administrators operations
        /// </summary>
        public static class AllUsers
        {
            /// <summary>
            /// View all users page operation
            /// </summary>
            public static readonly AuthOperation ViewList = Tuple.Create(USERS, 1);

            /// <summary>
            /// View all users page operation
            /// </summary>
            public static readonly AuthOperation ViewDetails = Tuple.Create(USERS, 2);

            /// <summary>
            /// View all users page operation
            /// </summary>
            public static readonly AuthOperation ViewActiveList = Tuple.Create(USERS, 3);
        }

        /// <summary>
        /// Contains tournament requests operations
        /// </summary>
        public static class TournamentRequests
        {
            /// <summary>
            /// View all tournament requests page operation
            /// </summary>
            public static readonly AuthOperation ViewList = Tuple.Create(TOURNAMENTREQUESTS, 1);

            /// <summary>
            /// Confirm tournament request
            /// </summary>
            public static readonly AuthOperation Confirm = Tuple.Create(TOURNAMENTREQUESTS, 2);

            /// <summary>
            /// Decline tournament request
            /// </summary>
            public static readonly AuthOperation Decline = Tuple.Create(TOURNAMENTREQUESTS, 3);
        }

        /// <summary>
        /// Contains requests operations
        /// </summary>
        public static class Requests
        {
            /// <summary>
            /// View all requests page operation
            /// </summary>
            public static readonly AuthOperation ViewList = Tuple.Create(REQUESTS, 1);

            /// <summary>
            /// Confirm request
            /// </summary>
            public static readonly AuthOperation Confirm = Tuple.Create(REQUESTS, 2);

            /// <summary>
            /// Decline request
            /// </summary>
            public static readonly AuthOperation Decline = Tuple.Create(REQUESTS, 3);
        }
    }
}

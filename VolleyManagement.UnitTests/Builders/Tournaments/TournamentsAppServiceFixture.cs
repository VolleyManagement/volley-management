namespace VolleyManagement.UnitTests.Builders.Tournaments
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Class for generating test data
    /// </summary>
    internal class TournamentsAppServiceFixture
    {
        /// <summary>
        /// Holds collection of tournaments
        /// </summary>
        private IList<Tournament> _tournaments = new List<Tournament>();

        /// <summary>
        /// Adds tournaments to collection
        /// </summary>
        /// <returns>Builder object with collection of tournaments</returns>
        public TournamentsAppServiceFixture TestTournaments()
        {
            _tournaments.Add(new Tournament()
                {
                    Id = 1,
                    Name = "Tournament 1",
                    Description = "Tournament 1 description",
                    Season = "2014/2015",
                    Scheme = "1",
                    RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('1')"
                });
            _tournaments.Add(new Tournament()
                {
                    Id = 2,
                    Name = "Tournament 2",
                    Description = "Tournament 2 description",
                    Season = "2014/2015",
                    Scheme = "2",
                    RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('2')"
                });
            _tournaments.Add(new Tournament()
                {
                    Id = 3,
                    Name = "Tournament 3",
                    Description = "Tournament 3 description",
                    Season = "2013/2014",
                    Scheme = "2.5",
                    RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('3')"
                });
            return this;
        }

        /// <summary>
        /// Builds test data
        /// </summary>
        /// <returns>Tournament collection</returns>
        public IList<Tournament> Build()
        {
            return _tournaments;
        }
    }
}

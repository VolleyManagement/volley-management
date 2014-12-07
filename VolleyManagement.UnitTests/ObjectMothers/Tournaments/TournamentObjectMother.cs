namespace VolleyManagement.UnitTests.ObjectMothers.Tournaments
{
    using System.Collections.Generic;
    using VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Tournament object mother class.
    /// </summary>
    public static class TournamentObjectMother
    {
        /// <summary>
        /// Creates tournament objects.
        /// </summary>
        /// <returns>Tournament objects.</returns>
        public static IEnumerable<Tournament> CreateTournaments()
        {
            yield return new Tournament()
            {
                Id = 1,
                Name = "Tournament 1",
                Description = "Tournament 1 description",
                Season = "2014/2015",
                Scheme = "1",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('1')"
            };
            yield return new Tournament()
            {
                Id = 2,
                Name = "Tournament 2",
                Description = "Tournament 2 description",
                Season = "2014/2015",
                Scheme = "2",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('2')"
            };
            yield return new Tournament()
            {
                Id = 3,
                Name = "Tournament 3",
                Description = "Tournament 3 description",
                Season = "2013/2014",
                Scheme = "2.5",
                RegulationsLink = "www.Volleyball.dp.ua/Regulations/Tournaments('3')"
            };
        }
    }
}

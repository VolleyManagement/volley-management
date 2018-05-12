using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VolleyManagement.Domain.TeamsAggregate;

namespace VolleyManagement.Domain.PlayersAggregate
{
    public class FreePlayerDto
    {
        /// <summary>
        /// Gets or sets first name of player
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of player
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets PlayerId
        /// </summary>
        public PlayerId Player_Id { get; set; }


        /// <summary>
        /// Gets or sets PlayerId
        /// </summary>
        public TeamId Team_Id { get; set; }

    }
}

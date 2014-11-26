// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomainToDal.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Defines IRepository contract.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SoftServe.VolleyManagement.Dal.MsSql.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Domain = VolleyManagement.Domain.Tournaments;
    using Dal = VolleyManagement.Dal.MsSql;
   
    public static class DomainToDal
    {
        public static Dal.Tournament GetTourament(Domain.Tournament _tournament)
        {
            Dal.Tournament tournament = new Dal.Tournament();     
            tournament.Id = to
            return tournament;
        }
    }
}

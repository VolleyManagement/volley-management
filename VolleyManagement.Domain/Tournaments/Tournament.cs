// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Tournament.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Defines the TournamentDto type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SoftServe.VolleyManagement.Domain.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// TournamentDto class.
    /// </summary>
    public class Tournament
    {

        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of tournament.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        [Required(ErrorMessage = "Введите название турнира")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        [Required(ErrorMessage = "Введите название сезона")]
        public string Season { get; set; }
    }
}

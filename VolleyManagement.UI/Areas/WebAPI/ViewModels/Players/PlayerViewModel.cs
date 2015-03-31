using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VolleyManagement.Domain.Players;

namespace VolleyManagement.UI.Areas.WebApi.ViewModels.Players
{
    public class PlayerViewModel
    {
        /// <summary>
        /// Maps domain entity to presentation
        /// </summary>
        /// <param name="player"> Domain object </param>
        /// <returns> View model object </returns>
        /// IT ISN`T MY TASK - IT IS ONLY DUMMY
        internal static PlayerViewModel Map(Player player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps presentation entity to domain
        /// </summary>
        /// <returns> Domain object </returns>
        /// IT ISN`T MY TASK - IT IS ONLY DUMMY
        public Player ToDomain()
        {
            throw new NotImplementedException();
        }
    }
}
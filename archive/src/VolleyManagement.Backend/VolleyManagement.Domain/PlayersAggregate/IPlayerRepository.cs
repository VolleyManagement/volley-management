﻿namespace VolleyManagement.Domain.PlayersAggregate
{
    using Data.Contracts;
    
    /// <summary>
    /// Defines specific contract for PlayerRepository
    /// </summary>
    public interface IPlayerRepository 
    {
        /// <summary>
        /// Adds the new player to the store.
        /// </summary>
        /// <param name="newEntity">Element to add.</param>
        Player Add(CreatePlayerDto playerDto);

        /// <summary>
        /// Update the player to the DB.
        /// </summary>
        /// <param name="updatedEntity">Updated element.</param>
        void Update(Player updatedEntity);

        /// <summary>
        /// Deletes the element by id from the store.
        /// </summary>
        /// <param name="id">The id of element to remove.</param>
        void Remove(int id);
    }
}

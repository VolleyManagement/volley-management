// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="SoftServe">
//   Copyright (c) SoftServe. All rights reserved.
// </copyright>
// <summary>
//   Defines IRepository contract.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SoftServe.VolleyManagement.Dal.Contracts
{
    using System;
    using SoftServe.VolleyManagement.Domain.Tournaments;

    /// <summary>
    /// Describe methods to work with the store.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the Tournament repository.
        /// </summary>
        IRepository<Tournament> Tournaments { get; }

        /// <summary>
        /// Commits all the changes the store.
        /// </summary>
        void Commit();
    }
}

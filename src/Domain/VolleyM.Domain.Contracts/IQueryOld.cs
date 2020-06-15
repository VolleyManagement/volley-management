﻿using System;
using System.Threading.Tasks;
using LanguageExt;

namespace VolleyM.Domain.Contracts
{
	/// <summary>
	/// Represents Query object to be executed against persistence layer
	/// </summary>
	/// <typeparam name="TParam">Query parameters</typeparam>
	/// <typeparam name="TResult">Query result</typeparam>
	[Obsolete]
	public interface IQueryOld<in TParam, TResult>
        where TResult : class
    {
        Task<Either<Error, TResult>> Execute(TParam param);
    }
}
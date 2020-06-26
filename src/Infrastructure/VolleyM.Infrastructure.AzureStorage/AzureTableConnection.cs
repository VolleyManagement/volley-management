using System;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Azure.Cosmos.Table;
using Serilog;
using VolleyM.Domain.Contracts;

namespace VolleyM.Infrastructure.AzureStorage
{
	/// <summary>
	/// Base class for all classes that need access to Azure Storage
	/// </summary>
	public abstract class AzureTableConnection
	{
		private readonly AzureTableStorageOptions _options;

		protected AzureTableConnection(AzureTableStorageOptions options)
		{
			_options = options;
		}

		protected Either<Error, CloudTableClient> OpenConnection()
		{
			if (!CloudStorageAccount.TryParse(_options.ConnectionString, out CloudStorageAccount account))
			{
				Log.Error("Azure Storage connection failed. Connection string is invalid");
				return Error.InternalError("Azure Storage account connection is invalid.");
			}

			return account.CreateCloudTableClient();
		}

		protected static Error MapStorageError(LanguageExt.Common.Error storageError)
		{
			return storageError
				.Exception
				.Match<Error>(
					TranslateException,
					() => Error.InternalError(storageError.Message));
		}

		private static Error TranslateException(Exception e)
		{
			return e switch
			{
				StorageException { Message: "Conflict" } => Error.Conflict(),
				StorageException stEx => Error.InternalError($"Azure Storage Error: {stEx.Message}"),
				_ => Error.InternalError($"Unknown Error: {e.Message}")
			};
		}

		protected EitherAsync<Error, T> PerformStorageOperation<T>(string tableName, Func<CloudTable, EitherAsync<Error, T>> operation, string operationName)
		{
			var conn = OpenConnection();

			try
			{
				var table = conn.Map(c => c.GetTableReference(tableName));
				return table.Match(
					t => TryOperation(operation(t)),
					e => (EitherAsync<Error, T>)e);
			}
			catch (Exception e)
			{
				Log.Error(e, "{AzureStorageOperation} Azure Storage operation failed.", operationName);
				return Error.InternalError($"{operationName} Azure Storage operation failed.");
			}
		}

		private EitherAsync<Error, T> TryOperation<T>(EitherAsync<Error, T> operationResult)
		{
			return Prelude.TryAsync(operationResult.ToEither())
				.ToEither(MapStorageError)
				.Match<Either<Error, T>>(
					Right: r => r,
					Left: l => l)
				.ToAsync();
		}
	}
}
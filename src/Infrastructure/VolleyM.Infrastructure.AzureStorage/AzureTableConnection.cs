using System;
using System.Collections.Generic;
using System.Linq;
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
					operation,
					e => (EitherAsync<Error, T>)e);
			}
			catch (Exception e)
			{
				Log.Error(e, "{AzureStorageOperation} Azure Storage operation failed.", operationName);
				return Error.InternalError($"{operationName} Azure Storage operation failed.");
			}
		}

		[Obsolete]
		protected async Task<Either<Error, T>> PerformStorageOperationOld<T>(string tableName, Func<CloudTable, Task<Either<Error, T>>> operation, string operationName)
		{
			var conn = OpenConnection();

			try
			{
				var table = conn.Map(c => c.GetTableReference(tableName));
				return await table.Match(
					operation,
					e => Task.FromResult((Either<Error, T>)e));
			}
			catch (StorageException e) when (IsConflictError(e))
			{
				return Error.Conflict();
			}
			catch (StorageException e)
			{
				Log.Error(e, "{AzureStorageOperation} Azure Storage operation failed.", operationName);
				return Error.InternalError($"{operationName} Azure Storage operation failed.");
			}
		}

		private static bool IsConflictError(StorageException e) =>
			string.Compare("Conflict", e.Message, StringComparison.OrdinalIgnoreCase) == 0;

		public EitherAsync<Error, Unit> ConfigureTables()
		{
			var conn = OpenConnection();

			return conn.MapAsync(async client =>
				{
					var tables = GetTablesForContext();

					var createTasks = Enumerable.Select<string, Task<bool>>(tables, table =>
					{
						var tableRef = client.GetTableReference(table);
						return tableRef.CreateIfNotExistsAsync();
					}).ToList();

					await Task.WhenAll(createTasks);

					return Unit.Default;
				})
				.ToAsync();
		}

		public EitherAsync<Error, Unit> CleanTables()
		{
			var conn = OpenConnection();

			return conn.MapAsync(async client =>
				{
					var tables = GetTablesForContext();

					var deleteTasks = Enumerable.Select<string, Task<bool>>(tables, table =>
					{
						var tableRef = client.GetTableReference(table);
						return tableRef.DeleteIfExistsAsync();
					});

					await Task.WhenAll(deleteTasks);

					return Unit.Default;
				})
				.ToAsync();
		}

		/// <summary>
		/// When overriden should return all the tables particular configuration should be responsible for.
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable<string> GetTablesForContext();
	}
}
using System;
using VolleyM.Domain.Contracts.Crosscutting;

namespace VolleyM.Domain.Framework
{
	public class RandomIdGenerator : IRandomIdGenerator
	{
		public string GetRandomId()
		{
			return Guid.NewGuid().ToString("N").Substring(0, 20).ToLower();
		}
	}
}
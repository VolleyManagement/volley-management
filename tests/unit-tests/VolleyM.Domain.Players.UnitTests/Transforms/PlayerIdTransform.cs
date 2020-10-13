using System;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests.Transforms
{
	public class PlayerIdTransform : ISpecFlowTransform
	{
		public Type TargetType { get; } = typeof(PlayerId);

		public object GetValue(string rawValue)
		{
			if (string.IsNullOrEmpty(rawValue))
			{
				return null;
			}

			return new PlayerId(rawValue);
		}
	}
}
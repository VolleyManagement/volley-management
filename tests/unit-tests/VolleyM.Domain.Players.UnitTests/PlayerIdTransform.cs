using System;
using VolleyM.Domain.Players.PlayerAggregate;
using VolleyM.Domain.UnitTests.Framework;

namespace VolleyM.Domain.Players.UnitTests
{
	public class PlayerIdTransform : ISpecFlowTransform
	{
		public Type TargetType { get; } = typeof(PlayerId);
		public object GetValue(string rawValue)
		{
			return new PlayerId(rawValue);
		}
	}
}
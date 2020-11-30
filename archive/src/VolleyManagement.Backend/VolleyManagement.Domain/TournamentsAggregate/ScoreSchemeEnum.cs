using System.ComponentModel;

namespace VolleyManagement.Domain.TournamentsAggregate
{
	public enum ScoreSchemeEnum
	{
		[Description("Best Of 5")]
		BestOf5 = 1,
		[Description("Best Of 3")]
		BestOf3 = 2
	}
}
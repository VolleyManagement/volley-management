using FluentValidation;

namespace VolleyM.Domain.Players.Handlers
{
	public interface IPlayerNameRequest
	{
		string FirstName { get; set; }

		string LastName { get; set; }
	}

	internal class PlayerNameValidator : AbstractValidator<IPlayerNameRequest>
	{
		public PlayerNameValidator()
		{
			RuleFor(r => r.FirstName)
				.NotEmpty()
				.MaximumLength(60);

			RuleFor(r => r.LastName)
				.NotEmpty()
				.MaximumLength(60);
		}
	}
}
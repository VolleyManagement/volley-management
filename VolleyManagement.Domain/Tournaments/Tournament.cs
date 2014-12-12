namespace VolleyManagement.Domain.Tournaments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using VolleyManagement.Domain.Properties;

    /// <summary>
    /// Tournament domain class.
    /// </summary>
    public class Tournament : IValidatableObject
    {
        /// <summary>
        /// Gets or sets a value indicating where Id.
        /// </summary>
        /// <value>Id of tournament.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Name.
        /// </summary>
        /// <value>Name of tournament.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Description.
        /// </summary>
        /// <value>Description of tournament.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Season.
        /// </summary>
        /// <value>Season of tournament.</value>
        public string Season { get; set; }

        /// <summary>
        /// Gets or sets a value indicating where Scheme.
        /// </summary>
        /// <value>Scheme of tournament.</value>
        public TournamentSchemeEnum Scheme { get; set; }

        /// <summary>
        /// Gets or sets a value indicating regulations of tournament.
        /// </summary>
        /// <value>regulations of tournament.</value>
        public string RegulationsLink { get; set; }

        /// <summary>
        /// Validates the model properties
        /// </summary>
        /// <param name="validationContext">Context of Validation</param>
        /// <returns>List of validation errors</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (string.IsNullOrEmpty(Name) && Name.Length >= 80)
            {
                errors.Add(new ValidationResult(Resources.ValidationResultName));
            }

            if (Description.Length >= 1024)
            {
                errors.Add(new ValidationResult(Resources.ValidationResultDescription));
            }

            if (string.IsNullOrEmpty(Season) && Season.Length >= 9)
            {
                errors.Add(new ValidationResult(Resources.ValidationResultSeason));
            }

            if (!Enum.IsDefined(typeof(TournamentSchemeEnum), Scheme))
            {
                errors.Add(new ValidationResult(Resources.ValidationResultScheme));
            }

            if (RegulationsLink.Length >= 1024)
            {
                errors.Add(new ValidationResult(Resources.ValidationResultRegLink));
            }
            return errors;
        }
    }
}

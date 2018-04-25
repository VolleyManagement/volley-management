using System;
using FeatureToggle.Toggles;

namespace VolleyManagement.Crosscutting.Contracts.FeatureToggles.Core
{
    using FeatureToggle.Core;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Describes whether we are deploying on IIS/Local environment vs Azure
    /// </summary>
    public class IisDeployment : IFeatureToggle
    {
        private const string FEATURE = "FeatureToggle.IisDeployment";

        public bool FeatureEnabled
        {
            get
            {
                var builder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", true).Build();
                var featureValue = builder[$"AppSettings:{FEATURE}"];
                return !string.IsNullOrWhiteSpace(featureValue) && bool.Parse(featureValue);
            }
        }
    }

}
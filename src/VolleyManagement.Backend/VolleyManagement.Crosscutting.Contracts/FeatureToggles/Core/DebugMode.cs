using System;

namespace VolleyManagement.Crosscutting.Contracts.FeatureToggles.Core
{
    using FeatureToggle.Core;
    using Microsoft.Extensions.Configuration;

    public class DebugMode : IFeatureToggle
    {
        private const string FEATURE = "FeatureToggle.DebugMode";

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
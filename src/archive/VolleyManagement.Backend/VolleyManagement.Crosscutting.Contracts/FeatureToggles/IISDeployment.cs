namespace VolleyManagement.Crosscutting.Contracts.FeatureToggles
{
    using FeatureToggle.Toggles;

    /// <summary>
    /// Describes whether we are deploying on IIS/Local environment vs Azure
    /// </summary>
    public class IisDeployment : SimpleFeatureToggle
    {
    }
}
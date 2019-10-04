namespace VolleyM.Architecture.UnitTests
{
    public static class PackageNamingConstants
    {
        internal static string SYSTEM_NS = "System";

        internal const string ROOT_NS = "VolleyM";

        internal const string API_NS = "API";
        internal const string DOMAIN_NS = "Domain";
        internal const string INFRASTRUCTURE_NS = "Infrastructure";

        internal const string SIMPLE_INJECTOR_NS = "SimpleInjector";
        internal const string AUTOMAPPER_NS = "AutoMapper";
        internal const string SERILOG_NS = "Serilog";

        internal static readonly string[] AllowedLayers = {
            DOMAIN_NS,
            INFRASTRUCTURE_NS,
            API_NS
        };

        internal static readonly string[] BoundedContexts = {
            "Contributors",
            "Teams",
            "Players",
            "Tournaments",
            "TournamentCalendar",
            "IdentityAndAccess"
        };

        internal static readonly string[] AllowedDomainPackages = {
            "Contracts"
        };

        internal static readonly string[] InfrastructureServices = {
            "AzureStorage",
            "Hardcoded",
            //Not service but allowed
            "Bootstrap"
        };

        internal static readonly string[] AllowedMicrosoftReferences = {
            "Microsoft.CodeAnalysis.LiveUnitTesting.Runtime"
        };
    }
}
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
        internal const string LANGUAGE_EXT = "LanguageExt.Core";

        internal static readonly string[] AllowedLayers = {
            DOMAIN_NS,
            INFRASTRUCTURE_NS,
            API_NS,
            "Tools"
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
            "Contracts",
            "Framework"
        };

        internal static readonly string[] InfrastructureServices = {
            "AzureStorage",
            "Hardcoded",
            //Not service but allowed
            "Bootstrap"
        };
        
        internal static readonly string[] ApiServices = {
            "Contracts"
        };

        internal static readonly string[] AllowedMicrosoftReferences = {
            "Microsoft.CodeAnalysis.LiveUnitTesting.Runtime",
            "Microsoft.Extensions.Configuration.Abstractions"
        };

        internal static readonly string[] AllowedLoggerReferences = {
            "Serilog",
            "Destructurama.Attributed"
        };
    }
}
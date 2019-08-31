namespace VolleyM.Architecture.UnitTests
{
    public static class PackageNamingConstants
    {
        internal static string SYSTEM_NS = "System";

        internal const string ROOT_NS = "VolleyM";

        internal const string DOMAIN_NS = "Domain";
        internal const string API_NS = "API";

        internal const string SIMPLE_INJECTOR_NS = "SimpleInjector";
        internal const string MEDIATR_NS = "MediatR";
        internal const string AUTOMAPPER_NS = "AutoMapper";

        internal static readonly string[] AllowedLayers = {
            DOMAIN_NS,
            "Infrastructure",
            API_NS
        };

        internal static readonly string[] BoundedContexts = {
            "Contributors",
            "Teams",
            "Players",
            "Tournaments",
            "TournamentCalendar",
            //Not context but allowed
            "Contracts"
        };

        internal static readonly string[] AllowedMicrosoftReferences = {
            "Microsoft.AspNetCore",
            "Microsoft.Extensions"
        };
    }
}
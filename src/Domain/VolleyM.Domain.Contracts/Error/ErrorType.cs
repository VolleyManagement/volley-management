namespace VolleyM.Domain.Contracts
{
    public enum ErrorType
    {
        Unknown = 0,
        Conflict = 1,
        NotFound = 2,
        InternalError = 3,
        NotAuthorized = 4,
        NotAuthenticated = 5,
        ValidationFailed = 6,
        FeatureDisabled = 7,
        DesignViolation = 8,//internal framework error, usually should happen during development only
    }
}
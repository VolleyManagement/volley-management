namespace VolleyM.Domain.Framework.Authorization
{
    public interface ICurrentUserManager
    {
        CurrentUserContext Context { get; set; }

        CurrentUserScope BeginScope(CurrentUserContext userScope);
    }
}
namespace VolleyM.Domain.Framework.Authorization
{
    public interface ICurrentUserManager
    {
        CurrentUserContext Context { get; set; }
    }
}
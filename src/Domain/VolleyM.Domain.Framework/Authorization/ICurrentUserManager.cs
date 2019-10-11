namespace VolleyM.Domain.Framework.Authorization
{
    public interface ICurrentUserManager
    {
        void SetCurrentContext(CurrentUserContext context);
    }
}
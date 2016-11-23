namespace VolleyManagement.UI
{
    using System;
    using System.Web;
    using VolleyManagement.Contracts;
    using VolleyManagement.Contracts.Authorization;
    using VolleyManagement.Domain.UsersAggregate;

    public class UserModule : IHttpModule
    {
        private const int ANONYM = -1;
        ICurrentUserService _currentUserService;
        IUserService _userService;

        public UserModule(ICurrentUserService currentUser, IUserService userService)
        {
            this._userService = userService;
            _currentUserService = currentUser;
        }

        public string ModuleName
        {
            get { return "UserModule"; }
        }

        // In the Init function, register for HttpApplication 
        // events by adding your handlers.
        public void Init(HttpApplication application)
        {
            application.EndRequest +=
                (new EventHandler(this.Application_BeginRequest));
        }

        private void Application_BeginRequest(Object source, EventArgs e)
        {
            int userId = this._currentUserService.GetCurrentUserId();
            if (userId != ANONYM)
            {
                try
                {
                    User u = this._userService.GetUser(userId);
                    if (u.IsBlocked)
                    {
                        HttpApplication application = (HttpApplication)source;
                        HttpContext context = application.Context;
                        context.GetOwinContext().Authentication.SignOut();

                        //maybe unnessesary
                        userId = ANONYM;
                        context.Response.Redirect(context.Request.RawUrl);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        public void Dispose()
        {

        }
    }
}
namespace VolleyManagement.Services
{
    using System.Web;
    using VolleyManagement.Contracts;

    public class HttpContextService : IHttpContextService
    {
        public HttpRequest Request
        {
            get
            {
                return HttpContext.Current.Request;
            }
        }
    }
}

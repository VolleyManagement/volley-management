namespace VolleyManagement.Contracts
{
    using System.Web;

    public interface IHttpContextService
    {
        HttpRequest Request { get; }
    }
}

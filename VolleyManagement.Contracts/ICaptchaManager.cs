namespace VolleyManagement.Contracts
{
    using System.Web;

    /// <summary>
    /// Defines a contract for CaptchaManager
    /// </summary>
    public interface ICaptchaManager
    {
        /// <summary>
        /// Method, that give result: captcha passed or not
        /// </summary>
        /// <param name="request">Request to the browser</param>
        /// <returns>Captcha result</returns>
        bool IsFormSubmit(HttpRequestBase request);
    }
}

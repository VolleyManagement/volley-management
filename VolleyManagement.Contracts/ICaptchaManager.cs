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
        /// <param name="response">Response of Captcha.</param>
        /// <returns>Captcha result</returns>
        bool IsFormSubmit(string response);
    }
}

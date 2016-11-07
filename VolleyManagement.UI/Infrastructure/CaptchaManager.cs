namespace VolleyManagement.UI.Infrastructure
{
    using System.Net;
    using System.Web;
    using System.Web.Configuration;
    using Newtonsoft.Json.Linq;
    using VolleyManagement.Contracts;

    /// <summary>
    /// Represents an Implementation of ICaptchaManager interface.
    /// </summary>
    public class CaptchaManager : ICaptchaManager
    {
        /// <summary>
        /// Method, that verifies if captcha is valid 
        /// </summary>
        /// <param name="request">Get request from browser</param>
        /// <returns>Captcha result</returns>
        public bool IsFormSubmit(HttpRequestBase request)
        {
            const string SECRET_KEY = "CaptchaSecret";
            var response = request["g-recaptcha-response"];
            string secretKey = WebConfigurationManager.AppSettings[SECRET_KEY];            
            var client = new WebClient();
            var result = client.DownloadString(
                string.Format(
                    "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                    secretKey,
                    response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            return status;
        }
    }
}
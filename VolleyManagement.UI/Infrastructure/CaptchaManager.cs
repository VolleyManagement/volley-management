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
            bool status = false;
            const string SECRET_KEY = "RecaptchaSecretKey";
            var response = request["g-recaptcha-response"];
            string secretKey = WebConfigurationManager.AppSettings[SECRET_KEY];
            using (var client = new WebClient())
            {
                var result = client.DownloadString(
                    string.Format(
                        "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                        secretKey,
                        response));
                var obj = JObject.Parse(result);
                status = (bool)obj.SelectToken("success");
            }

            return status;
        }
    }
}
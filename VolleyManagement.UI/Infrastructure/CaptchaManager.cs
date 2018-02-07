namespace VolleyManagement.UI.Infrastructure
{
    using System.Net;
    using System.Web;
    using System.Web.Configuration;
    using Contracts;
    using Newtonsoft.Json.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System;

    /// <summary>
    /// Represents an Implementation of ICaptchaManager interface.
    /// </summary>
    public class CaptchaManager : ICaptchaManager
    {
        private const string CONFIG_KEY_RECAPTCHA_SECERET = "RecaptchaSecretKey";
        private const string RECAPTCHA_VERIFY_URL_FORMAT = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
        private const string RECAPTCHA_JSON_TOKEN_SUCCESS = "success";
        
        /// <summary>
        /// Method, that verifies if captcha is valid
        /// </summary>
        /// <param name="userResponseToken">Response of Captcha.</param>
        /// <returns>Captcha result</returns>
        public async Task<bool> ValidateUserCaptcha(string userResponseToken)
        {
            var isCaptchaValid = false;
            using (var client = new HttpClient())
            {
                try
                {
                    var jsonResultStr = await client.GetStringAsync(GetCapchaRequestUrl(userResponseToken));

                    var jsonResultObj = JObject.Parse(jsonResultStr);
                    isCaptchaValid = jsonResultObj[RECAPTCHA_JSON_TOKEN_SUCCESS].Value<bool>();
                }               
                catch (HttpRequestException httpException)
                {
                    //Means that there are issues with network. LOGGING REQUIRED
                    isCaptchaValid = false;
                }
                catch (ArgumentNullException argumentNullException)
                {
                    //Means that response from Google is not JSON. LOGGING REQUIRED
                    isCaptchaValid = false;
                }
            }

            return isCaptchaValid;
        }

        private string GetCapchaRequestUrl(string userResponseToken)
        {
            string secretKey = WebConfigurationManager.AppSettings[CONFIG_KEY_RECAPTCHA_SECERET];
            return string.Format(RECAPTCHA_VERIFY_URL_FORMAT,
                                 secretKey,
                                 userResponseToken);
        }
    }
}
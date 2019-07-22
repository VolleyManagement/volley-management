namespace VolleyManagement.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a contract for CaptchaManager
    /// </summary>
    public interface ICaptchaManager
    {
        /// <summary>
        /// Method, that verifies if captcha is valid
        /// </summary>
        /// <param name="userResponseToken">Response captcha token from user.</param>
        /// <returns>Captcha result</returns>
        Task<bool> ValidateUserCaptchaAsync(string userResponseToken);
    }
}

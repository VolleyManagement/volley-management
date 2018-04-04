namespace VolleyManagement.API.Model
{
    /// <summary>
    /// Contains user info provided by google auth
    /// </summary>
    public class GoogleUserInfo
    {
        public string AccessToken { get; set; }

        public string ExpiresAt { get; set; }

        public string ExpiresIn { get; set; }

        public string IdToken { get; set; }

        public string Code { get; set; }

        public string ID { get; set; }

        public string Name { get; set; }

        public string ImageURL { get; set; }

        public string Email { get; set; }
    }
}

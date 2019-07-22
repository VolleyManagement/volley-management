namespace VolleyManagement.Services.Mail
{
    using System.Net;
    using System.Web.Configuration;
    using Contracts.ExternalResources;
    using Crosscutting.Contracts.Infrastructure;
    using SendGrid.Helpers.Mail;

    public class SendGridMailService : IMailService
    {
        private const string SG_API_KEY = "SendGridApiKey";

        private readonly ILog _logger;

        public SendGridMailService(ILog logger)
        {
            _logger = logger;
        }

        public void Send(EmailMessage message)
        {
            var msg = new SendGridMessage();
            msg.AddTo(message.Recipient);
            msg.From = new EmailAddress("volleymanagement.test@gmail.com", "Volley Management System");
            msg.Subject = message.Subject;
            msg.PlainTextContent = message.Body;

            var web = new SendGrid.SendGridClient(GetApiKey());
            var response = web.SendEmailAsync(msg);
            response.Wait();
            if (response.Result.StatusCode != HttpStatusCode.OK)
            {
                _logger.Write(LogLevelEnum.Error, $"Failed to send SendGrid email. Response was: ${response.Result}");
            }
        }

        private static string GetApiKey()
        {
            return WebConfigurationManager.AppSettings[SG_API_KEY];
        }
    }
}
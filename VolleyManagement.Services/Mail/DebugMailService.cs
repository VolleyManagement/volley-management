namespace VolleyManagement.Services.Mail
{
    using System.Diagnostics;
    using Contracts.ExternalResources;
    public class DebugMailService : IMailService
    {
        public void Send(EmailMessage message)
        {
            Trace.WriteLine($"To:{message.Recipient}, Subject:{message.Subject}, Body:{message.Body}");
        }
    }
}
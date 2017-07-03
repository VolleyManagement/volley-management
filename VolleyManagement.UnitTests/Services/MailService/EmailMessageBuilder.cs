namespace VolleyManagement.UnitTests.Services.MailService
{
    using System.Diagnostics.CodeAnalysis;
    using Crosscutting.Contracts.MailService;

    /// <summary>
    /// Represents a builder of <see cref="EmailMessage"/> objects for unit
    /// tests for <see cref="FeedbackService"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class EmailMessageBuilder
    {
        private readonly EmailMessage _emailMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailService.EmailMessageBuilder"/> class.
        /// </summary>
        public EmailMessageBuilder()
        {
            _emailMessage = new EmailMessage(
                recipient: "example2@gmail.com",
                subject: "Subject",
                body: "Body");
        }

        /// <summary>
        /// Builds test feedback.
        /// </summary>
        /// <returns>Test feedback</returns>
        public EmailMessage Build()
        {
            return _emailMessage;
        }
    }
}

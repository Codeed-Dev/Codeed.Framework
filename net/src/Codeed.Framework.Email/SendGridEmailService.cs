using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using Codeed.Framework.Email.Configuration;
using Codeed.Framework.Email.Models;

namespace Codeed.Framework.Email
{
    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridConfiguration _sendGridConfiguration;

        public SendGridEmailService(IOptions<SendGridConfiguration> sendGridConfiguration)
        {
            _sendGridConfiguration = sendGridConfiguration.Value;
        }

        public async Task SendTemplateEmail(TemplateEmailConfig config, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(config.To))
                throw new ArgumentNullException(nameof(config.To));

            if (string.IsNullOrEmpty(config.TemplateId))
                throw new ArgumentNullException(nameof(config.TemplateId));

            var client = new SendGridClient(_sendGridConfiguration.ApiKey);
            var from = new EmailAddress(_sendGridConfiguration.SenderEmail, _sendGridConfiguration.SenderName);
            var to = new EmailAddress(config.To);
            var email = MailHelper.CreateSingleTemplateEmail(from, to, config.TemplateId, config.Parameters);
            await client.SendEmailAsync(email, cancellationToken);
        }
    }
}

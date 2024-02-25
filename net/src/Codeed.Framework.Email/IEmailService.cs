using Codeed.Framework.Email.Models;

namespace Codeed.Framework.Email
{
    public interface IEmailService
    {
        Task SendTemplateEmail(TemplateEmailConfig config, CancellationToken cancellationToken);
    }
}

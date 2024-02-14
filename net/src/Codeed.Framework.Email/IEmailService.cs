using Scaffolding.Core.Application.Email.Models;

namespace Scaffolding.Core.Application.Email
{
    public interface IEmailService
    {
        Task SendTemplateEmail(TemplateEmailConfig config, CancellationToken cancellationToken);
    }
}

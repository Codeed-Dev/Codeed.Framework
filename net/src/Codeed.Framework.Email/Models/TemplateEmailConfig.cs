namespace Codeed.Framework.Email.Models
{
    public class TemplateEmailConfig : EmailConfig
    {
        public string TemplateId { get; set; } = "";

        public object? Parameters { get; set; }
    }
}

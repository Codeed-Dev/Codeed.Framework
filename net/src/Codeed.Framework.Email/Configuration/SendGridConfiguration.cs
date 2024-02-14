namespace Scaffolding.Core.Application.Environment
{
    public class SendGridConfiguration
    {
        public string ApiKey { get; set; } = "";

        public string SenderEmail { get; set; } = "";

        public string SenderName { get; set; } = "";
    }
}

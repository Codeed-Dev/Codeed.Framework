namespace Codeed.Framework.AspNet.RegisterServicesConfigurations.Configurations
{
    public class AzureConfiguration
    {
        public string BlobContainerConnectionString { get; set; } = "";

        public string BlobAccountName { get; set; } = "";

        public string BlobAccountKey { get; set; } = "";

        public string ApplicationInsightsConnectionKey { get; set; } = "";
    }
}

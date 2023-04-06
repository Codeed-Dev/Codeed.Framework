namespace Codeed.Framework.Domain
{
    public interface ITenantEvent : IEvent
    {
        string Tenant { get; set; }
    }
}
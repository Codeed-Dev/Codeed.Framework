namespace Codeed.Framework.Domain
{
    public interface IEntityWithTenant
    {
        string Tenant { get; }
    }
}
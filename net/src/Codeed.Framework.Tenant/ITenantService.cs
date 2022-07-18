namespace Codeed.Framework.Tenant
{
    public interface ITenantService
    {
        string Tenant { get; }

        bool Authorize(object permission);

        IEnumerable<object> GetPermissions();
    }
}
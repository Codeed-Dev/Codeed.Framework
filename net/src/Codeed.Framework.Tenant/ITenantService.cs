namespace Codeed.Framework.Tenant
{
    public interface ITenantService
    {
        string Tenant { get; }

        IEnumerable<string> GetTenants();

        bool Authorize(object permission);

        IEnumerable<object> GetPermissions();

        string GetRootUserUid();
    }
}
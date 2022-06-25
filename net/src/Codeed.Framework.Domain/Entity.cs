using System.ComponentModel.DataAnnotations;

namespace Codeed.Framework.Domain
{
    public abstract class Entity : EntityWithoutTenant, IEntityWithTenant
    {
        public string Tenant { get; set; }
    }
}

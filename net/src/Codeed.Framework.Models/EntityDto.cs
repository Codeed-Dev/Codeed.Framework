namespace Codeed.Framework.Models
{
    public abstract class EntityDto : IDto
    {
        public Guid? Id { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}

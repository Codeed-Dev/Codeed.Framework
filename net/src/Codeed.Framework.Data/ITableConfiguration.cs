using Microsoft.EntityFrameworkCore;

namespace Codeed.Framework.Data
{
    public interface ITableConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
    }
}

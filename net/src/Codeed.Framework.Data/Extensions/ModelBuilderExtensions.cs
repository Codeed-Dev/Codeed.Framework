using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codeed.Framework.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static IEnumerable<IMutableEntityType> GetEntities<T>(this ModelBuilder modelbuilder)
        {
            return typeof(T).IsInterface ?
                modelbuilder.Model.GetEntityTypes().Where(x => typeof(T).IsAssignableFrom(x.ClrType)) :
                modelbuilder.Model.GetEntityTypes().Where(x => x.ClrType.IsSubclassOf(typeof(T)));
        }
    }
}

using Codeed.Framework.Data;
using Codeed.Framework.Identity.Domain;

namespace Sample.Identity.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IQueryable<User> FindByUid(string userUid);
    }
}

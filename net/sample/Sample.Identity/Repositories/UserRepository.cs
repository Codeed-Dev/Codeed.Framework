using Codeed.Framework.Data;
using Codeed.Framework.Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Sample.Identity.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IdentityDbContext context) : base(context, context)
        {
        }

        public IQueryable<User> FindByUid(string userUid)
        {
            return QueryAll().Where(x => x.Uid == userUid);
        }
    }
}

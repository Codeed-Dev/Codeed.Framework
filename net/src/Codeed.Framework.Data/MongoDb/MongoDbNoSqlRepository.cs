using Codeed.Framework.Models;
using CodeedMeta.SharedContext.Data;
using MongoDB.Driver;
using System.Threading;
using System.Threading.Tasks;

namespace Codeed.Framework.Data.MongoDb
{
    public class MongoDbNoSqlRepository<T> : INoSqlRepository<T>
        where T : INoSqlDocument
    {
        private readonly IMongoCollection<T> _modelCollection;

        public MongoDbNoSqlRepository(IMongoDatabase mongoDatabase)
        {
            _modelCollection = mongoDatabase.GetCollection<T>(nameof(T));
        }

        public Task<T> GetAsync(string id, CancellationToken cancellationToken)
        {
            return _modelCollection.Find(u => u.Id == id)
                                   .FirstOrDefaultAsync(cancellationToken);
        }


        public Task Add(T userWorkspaceDto, CancellationToken cancellationToken)
        {
            var insertOneOptions = new InsertOneOptions();
            return _modelCollection.InsertOneAsync(userWorkspaceDto, insertOneOptions, cancellationToken);
        }


        public Task Delete(string uid, CancellationToken cancellationToken)
        {
            return _modelCollection.DeleteOneAsync(u => u.Id == uid, cancellationToken);
        }

        public Task Update(string uid, T userWorkspaceDto, CancellationToken cancellationToken)
        {
            var replaceOptions = new ReplaceOptions()
            {
            };

            return _modelCollection.ReplaceOneAsync(u => u.Id == uid, userWorkspaceDto, replaceOptions, cancellationToken);
        }

        public Task AddOrUpdate(string uid, T userWorkspaceDto, CancellationToken cancellationToken)
        {
            var replaceOptions = new ReplaceOptions()
            {
                IsUpsert = true
            };

            return _modelCollection.ReplaceOneAsync(u => u.Id == uid, userWorkspaceDto, replaceOptions, cancellationToken);
        }
    }
}

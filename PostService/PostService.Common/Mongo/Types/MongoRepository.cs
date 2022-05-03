using System.Linq.Expressions;
using MongoDB.Driver;
using PostService.Common.Types;

namespace PostService.Common.Mongo.Types;
    public class MongoRepository<TEntity>: IMongoRepository<TEntity> where TEntity : IIdentifiable
    {
        private readonly IMongoCollection<TEntity> mongoCollection;

        public MongoRepository(IMongoDatabase database, string collectionName) =>
            this.mongoCollection = database.GetCollection<TEntity>(collectionName);

        public Task AddAsync(TEntity entity) => this.mongoCollection.InsertOneAsync(entity);

        public Task RemoveAsync(TEntity entity) => this.mongoCollection.DeleteOneAsync(e => e.Id == entity.Id);

        public Task UpdateAsync(TEntity entity) => this.mongoCollection.ReplaceOneAsync(e => e.Id == entity.Id, entity);

        public Task<TEntity> FindAsync(Guid id) =>
            this.mongoCollection.FindAsync(e => e.Id == id).Result.SingleOrDefaultAsync();

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate) => this.mongoCollection.FindAsync(predicate).Result.SingleOrDefaultAsync();

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate) =>
            this.mongoCollection.FindAsync(predicate).Result.AnyAsync();
    }

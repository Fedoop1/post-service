using System.Linq.Expressions;
using PostService.Common.Types;

namespace PostService.Common.Mongo.Types;
public interface IMongoRepository<TEntity> where TEntity: IIdentifiable
{
    Task AddAsync(TEntity entity);
    Task RemoveAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task<TEntity> FindAsync(Guid id);
    Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
}

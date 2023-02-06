using System.Linq.Expressions;
using Shared.Optionals;

namespace Domain.Core.Repositories;

public interface IReadRepository<T>
{
    IQueryable<T> GetQueryable(Expression<Func<T, bool>> @where, params Expression<Func<T, object>>[] includes);
    
    Optional<T> GetSingleOrDefault(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);

    Optional<T> GetFirstOrDefault(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
    
    List<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
    
    void Create(T entity);
    void CreateMany(List<T> entities);

    void Update(T entity);
    void UpdateMany(List<T> entities);
}
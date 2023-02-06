using System.Linq.Expressions;
using Shared.Optionals;

namespace Domain.Core.Repositories;

public interface IReadRepository<T>
{
    IQueryable<T> GetQueryable(Expression<Func<T, bool>> @where, Expression<Func<T, object>>[] includes = null);
    
    Optional<T> GetSingleOrDefault(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);

    Optional<T> GetFirstOrDefault(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
    
    IQueryable<T> GetManyQueryable(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
    
    List<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
}
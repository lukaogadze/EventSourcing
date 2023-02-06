using Shared.Optionals;

namespace Domain.Core.Repositories;

public interface IWriteRepository<T>
{
    Optional<T> FindBy(Guid id);
    
    void Create(T entity);
    void CreateMany(IEnumerable<T> entities);
    
    void Update(T entity);
    void UpdateMany(IEnumerable<T> entities);
}
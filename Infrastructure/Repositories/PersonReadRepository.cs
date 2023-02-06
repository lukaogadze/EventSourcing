using System.Linq.Expressions;
using Domain.People.Models.Read;
using Domain.People.Repositories.Read;
using Microsoft.EntityFrameworkCore;
using Shared.Optionals;

namespace Infrastructure.Repositories;

public class PersonReadRepository : IPersonReadRepository
{
    private readonly EventSourcingDbContext _eventSourcingDbContext;

    public PersonReadRepository(EventSourcingDbContext eventSourcingDbContext)
    {
        _eventSourcingDbContext = eventSourcingDbContext;
    }

    public IQueryable<PersonReadModel> GetQueryable(Expression<Func<PersonReadModel, bool>> where, params Expression<Func<PersonReadModel, object>>[] includes)
    {
        var query = _eventSourcingDbContext.PersonReadModels.AsNoTracking();
        query = query.Where(where);

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query;
    }

    public Optional<PersonReadModel> GetSingleOrDefault(Expression<Func<PersonReadModel, bool>> where, params Expression<Func<PersonReadModel, object>>[] includes)
    {
        var query = GetQueryable(where, includes);

        var result = query.SingleOrDefault();

        return result == null ? Optional.Nothing<PersonReadModel>() : Optional.Something(result);
    }

    public Optional<PersonReadModel> GetFirstOrDefault(Expression<Func<PersonReadModel, bool>> where, params Expression<Func<PersonReadModel, object>>[] includes)
    {
        var query = GetQueryable(where, includes);

        var result = query.FirstOrDefault();

        return result == null ? Optional.Nothing<PersonReadModel>() : Optional.Something(result);
    }
    

    public List<PersonReadModel> GetMany(Expression<Func<PersonReadModel, bool>> where, params Expression<Func<PersonReadModel, object>>[] includes)
    {
        var query = GetQueryable(where, includes);

        return query.ToList();
    }

    public void Create(PersonReadModel entity)
    {
        _eventSourcingDbContext.PersonReadModels.Add(entity);
    }

    public void CreateMany(List<PersonReadModel> entities)
    {
        _eventSourcingDbContext.PersonReadModels.AddRange(entities);
    }

    public void Update(PersonReadModel entity)
    {
        _eventSourcingDbContext.PersonReadModels.Update(entity);
    }

    public void UpdateMany(List<PersonReadModel> entities)
    {
        _eventSourcingDbContext.PersonReadModels.UpdateRange(entities);
    }
}
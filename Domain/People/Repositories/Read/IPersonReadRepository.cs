using Domain.Core.Repositories;
using Domain.People.Models.Read;

namespace Domain.People.Repositories.Read;

public interface IPersonReadRepository : IReadRepository<PersonReadModel>
{
}
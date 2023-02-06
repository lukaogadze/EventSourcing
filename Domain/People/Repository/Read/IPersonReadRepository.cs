using Domain.Core.Repository;
using Domain.People.Models.Read;

namespace Domain.People.Repository.Read;

public interface IPersonReadRepository : IReadRepository<PersonReadModel>
{
}
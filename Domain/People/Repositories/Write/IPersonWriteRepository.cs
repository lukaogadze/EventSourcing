using Domain.Core.Repositories;
using Domain.People.Models.Write;

namespace Domain.People.Repositories.Write;

public interface IPersonWriteRepository : IWriteRepository<Person>
{
}
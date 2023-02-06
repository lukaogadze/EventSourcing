using Domain.Core.Repository;
using Domain.People.Models.Write;

namespace Domain.People.Repository.Write;

public interface IPersonWriteRepository : IWriteRepository<Person>
{
}
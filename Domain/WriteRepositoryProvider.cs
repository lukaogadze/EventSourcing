using Domain.People.Repository.Write;

namespace Domain;

public class WriteRepositoryProvider
{
    public IPersonWriteRepository People { get; }

    public WriteRepositoryProvider(IPersonWriteRepository people)
    {
        People = people;
    }
}
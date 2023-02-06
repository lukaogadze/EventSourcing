using Domain.People.Repositories.Write;

namespace Domain;

public class WriteRepositoryProvider
{
    public IPersonWriteRepository People { get; private set; }

    public WriteRepositoryProvider(IPersonWriteRepository people)
    {
        People = people;
    }
}
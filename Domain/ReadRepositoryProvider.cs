using Domain.People.Repositories.Read;

namespace Domain;

public class ReadRepositoryProvider
{
    public IPersonReadRepository People { get; private set; }

    public ReadRepositoryProvider(IPersonReadRepository people)
    {
        People = people;
    }
}
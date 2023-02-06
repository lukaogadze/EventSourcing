using Domain.People.Repository.Read;

namespace Domain;

public class ReadRepositoryProvider
{
    public IPersonReadRepository People { get; }

    public ReadRepositoryProvider(IPersonReadRepository people)
    {
        People = people;
    }
}
namespace Domain.People.Models.Read;

public class PersonReadModel
{
    public Guid Id { get; private set; }
    public DateTimeOffset CreateDate { get; set; }
    
    public string FirstName { get; private set; }
    public string MiddleName { get; private set; }
    public string LastName { get; private set; }

    public string City { get; private set; }
    public string Street { get; private set; }

    public DateTimeOffset BirthDate { get; private set; }
    public string Hobby { get; private set; }

    private PersonReadModel()
    {
    }

    public PersonReadModel(
        Guid aggregateId,
        string firstName,
        string middleName,
        string lastName,
        string city,
        string street,
        DateTimeOffset birthDate,
        string hobby)
    {
        Id = aggregateId;
        CreateDate = DateTimeOffset.Now;

        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        City = city;
        Street = street;
        BirthDate = birthDate;
        Hobby = hobby;
    }

    public void Update(
        string firstName,
        string middleName,
        string lastName,
        string city,
        string street,
        DateTimeOffset birthDate,
        string hobby)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        City = city;
        Street = street;
        BirthDate = birthDate;
        Hobby = hobby;
    }
}
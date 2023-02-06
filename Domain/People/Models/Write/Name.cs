using Domain.Core;

namespace Domain.People.Models.Write;

public class Name : ValueObject
{
    public string FirstName { get; }
    public string MiddleName { get; }
    public string LastName { get; }

    public Name(string firstName, string middleName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentNullException(nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentNullException(nameof(lastName));
        }
        
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return MiddleName;
        yield return LastName;
    }
}
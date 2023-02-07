using Domain.Core;
using Domain.People.DomainEvents;
using Domain.People.Models.Write;
using NUnit.Framework;
using Shared;

namespace Tests;

[TestFixture]
public class JsonSerializerShould
{
    [Test]
    public void Serialize_Deserialize_Events()
    {
        var expected = new PersonCreated(
            Guid.NewGuid(),
            new Name("luka", null, "ogadze"),
            new Address("Test1", "Test2"),
            DateTimeOffset.Now,
            "helping"
        );
        var json = JsonService.Serialize(expected);

        var actual = JsonService.Deserialize<DomainEvent>(json, nameof(PersonCreated)).Value;
        
        Assert.Fail();
        // Assert.That(actual.Id, Is.EqualTo(expected.Id));
        // Assert.That(actual.OccurredOn, Is.EqualTo(expected.OccurredOn));
        // Assert.That(actual.Name, Is.EqualTo(expected.Name));
        // Assert.That(actual.Address, Is.EqualTo(expected.Address));
        // Assert.That(actual.BirthDate, Is.EqualTo(expected.BirthDate));
        // Assert.That(actual.Hobby, Is.EqualTo(expected.Hobby));

    }
}
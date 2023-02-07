using Domain;
using Domain.People.Models.Read;
using Domain.People.Models.Write;
using Domain.People.Repositories.Write;
using Infrastructure;
using Infrastructure.EventStore;
using Infrastructure.Repositories;
using NUnit.Framework;
using System;

namespace Tests;

[TestFixture]
public class Tests
{
    private ReadRepositoryProvider _readRepositoryProvider;
    private WriteRepositoryProvider _writeRepositoryProvider;
    private EventSourcingDbContext _eventSourcingDbContext;

    [SetUp]
    public void Init()
    {
        _eventSourcingDbContext = new EventSourcingDbContext();
        _readRepositoryProvider = new ReadRepositoryProvider(new PersonReadRepository(_eventSourcingDbContext));
        _writeRepositoryProvider =
            new WriteRepositoryProvider(new PersonWriteRepository(new EventStore(_eventSourcingDbContext)));
    }

    [TearDown]
    public void Destroy()
    {
        _eventSourcingDbContext.Dispose();
        _eventSourcingDbContext = null;
        _readRepositoryProvider = null;
        _writeRepositoryProvider = null;
    }
    
    

    [Test]
    public void CreatePerson()
    {
        var person = new Person(
            new Name("John", "Wayne", "Gacy"),
            new Address("Test City 1", "Test Street 1"),
            DateTimeOffset.Now,
            "Killing"
        );
        
        _writeRepositoryProvider.People.Create(person);
        _eventSourcingDbContext.SaveChanges();
    }

    [Test] public void GetCreatedPerson()
    {
        var personReadModel = _readRepositoryProvider.People.GetFirstOrDefault(x => x.FirstName == "John");


        Assert.That(personReadModel.IsSomething, Is.True);
    }

    [Test]
    public void UpdatePerson()
    {
        var people = _readRepositoryProvider.People.GetMany(x => x.MiddleName == "Wayne");
        foreach (PersonReadModel personReadModel in people)
        {
            var personWriteModel = _writeRepositoryProvider.People.FindBy(personReadModel.Id).Value;
            personWriteModel.Update(
                new Name("John", "Wick", "Gacy"),
                new Address("Test City 1", "Test Street 1"),
                DateTimeOffset.Now,
                "Killing"
            );
            _writeRepositoryProvider.People.Update(personWriteModel);
        }
        
        _eventSourcingDbContext.SaveChanges();                
    }

    [Test]
    public void GetUpdatedPerson()
    {
        var personReadModel = _readRepositoryProvider.People.GetFirstOrDefault(x => x.MiddleName == "Wick");

        Assert.That(personReadModel.IsSomething, Is.True);
    }
}
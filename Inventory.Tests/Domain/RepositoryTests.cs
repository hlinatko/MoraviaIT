using System;

using Biggy.Data.Json;

using Inventory.Persistence.Engine;
using Inventory.Persistence.Exceptions;
using Inventory.Persistence.Models;

using NUnit.Framework;

namespace Inventory.Tests.Domain
{
  [TestFixture]
  public class RepositoryTests
  {
    private IRepository<FakeAggregate> _sut;

    [SetUp]
    public void Setup()
    { 
      _sut= new Repository<FakeAggregate>(new Store(new JsonStore<EventDescriptor>(), new JsonSerializer()));
    }

    [Test]
    public void It_should_throw_on_non_existing_aggregate()
    {
      var nonExistingGuid= Guid.NewGuid();
      Assert.Throws<AggregateNotFound>(()=>_sut.GetById(nonExistingGuid));
    }

    [Test]
    public void It_should_be_able_to_return_given_aggregate()
    {
      var newId = Guid.NewGuid();
      var fake = new FakeAggregate(newId, "Not Everything Is At It Seems");

      _sut.Save(fake, -1);

      var db = _sut.GetById(newId);

      Assert.AreEqual(fake.Id, db.Id);
      Assert.AreEqual(fake.Name, db.Name);
    }

    [Test]
    public void It_shoud_save_given_object()
    {
      var newId = Guid.NewGuid();
      var fake = new FakeAggregate(newId, "Not Everything Is At It Seems");

      Assert.DoesNotThrow(()=>_sut.Save(fake, -1));
    }

    [Test]
    public void It_shoud_bubble_up_concurrency_exceptions()
    {
      var newId = Guid.NewGuid();
      var fake = new FakeAggregate(newId, "Not Everything Is At It Seems");

      Assert.Throws<Concurrency>(() => _sut.Save(fake, 35));
    }


    [Test]
    public void It_shoud_allow_to_update_aggregate()
    {
      var newId = Guid.NewGuid();
      var fake = new FakeAggregate(newId, "Not Everything Is At It Seems");
       _sut.Save(fake, -1);

       fake.ChangeName("Fake Can Be Just As Good");

       _sut.Save(fake, 0);

       var db = _sut.GetById(newId);

       Assert.AreEqual("Fake Can Be Just As Good", db.Name);
    }

  }
}

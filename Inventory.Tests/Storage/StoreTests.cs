using System;
using System.Collections.Generic;
using System.Linq;

using Biggy.Core;
using Biggy.Data.Json;

using Inventory.Messaging;
using Inventory.Persistence.Engine;
using Inventory.Persistence.Exceptions;
using Inventory.Persistence.Models;

using NUnit.Framework;

namespace Inventory.Tests.Storage
{
  [TestFixture]
  public class StoreTests
  {
    
    IDataStore<EventDescriptor> _db;
    Guid _id = Guid.NewGuid();
    IStore _sut;
    ISerializer _serializer;

    [SetUp]
    public void Setup()
    {
      _db = new JsonStore<EventDescriptor>();
      _db.DeleteAll();    
      _serializer= new JsonSerializer();
      _sut = new Store( _db, _serializer);
    }

    [Test]
    public void Store_can_add_new_occurrencie()
    {
      Assert.DoesNotThrow(() => _sut.SaveEvents(_id, GetDummyEvents(1), -1));
    }


    [Test]
    public void Store_can_add_many_new_occurrencies()
    { 
      Assert.DoesNotThrow(() => _sut.SaveEvents(_id, GetDummyEvents(1000), -1));
    }    

    [Test]
    public void Store_can_return_eventList()
    {
	  var sent= GetDummyEvents(678000).ToList ();
      _sut.SaveEvents(_id, sent, -1);

      var retrieved = _sut.GetEventsForAggregate(_id);
	  
	  Assert.AreEqual (sent.Count , retrieved.Count);
      var expected= sent.Max(e=>e.Version);
      var actual= retrieved.Max(e=>e.Version);
      
	  Assert.AreEqual(expected,actual );
    }

    [Test]
    public void Concurrency_exception_must_be_raised()
    {
      var sent = GetDummyEvents(100000);
      _sut.SaveEvents(_id, sent, -1);

      var newDump = GetDummyEvents(100000);      

      Assert.Throws<Concurrency>(() => _sut.SaveEvents(_id, newDump, 678));
    }


    [Test]
    public void Events_can_be_deserialized()
    {
      var sent = GetDummyEvents(100000);
      _sut.SaveEvents(_id, sent, -1);

      var list= _sut.GetEventsForAggregate(_id);

      foreach (var item in list)
      	Assert.IsInstanceOf<TestEvent>(item);
    }
	
	[Test]
	public void Wrong_initial_version_should_throw_concurrency_exception()
	{
		var sent = GetDummyEvents (100);
		_sut.SaveEvents (_id,sent,-1);
		var outband = GetDummyEvents (10);
		Assert.Throws<Concurrency> (()=> _sut.SaveEvents (_id,outband,-1));
	}

		[Test]
		public void An_event_stream_can_be_appened()
		{
			var sent = GetDummyEvents (100);
			_sut.SaveEvents (_id,sent,-1);
			var outband = GetDummyEvents (10);

			var current = sent.Max (e => e.Version);
			_sut.SaveEvents (_id,outband,current);
			var actual = _sut.GetEventsForAggregate (_id);

			Assert.AreEqual(sent.Count()+ outband.Count(),actual.Count);
		}   

    static IEnumerable<Event> GetDummyEvents(int quantity)
    {
      for (int i = 0; i < quantity; i++)
        yield return new TestEvent("this is " + i +" a demo", i);      
    }
  }
}

using System;

namespace Inventory
{
  public class Repository<T> : IRepository<T> where T : AggregateRoot, new() 
  {
    private readonly IStore _storage;

    public Repository(IStore storage)
    {
      _storage = storage;
    }

    public void Save(AggregateRoot aggregate, int expectedVersion)
    {
      _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
    }

    public T GetById(Guid id)
    {
      var obj = new T();
      var e = _storage.GetEventsForAggregate(id);
      obj.LoadsFromHistory(e);
      return obj;
    }
  }
}

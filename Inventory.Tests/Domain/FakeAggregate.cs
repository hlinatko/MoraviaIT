using System;
using Inventory.Messaging;

namespace Inventory.Tests.Domain
{
  public class FakeAggregate:AggregateRoot
  {
    private Guid _id;
    // really don't needed for bussines process only to test repo
    public string Name;

    public FakeAggregate(){}

    public override Guid Id
    {
      get { return _id ;}
    }    

    public FakeAggregate(Guid id, string name)
    {
      ApplyChange(new FakeCreated(id, name));
    }

    public void ChangeName(string newName)
    {
      if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName cannot be null");
      ApplyChange(new FakeRenamed(_id, newName));
    }

    private void Apply(FakeCreated e)
    {
      _id = e.Id;
      Name = e.Name;
    }

    private void Apply(FakeRenamed e)
    {
      Name = e.Name;
    }
  }

  public class FakeRenamed : Event
  {
  public readonly Guid Id;
    public readonly string Name;

    public FakeRenamed(Guid id, string name)
    {
      Id = id;
      Name = name;
    }
  }

  public class FakeCreated:Event
  {
    public readonly Guid Id;
    public readonly string Name;

    public FakeCreated(Guid id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}

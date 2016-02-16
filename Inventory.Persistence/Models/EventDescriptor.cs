using System;
using Inventory.Messaging;

namespace Inventory.Persistence.Models
{
  public class EventDescriptor
  {
    public string EventData {get;set;}
    public Guid Id {get;set;}
    public int Version { get; set; }

    public EventDescriptor(){}
    
    public EventDescriptor(Guid id, string eventData, int version)
    {
      EventData = eventData;
      Version = version;
      Id = id;
    }
  }
}
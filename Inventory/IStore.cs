using System;
using System.Collections.Generic;
using Inventory.Messaging;

namespace Inventory
{
  public interface IStore
  {    
      void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
      List<Event> GetEventsForAggregate(Guid aggregateId);    
  }
}

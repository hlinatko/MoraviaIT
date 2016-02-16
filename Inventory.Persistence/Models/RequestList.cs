using System.Collections.Generic;

namespace Inventory.Persistence.Models
{
  public class RequestList
  {
    public int Version { get;set; }
    public IList<EventDescriptor> Payload { get; set; }

    public RequestList()
    {
      Payload = new List<EventDescriptor>();
    }
  }
}
using Inventory.Messaging;
using Newtonsoft.Json;

namespace Inventory.Persistence.Engine
{

  public class JsonSerializer : ISerializer
  {
    private readonly JsonSerializerSettings settings = new JsonSerializerSettings
    {
      TypeNameHandling = TypeNameHandling.All
    };

    public string Serialize(Event @event)
    {
      return JsonConvert.SerializeObject(@event,settings);
    }

    public Event Deserialize(string data)
    {
      return JsonConvert.DeserializeObject<Event>(data,settings);
    }
  }
}

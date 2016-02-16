
namespace Inventory.Tests.Storage
{
  public class FakePublisher:IEventPublisher
  {
    public void Publish<T>(T @event) where T : Messaging.Event
    {
     //sunk all events I'm a fake cannot help myself.
    }
  }
}

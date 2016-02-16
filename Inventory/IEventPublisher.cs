using Inventory.Messaging;

namespace Inventory
{
  public interface IEventPublisher
  {
    void Publish<T>(T @event) where T : Event;
  }
}

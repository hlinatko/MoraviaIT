using Inventory.Messaging;

namespace Inventory
{
  public interface ICommandSender
  {
    void Send<T>(T command) where T : Command;
  }
}

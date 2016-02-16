using Inventory.Handlers;
using Inventory.Messaging;
using Inventory.Web.Services;
using Nancy;

namespace Inventory.Web
{
  public class Bootstrapper:DefaultNancyBootstrapper
  {
    protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
    {
      Wiring.Wire();
      base.ApplicationStartup(container, pipelines);
    }

    /*protected override IRootPathProvider RootPathProvider
    {
        get { return new MyRootPathProvider(); }
    }*/
  }

  public static class Wiring
  {
    public static void Wire()
    {
      var bus = new MiniVan();

      var storage = new RestStore();
      var rep = new Repository<InventoryItem>(storage);
      var commands = new InventoryCommandHandlers(rep);
      bus.RegisterHandler<CheckInItemsToInventory>(commands.Handle);
      bus.RegisterHandler<CreateInventoryItem>(commands.Handle);
      bus.RegisterHandler<DeactivateInventoryItem>(commands.Handle);
      bus.RegisterHandler<RemoveItemsFromInventory>(commands.Handle);
      bus.RegisterHandler<RenameInventoryItem>(commands.Handle);        
      ServiceLocator.Bus = bus;
    }
  }
}
using System;
using System.IO;

using Biggy.Core;
using Biggy.Data.Json;

using Inventory.Persistence.Engine;
using Inventory.Persistence.Models;

using Nancy;
using Nancy.TinyIoc;

namespace Inventory.Persistence
{
  public class PersistenceBootstrapper:DefaultNancyBootstrapper
  {   
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {      
      var dir= Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data"));
      var store = new JsonStore<EventDescriptor>(dir, "store", "events");
      container.Register<IDataStore<EventDescriptor>,JsonStore<EventDescriptor>>(store);
      container.Register<IStore>(new Store(store, new JsonSerializer()));
    }
  }
}
using System;
using Inventory.Messaging;
using Inventory.Web.Services;
using Nancy;
using Inventory.Web.Objects;
using System.Collections.Generic;

namespace Inventory.Web.Modules
{
  public class HomeModule:NancyModule
  {
    private readonly MiniVan _bus = ServiceLocator.Bus;

    public HomeModule()
    {
        Get["/"] = _ =>
        {
            return View["index", GetModel()];
        };

        Post["/"] = _ =>
        {
        _bus.Send(new CreateInventoryItem(Guid.NewGuid(), Request.Form.name));
          
        return View["index", GetModel()];            
        };

        Put["/{guid:id}/{int:version}"] = _ =>
        {
        _bus.Send(new RenameInventoryItem(_.id, Request.Form.name, _.version));
        return View["index"];
        };

        Delete["/{guid:id}/{int:version}"] = _ =>
        {
        _bus.Send(new DeactivateInventoryItem(_.id, _.version));
        return View["index"];
        };

        Post["/Checkin/{guid:id}/{int:version}"] = _ =>
        {
        _bus.Send(new CheckInItemsToInventory(_.id, Request.Form.number, _.version));
        return View["index"];
        };


        Post["/Checkout/{guid:id}/{int:version}"] = _ =>
        {
        _bus.Send(new RemoveItemsFromInventory(_.id, Request.Form.number, _.version));
        return View["index"];
        };

        //new ones
        
        Get["/add"] = _ =>
        {
            return View["add"];
        };

        Get["/remove"] = _ =>
        {
            return View["remove"];
        };

        Post["/remove"] = _ =>
        {
            //TODO: this should be calling _bus

            RestStore store = new RestStore();
            store.Remove(Request.Form.id);

            return View["index", GetModel()];
        };

        Get["/remove/{id}"] = parameters =>
        {
            //TODO: this should be calling _bus

            //_bus.Send(new DeactivateInventoryItem(parameters.id, _.version));
            //_bus.Send(new DeactivateInventoryItem(parameters.id, -1));

            string id = parameters.id;

            RestStore store = new RestStore();
            store.Remove(new Guid(id));

            return View["index", GetModel()];
        };

        Get["/getjson"] = _ =>
        {
            RestStore store = new RestStore();

            return store.GetTextFromFile();
        };
    }

    public IEnumerable<InventoryHelpClass> GetModel()
    {
        RestStore store = new RestStore();
        InventoryDataSource data = store.GetFromFile();
        IEnumerable<InventoryHelpClass> model = data.data;

        return model;
    }
  }
}
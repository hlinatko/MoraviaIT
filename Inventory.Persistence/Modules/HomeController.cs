using System;
using System.Dynamic;

using Inventory.Messaging;
using Inventory.Persistence.Exceptions;
using Inventory.Persistence.Models;

using Nancy;
using Nancy.ModelBinding;

namespace Inventory.Persistence.Modules
{
  public class HomeController:NancyModule
  {
    public HomeController (IStore store) : base ("/")
		{     

			Get ["/{aggregate:guid}"] = _ => {
        
        dynamic events= new ExpandoObject();

        try
        {
          events.Result = store.GetEventsForAggregate(Guid.Parse(_.aggregate)).ToList<Event>();
        }        
        catch (AggregateNotFound ex)
        {
          events.Error = ex.GetType();
          events.StatuCode = 404;
          events.Message = ex.Message;
        }
        catch (Exception ex)
        {
          events.Error = ex.GetType();
          events.StatuCode = 500;
          events.Message = ex.Message;
        }
				
				return events;        
			};

      Post["/{aggregate:guid}"] = _ => 
      {
        var eventList = this.Bind<RequestList>();
        return 200;
      };
		}
	}
}
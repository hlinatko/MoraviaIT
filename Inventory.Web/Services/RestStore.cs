using System;
using System.Collections.Generic;
using Inventory.Messaging;
using Newtonsoft.Json;
using System.Collections;
using System.IO;
using Inventory.Web.Objects;

namespace Inventory.Web.Services
{
    public class RestStore : IStore
    {
        public RestStore()
        {                       

        }

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            InventoryDataSource dataSource = GetFromFile();

            foreach (Inventory.Messaging.Event ev in events)
            {
                if (ev.GetType() == typeof(InventoryItemCreated))
                {
                    Inventory.Messaging.InventoryItemCreated item = (InventoryItemCreated)ev;

                    dataSource.data.Add(new InventoryHelpClass { GUID = item.Id.ToString(), Name = item.Name });
                }              
  
                //TODO: handle other events and also store them all with types to call next method and others...
                
                //TODO: use version to handle concurrency, etc.
            }                       

            Save(dataSource);            
        }
        
        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            //TODO: call it properly...

            throw new NotImplementedException();
                
        }

        //PetrK, new methods:

        public string GetRoot()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public string GetPath()
        {
            string sRet = Path.Combine(GetRoot(), "store.json");
            return sRet;
        }

        public string GetTextFromFile()
        {
            string sRet = "";

            string sPath = GetPath();

            if (File.Exists(sPath))
            {
                sRet = File.ReadAllText(sPath);
            }

            return sRet;
        }

        public InventoryDataSource GetFromFile()
        {
            InventoryDataSource data = null;

            string sText = GetTextFromFile();

            if (sText != "")
            {
                data = JsonConvert.DeserializeObject<InventoryDataSource>(sText);
            }
            else
            {
                data = new InventoryDataSource();
            }

            return data;
        }


        public void Save( InventoryDataSource dataSource)
        {
            string sText = JsonConvert.SerializeObject(dataSource);

            File.WriteAllText(GetPath(), sText);
        }

        public void Remove(string id)
        {
            InventoryDataSource dataSource = GetFromFile();

            InventoryHelpClass invToRemove = null;
            foreach (InventoryHelpClass invItem in dataSource.data)
            {
                if (invItem.GUID == id)
                {
                    invToRemove = invItem;
                    break;
                }
            }

            if (invToRemove != null)
            {
                dataSource.data.Remove(invToRemove);
            }

            Save(dataSource);
        }
    }
}
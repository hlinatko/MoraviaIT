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

            dataSource.data.Add(new InventoryHelpClass { _guid = aggregateId, _events = events });

            Save(dataSource);            
        }
        
        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            InventoryDataSource dataSource = GetFromFile();

            List<Event> ret = null;
            foreach (InventoryHelpClass inventoryHelp in dataSource.data)
            {
                if (inventoryHelp._guid == aggregateId)
                {
                    ret = (List<Event>)inventoryHelp._events;
                    break;
                }
            }

            return ret;                
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

        public JsonSerializerSettings GetJsonSettings()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

            return settings;
        }

        public InventoryDataSource GetFromFile()
        {
            InventoryDataSource data = null;

            string sText = GetTextFromFile();

            if (sText != "")
            {                
                data = JsonConvert.DeserializeObject<InventoryDataSource>(sText, GetJsonSettings());
            }
            else
            {
                data = new InventoryDataSource();
            }

            return data;
        }


        public void Save( InventoryDataSource dataSource)
        {
            string sText = JsonConvert.SerializeObject(dataSource, GetJsonSettings());

            File.WriteAllText(GetPath(), sText);
        }

        public void Remove(Guid guid)
        {
            InventoryDataSource dataSource = GetFromFile();

            InventoryHelpClass invToRemove = null;
            foreach (InventoryHelpClass invItem in dataSource.data)
            {
                if (invItem._guid == guid)
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory.Messaging;

namespace Inventory.Web.Objects
{
    //helper class to store data for easy access and for json file

    public class InventoryDataSource
    {
        public List<InventoryHelpClass> data = new List<InventoryHelpClass>();
    }

    public class InventoryHelpClass
    {
        public Guid _guid { get; set; }
        public IEnumerable<Event> _events { get; set; }

        public string _types{
            get
            {
                string sRet = "";

                foreach (Event ev in _events)
                {
                    sRet += ev.GetType();

                    if (ev.GetType() == typeof(InventoryItemCreated))
                    {
                        sRet += " (Name: " + ((InventoryItemCreated)ev).Name + ")";
                    }

                    sRet += ",";
                }

                sRet = sRet.Remove(sRet.Length - 1, 1);

                return sRet;
            }
        }
    }
}
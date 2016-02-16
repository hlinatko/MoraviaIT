using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Web.Objects
{
    //helper class to store data for easy access and for json file

    public class InventoryDataSource
    {
        public List<InventoryHelpClass> data = new List<InventoryHelpClass>();
    }

    public class InventoryHelpClass
    {
        public string GUID { get; set; }
        public string Name { get; set; }
    }
}
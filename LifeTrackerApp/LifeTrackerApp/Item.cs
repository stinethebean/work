using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace LifeTrackerApp
{
    public class Item
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }

    Item item = new Item { Text = "Awesome item" };
    await App.MobileService.GetTable<Item>().InsertAsync(item);
}

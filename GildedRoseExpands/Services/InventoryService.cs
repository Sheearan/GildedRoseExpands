using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GildedRoseExpands.Interfaces;
using GildedRoseExpands.Models;

namespace GildedRoseExpands.Services
{
    public class InventoryService : IInventoryService
    {
        private List<Item> inventory;

        public InventoryService()
        {
            Item tunic = new Item { Name = "Fine Tunic", Description = "A sleeveless, knee-length garment.", Price = 49.95M };
            Item banana = new Item { Name = "Fine Banana", Description = "A curved, yellow fruit.", Price = 1.95M };
            inventory = new List<Item> { tunic, banana };
        }

        public IEnumerable<Item> GetCurrentInventory()
        {
            return inventory;
        }
    }
}
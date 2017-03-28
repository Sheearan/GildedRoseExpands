using System.Collections.Generic;
using GildedRoseExpands.Interfaces;
using GildedRoseExpands.Models;

namespace GildedRoseExpands.Services
{
    public class InventoryService : IInventoryService
    {
        // Note: I know this is more or less a copy of the mock inventory service. In a real system, I'd expect this to
        // actually connect to a database and return information from the database.
        private List<Item> inventory;

        public InventoryService()
        {
            Item tunic = new Item { ItemId = 1, Name = "Fine Tunic", Description = "A sleeveless, knee-length garment.", Price = 49.95M, Quantity = 42 };
            Item banana = new Item { ItemId = 2, Name = "Fine Banana", Description = "A curved, yellow fruit.", Price = 1.95M, Quantity = 0 };
            inventory = new List<Item> { tunic, banana };
        }

        public IEnumerable<Item> GetCurrentInventory()
        {
            return inventory;
        }

        public Item GetItem(int itemId)
        {
            {
                foreach (Item i in inventory)
                {
                    if (i.ItemId == itemId)
                    {
                        return i;
                    }
                }

                return null;
            }
        }

        public void SetQuantity(int itemId, int quantity)
        {
            foreach (Item i in inventory)
            {
                if (i.ItemId == itemId)
                {
                    i.Quantity = quantity;
                }
            }
        }
    }
}
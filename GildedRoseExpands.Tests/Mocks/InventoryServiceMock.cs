using GildedRoseExpands.Interfaces;
using System.Collections.Generic;
using GildedRoseExpands.Models;

namespace GildedRoseExpands.Tests.Mocks
{
    internal class InventoryServiceMock : IInventoryService
    {
        IEnumerable<Item> inventory;

        internal InventoryServiceMock()
        {
            // Use the default mock database values
            Item tunic = new Item { ItemId = 1, Name = "Fine Tunic", Description = "A sleeveless, knee-length garment.", Price = 49.95M, Quantity = 42 };
            Item banana = new Item { ItemId = 2, Name = "Fine Banana", Description = "A curved, yellow fruit.", Price = 1.95M, Quantity = 0 };
            inventory = new List<Item> { tunic, banana };
        }

        internal InventoryServiceMock(IEnumerable<Item> inventoryToSimulate)
        {
            inventory = inventoryToSimulate;
        }

        public IEnumerable<Item> GetCurrentInventory()
        {
            return inventory;
        }

        public Item GetItem(int itemId)
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

        internal int GetItemQuantity(int itemId)
        {
            foreach (Item i in inventory)
            {
                if (i.ItemId == itemId)
                {
                    return i.Quantity;
                }
            }

            return -1;
        }
    }
}

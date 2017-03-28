using GildedRoseExpands.Interfaces;
using System.Collections.Generic;
using GildedRoseExpands.Models;

namespace GildedRoseExpands.Tests.Mocks
{
    internal class InventoryServiceMock : IInventoryService
    {
        IEnumerable<Item> inventory;

        public InventoryServiceMock(IEnumerable<Item> inventoryToSimulate)
        {
            inventory = inventoryToSimulate;
        }

        public IEnumerable<Item> GetCurrentInventory()
        {
            return inventory;
        }
    }
}

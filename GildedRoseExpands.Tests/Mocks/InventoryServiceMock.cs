using GildedRoseExpands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

using System.Collections.Generic;

using GildedRoseExpands.Models;

namespace GildedRoseExpands.Interfaces
{
    public interface IInventoryService
    {
        IEnumerable<Item> GetCurrentInventory();
    }
}

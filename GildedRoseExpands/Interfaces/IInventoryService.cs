using System.Collections.Generic;

using GildedRoseExpands.Models;

namespace GildedRoseExpands.Interfaces
{
    public interface IInventoryService
    {
        IEnumerable<Item> GetCurrentInventory();
        Item GetItem(int itemId);
        void SetQuantity(int itemId, int quantity);
    }
}

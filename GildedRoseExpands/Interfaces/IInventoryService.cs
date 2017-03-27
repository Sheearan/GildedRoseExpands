using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GildedRoseExpands.Models;

namespace GildedRoseExpands.Interfaces
{
    public interface IInventoryService
    {
        IEnumerable<Item> GetCurrentInventory();
    }
}

using System.Collections.Generic;
using System.Web.Http;
using GildedRoseExpands.Models;
using GildedRoseExpands.Interfaces;
using GildedRoseExpands.Services;

namespace GildedRoseExpands.Controllers
{
    public class InventoryController : ApiController
    {
        private IInventoryService inventoryService;

        public InventoryController()
        {
            inventoryService = new InventoryService();
        }

        public InventoryController(IInventoryService inventory)
        {
            inventoryService = inventory;
        }

        // GET api/inventory
        public IEnumerable<Item> Get()
        {
            return inventoryService.GetCurrentInventory();
        }
    }
}

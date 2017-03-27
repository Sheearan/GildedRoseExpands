using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        public IEnumerable<Item> Get()
        {
            return inventoryService.GetCurrentInventory();
        }
    }
}

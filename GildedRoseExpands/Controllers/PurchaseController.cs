using GildedRoseExpands.Interfaces;
using GildedRoseExpands.Models;
using GildedRoseExpands.Services;
using System.Web.Http;

namespace GildedRoseExpands.Controllers
{
    [Authorize]
    public class PurchaseController : ApiController
    {
        private IInventoryService inventoryService;

        public PurchaseController()
        {
            inventoryService = new InventoryService();
        }

        public PurchaseController(IInventoryService inventory)
        {
            inventoryService = inventory;
        }

        // POST api/purchase/42
        public void Post(int id)
        {
            Item purchasedItem = inventoryService.GetItem(id);
            inventoryService.SetQuantity(id, purchasedItem.Quantity - 1);
        }
    }
}

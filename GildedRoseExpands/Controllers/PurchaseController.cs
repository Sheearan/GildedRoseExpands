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
        public PurchaseResults Post(int id)
        {
            Item purchasedItem = inventoryService.GetItem(id);

            if (purchasedItem.Quantity > 0)
            {
                inventoryService.SetQuantity(id, purchasedItem.Quantity - 1);
                return PurchaseResults.ItemPurchased;
            }
            else
            {
                return PurchaseResults.OutOfStock;
            }
        }
    }
}

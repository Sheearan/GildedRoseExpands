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
        private IPaymentService paymentService;

        public PurchaseController()
        {
            inventoryService = new InventoryService();
            paymentService = new PaymentService();
        }

        public PurchaseController(IInventoryService inventory, IPaymentService payment)
        {
            inventoryService = inventory;
            paymentService = payment;
        }

        // POST api/purchase/42
        public PurchaseResults Post(int id)
        {
            Item purchasedItem = inventoryService.GetItem(id);

            if (purchasedItem.Quantity > 0)
            {
                return AttemptPurchase(id, purchasedItem);
            }
            else
            {
                return PurchaseResults.OutOfStock;
            }
        }

        private PurchaseResults AttemptPurchase(int id, Item purchasedItem)
        {
            if (paymentService.processPayment())
            {
                inventoryService.SetQuantity(id, purchasedItem.Quantity - 1);
                return PurchaseResults.ItemPurchased;
            }
            else
            {
                return PurchaseResults.PaymentFailed;
            }
        }
    }
}

using System.Web.Http;
using GildedRoseExpands.Interfaces;
using GildedRoseExpands.Models;
using GildedRoseExpands.Services;

namespace GildedRoseExpands.Controllers
{
    [Authorize]
    public class PurchaseController : ApiController
    {
        private IInventoryService inventoryService;
        private IPaymentService paymentService;
        private IShippingService shippingService;

        public PurchaseController()
        {
            inventoryService = new InventoryService();
            paymentService = new PaymentService();
            shippingService = new ShippingService();
        }

        public PurchaseController(IInventoryService inventory, IPaymentService payment, IShippingService shipping)
        {
            inventoryService = inventory;
            paymentService = payment;
            shippingService = shipping;
        }

        // POST api/purchase/42
        public PurchaseResults Post(int id)
        {
            Item purchasedItem = inventoryService.GetItem(id);

            if (purchasedItem != null)
            {
                return AttemptPurchase(purchasedItem);
            }

            return PurchaseResults.ItemNotFound;
        }

        private PurchaseResults AttemptPurchase(Item purchasedItem)
        {
            if (purchasedItem.Quantity > 0)
            {
                return PurchaseItem(purchasedItem);
            }
            else
            {
                return PurchaseResults.OutOfStock;
            }
        }

        private PurchaseResults PurchaseItem(Item purchasedItem)
        {
            if (paymentService.processPayment())
            {
                shippingService.shipItem(purchasedItem.ItemId, User.Identity.Name);
                inventoryService.SetQuantity(purchasedItem.ItemId, purchasedItem.Quantity - 1);
                return PurchaseResults.ItemPurchased;
            }
            else
            {
                return PurchaseResults.PaymentFailed;
            }
        }
    }
}

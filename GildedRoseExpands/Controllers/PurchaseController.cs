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
        private ILoggingService loggingService;

        public PurchaseController()
        {
            inventoryService = new InventoryService();
            paymentService = new PaymentService();
            shippingService = new ShippingService();
            loggingService = new LoggingService();
        }

        public PurchaseController(IInventoryService inventory, IPaymentService payment, IShippingService shipping, ILoggingService logging)
        {
            inventoryService = inventory;
            paymentService = payment;
            shippingService = shipping;
            loggingService = logging;
        }

        // POST api/purchase/42
        public PurchaseResults Post(int id)
        {
            Item purchasedItem = inventoryService.GetItem(id);

            if (purchasedItem != null)
            {
                return AttemptPurchase(purchasedItem);
            }

            loggingService.logString(string.Format("{0} attempted to purchase item {1}, which doesn't exist.", User.Identity.Name, id));
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
                loggingService.logString(string.Format("{0} attempted to purchase item {1}, which is out of stock.", User.Identity.Name, purchasedItem.ItemId));
                return PurchaseResults.OutOfStock;
            }
        }

        private PurchaseResults PurchaseItem(Item purchasedItem)
        {
            if (paymentService.processPayment())
            {
                shippingService.shipItem(purchasedItem.ItemId, User.Identity.Name);
                inventoryService.SetQuantity(purchasedItem.ItemId, purchasedItem.Quantity - 1);

                loggingService.logString(string.Format("Item {0} purchased by {1}.", purchasedItem.ItemId, User.Identity.Name));
                return PurchaseResults.ItemPurchased;
            }
            else
            {
                loggingService.logString(string.Format("{0} attempted to purchase item {1}, but payment failed.", User.Identity.Name, purchasedItem.ItemId));
                return PurchaseResults.PaymentFailed;
            }
        }
    }
}

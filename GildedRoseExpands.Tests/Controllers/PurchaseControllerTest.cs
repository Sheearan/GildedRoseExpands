using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRoseExpands.Tests.Mocks;
using GildedRoseExpands.Controllers;
using GildedRoseExpands.Models;
using GildedRoseExpands.Tests.Helpers;

namespace GildedRoseExpands.Tests.Controllers
{
    [TestClass]
    public class PurchaseControllerTest
    {
        private const int IN_STOCK_ITEM = 1;
        private const int OUT_OF_STOCK_ITEM = 2;
        private const int NONEXISTENT_ITEM = 314;

        [TestMethod]
        public void PurchasingInStockItemDecreasesQuantity()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(true);
            int beginningQuantity = mocks.inventory.GetItemQuantity(IN_STOCK_ITEM);

            // Act
            PurchaseResults result = mocks.controller.Post(IN_STOCK_ITEM);

            // Assert
            int expectedValue = beginningQuantity - 1;
            int actualValue = mocks.inventory.GetItemQuantity(IN_STOCK_ITEM);
            Assert.AreEqual(result, PurchaseResults.ItemPurchased, "Item should have been successfully purchased.");
            Assert.AreEqual(expectedValue, actualValue, string.Format("Expected {0} of item {1} instead of {2}. Purchasing an item should decrement the quantity by one.", expectedValue, IN_STOCK_ITEM, actualValue));
        }

        [TestMethod]
        public void PurchasingInStockItemRequestsShipment()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(true);

            // Act
            PurchaseResults result = mocks.controller.Post(IN_STOCK_ITEM);

            // Assert
            Assert.AreEqual(1, mocks.shipping.getOrders().Count, "Order should have been sent to the shipping service.");
            Assert.AreEqual(IN_STOCK_ITEM, mocks.shipping.getOrders()[0].Item1, string.Format("We should have ordered item {0} instead of item {1}", IN_STOCK_ITEM, mocks.shipping.getOrders()[0].Item1));
        }

        [TestMethod]
        public void PurchasingOutOfStockItemReturnsOutOfStock()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(true);

            // Act
            PurchaseResults result = mocks.controller.Post(OUT_OF_STOCK_ITEM);

            // Assert
            Assert.AreEqual(PurchaseResults.OutOfStock, result, "API should have reported out of stock item.");
        }

        [TestMethod]
        public void FailedPaymentMethodDoesNotDecreaseQuantity()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(false);
            int beginningQuantity = mocks.inventory.GetItemQuantity(IN_STOCK_ITEM);

            // Act
            PurchaseResults result = mocks.controller.Post(IN_STOCK_ITEM);

            // Assert
            int expectedValue = beginningQuantity - 1;
            int actualValue = mocks.inventory.GetItemQuantity(IN_STOCK_ITEM);
            Assert.AreEqual(PurchaseResults.PaymentFailed, result, "API should have reported payment failure.");
            Assert.AreEqual(beginningQuantity, actualValue, string.Format("Expected {0} of item {1} instead of {2}. Failed payment should not affect the quantity.", expectedValue, IN_STOCK_ITEM, actualValue));
        }

        [TestMethod]
        public void NonexistentItemIdReturnsItemNotFound()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(true);

            // Act
            PurchaseResults result = mocks.controller.Post(NONEXISTENT_ITEM);

            // Assert
            Assert.AreEqual(PurchaseResults.ItemNotFound, result, "API should have reported item not found.");
        }

        [TestMethod]
        public void PurchasingInStockItemLogsPurchase()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(true);

            // Act
            PurchaseResults result = mocks.controller.Post(IN_STOCK_ITEM);

            // Assert
            List<string> logEntries = mocks.logging.GetLogEntries();
            Assert.AreEqual(1, logEntries.Count, "Purchase should have been logged.");
            Assert.AreNotEqual(-1, logEntries[0].IndexOf("Item 1 purchased by "), "Logging should indicate successful purchase.");
        }

        [TestMethod]
        public void PurchasingOutOfStockItemLogsAttemptedPurchaseOfOutOfStockItem()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(true);

            // Act
            PurchaseResults result = mocks.controller.Post(OUT_OF_STOCK_ITEM);

            // Assert
            List<string> logEntries = mocks.logging.GetLogEntries();
            Assert.AreEqual(1, logEntries.Count, "Attempted purchase should have been logged.");
            Assert.AreNotEqual(-1, logEntries[0].IndexOf(" attempted to purchase item 2, which is out of stock."), "Logging should indicate attempt at purchasing an out of stock item.");
        }

        [TestMethod]
        public void FailedPaymentMethodLogsAttemptedPurchaseWithFailedPayment()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(false);
            int beginningQuantity = mocks.inventory.GetItemQuantity(IN_STOCK_ITEM);

            // Act
            PurchaseResults result = mocks.controller.Post(IN_STOCK_ITEM);

            // Assert
            List<string> logEntries = mocks.logging.GetLogEntries();
            Assert.AreEqual(1, logEntries.Count, "Attempted purchase should have been logged.");
            Assert.AreNotEqual(-1, logEntries[0].IndexOf(" attempted to purchase item 1, but payment failed."), "Logging should indicate failed payment.");
        }

        [TestMethod]
        public void NonexistentItemIdLogsRequestForNonexistentItem()
        {
            // Arrange
            PurchaseMockCollection mocks = CreateController(true);

            // Act
            PurchaseResults result = mocks.controller.Post(NONEXISTENT_ITEM);

            // Assert
            List<string> logEntries = mocks.logging.GetLogEntries();
            Assert.AreEqual(1, logEntries.Count, "Attempted purchase should have been logged.");
            Assert.AreNotEqual(-1, logEntries[0].IndexOf(" attempted to purchase item 314, which doesn't exist."), "Logging should indicate attempt at purchasing a nonexistent item.");
        }

        private PurchaseMockCollection CreateController(bool paymentResult)
        {
            PurchaseMockCollection mocks = new PurchaseMockCollection();
            mocks.inventory = new InventoryServiceMock();
            mocks.payment = new PaymentServiceMock(paymentResult);
            mocks.shipping = new ShippingServiceMock();
            mocks.logging = new LoggingServiceMock();
            mocks.controller = new PurchaseController(mocks.inventory, mocks.payment, mocks.shipping, mocks.logging);
            return mocks;
        }
    }
}

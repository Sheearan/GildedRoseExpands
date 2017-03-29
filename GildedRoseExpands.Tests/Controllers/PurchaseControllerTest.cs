﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRoseExpands.Tests.Mocks;
using GildedRoseExpands.Controllers;
using GildedRoseExpands.Models;

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
            InventoryServiceMock inventory = new InventoryServiceMock();
            PaymentServiceMock payment = new PaymentServiceMock(true);
            ShippingServiceMock shipping = new ShippingServiceMock();
            PurchaseController controller = new PurchaseController(inventory, payment, shipping);
            int beginningQuantity = inventory.GetItemQuantity(IN_STOCK_ITEM);

            // Act
            PurchaseResults result = controller.Post(IN_STOCK_ITEM);

            // Assert
            int expectedValue = beginningQuantity - 1;
            int actualValue = inventory.GetItemQuantity(IN_STOCK_ITEM);
            Assert.AreEqual(result, PurchaseResults.ItemPurchased, "Item should have been successfully purchased.");
            Assert.AreEqual(expectedValue, actualValue, string.Format("Expected {0} of item {1} instead of {2}. Purchasing an item should decrement the quantity by one.", expectedValue, IN_STOCK_ITEM, actualValue));
        }

        [TestMethod]
        public void PurchasingInStockItemRequestsShipment()
        {
            // Arrange
            InventoryServiceMock inventory = new InventoryServiceMock();
            PaymentServiceMock payment = new PaymentServiceMock(true);
            ShippingServiceMock shipping = new ShippingServiceMock();
            PurchaseController controller = new PurchaseController(inventory, payment, shipping);

            // Act
            PurchaseResults result = controller.Post(IN_STOCK_ITEM);

            // Assert
            Assert.AreEqual(1, shipping.getOrders().Count, "Order should have been sent to the shipping service.");
            Assert.AreEqual(IN_STOCK_ITEM, shipping.getOrders()[0].Item1, string.Format("We should have ordered item {0} instead of item {1}", IN_STOCK_ITEM, shipping.getOrders()[0].Item1));
        }

        [TestMethod]
        public void PurchasingOutOfStockItemReturnsOutOfStock()
        {
            // Arrange
            InventoryServiceMock inventory = new InventoryServiceMock();
            PaymentServiceMock payment = new PaymentServiceMock(true);
            ShippingServiceMock shipping = new ShippingServiceMock();
            PurchaseController controller = new PurchaseController(inventory, payment, shipping);

            // Act
            PurchaseResults result = controller.Post(OUT_OF_STOCK_ITEM);

            // Assert
            Assert.AreEqual(PurchaseResults.OutOfStock, result, "API should have reported out of stock item.");
        }

        [TestMethod]
        public void FailedPaymentMethodDoesNotDecreaseQuantity()
        {
            // Arrange
            InventoryServiceMock inventory = new InventoryServiceMock();
            PaymentServiceMock payment = new PaymentServiceMock(false);
            ShippingServiceMock shipping = new ShippingServiceMock();
            PurchaseController controller = new PurchaseController(inventory, payment, shipping);
            int beginningQuantity = inventory.GetItemQuantity(IN_STOCK_ITEM);

            // Act
            PurchaseResults result = controller.Post(IN_STOCK_ITEM);

            // Assert
            int expectedValue = beginningQuantity - 1;
            int actualValue = inventory.GetItemQuantity(IN_STOCK_ITEM);
            Assert.AreEqual(PurchaseResults.PaymentFailed, result, "API should have reported payment failure.");
            Assert.AreEqual(beginningQuantity, actualValue, string.Format("Expected {0} of item {1} instead of {2}. Failed payment should not affect the quantity.", expectedValue, IN_STOCK_ITEM, actualValue));
        }

        [TestMethod]
        public void NonexistentItemIdReturnsItemNotFound()
        {
            // Arrange
            InventoryServiceMock inventory = new InventoryServiceMock();
            PaymentServiceMock payment = new PaymentServiceMock(true);
            ShippingServiceMock shipping = new ShippingServiceMock();
            PurchaseController controller = new PurchaseController(inventory, payment, shipping);

            // Act
            PurchaseResults result = controller.Post(NONEXISTENT_ITEM);

            // Assert
            Assert.AreEqual(PurchaseResults.ItemNotFound, result, "API should have reported item not found.");
        }
    }
}

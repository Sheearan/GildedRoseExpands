using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using GildedRoseExpands.Controllers;
using GildedRoseExpands.Models;

namespace GildedRoseExpands.Tests.Integration
{
    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void IntegrationGetInventoryReturnsExpectedItems()
        {
            // Arrange
            InventoryController inventoryController = new InventoryController();

            // Act
            IEnumerable<Item> result = inventoryController.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            Assert.AreEqual("Fine Tunic", result.ElementAt(0).Name);
            Assert.AreEqual("A sleeveless, knee-length garment.", result.ElementAt(0).Description);
            Assert.AreEqual(49.95M, result.ElementAt(0).Price);

            Assert.AreEqual("Fine Banana", result.ElementAt(1).Name);
            Assert.AreEqual("A curved, yellow fruit.", result.ElementAt(1).Description);
            Assert.AreEqual(1.95M, result.ElementAt(1).Price);
        }

        [TestMethod]
        public void IntegrationPurchasingInStockItemReturnsInStock()
        {
            // Arrange
            InventoryController inventoryController = new InventoryController();
            PurchaseController purchaseController = new PurchaseController();
            IEnumerable<Item> inventory = inventoryController.Get();
            Item inStockItem = null;
            foreach (Item i in inventory)
            {
                if (i.Quantity > 0)
                {
                    inStockItem = i;
                    break;
                }
            }

            // Act
            Assert.IsNotNull(inStockItem, "There should be an in stock item in the test database.");
            PurchaseResults result = purchaseController.Post(inStockItem.ItemId);

            // Assert
            Assert.AreEqual(PurchaseResults.ItemPurchased, result, "Item should have been successfully purchased.");
        }

        [TestMethod]
        public void IntegrationPurchasingOutOfStockItemReturnsOutOfStock()
        {
            // Arrange
            InventoryController inventoryController = new InventoryController();
            PurchaseController purchaseController = new PurchaseController();
            IEnumerable<Item> inventory = inventoryController.Get();
            Item outOfStockItem = null;
            foreach (Item i in inventory)
            {
                if (i.Quantity == 0)
                {
                    outOfStockItem = i;
                    break;
                }
            }

            // Act
            Assert.IsNotNull(outOfStockItem, "There should be an out of stock item in the test database.");
            PurchaseResults result = purchaseController.Post(outOfStockItem.ItemId);

            // Assert
            Assert.AreEqual(PurchaseResults.OutOfStock, result, "API should have reported out of stock item.");
        }

        [TestMethod]
        public void IntegrationNonexistentItemIdReturnsItemNotFound()
        {
            // Arrange
            InventoryController inventoryController = new InventoryController();
            PurchaseController purchaseController = new PurchaseController();
            IEnumerable<Item> inventory = inventoryController.Get();
            int nonexistentItemId = 0;
            foreach (Item i in inventory)
            {
                if (i.ItemId >= nonexistentItemId)
                {
                    nonexistentItemId = i.ItemId + 1;
                }
            }

            // Act
            PurchaseResults result = purchaseController.Post(nonexistentItemId);

            // Assert
            Assert.AreEqual(PurchaseResults.ItemNotFound, result, "API should have reported item not found.");
        }
    }
}

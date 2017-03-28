using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRoseExpands.Tests.Mocks;
using GildedRoseExpands.Controllers;

namespace GildedRoseExpands.Tests.Controllers
{
    [TestClass]
    public class PurchaseControllerTest
    {
        private const int IN_STOCK_ITEM = 1;

        [TestMethod]
        public void PurchasingInStockItemDecreasesQuantity()
        {
            // Arrange
            InventoryServiceMock inventory = new InventoryServiceMock();
            PurchaseController controller = new PurchaseController(inventory);
            int beginningQuantity = inventory.GetItemQuantity(IN_STOCK_ITEM);

            // Act
            controller.Post(IN_STOCK_ITEM);

            // Assert
            int expectedValue = beginningQuantity - 1;
            int actualValue = inventory.GetItemQuantity(IN_STOCK_ITEM);
            Assert.AreEqual(expectedValue, actualValue, string.Format("Expected {0} of item {1} instead of {2}. Purchasing an item should decrement the quantity by one.", expectedValue, IN_STOCK_ITEM, actualValue));
        }
    }
}

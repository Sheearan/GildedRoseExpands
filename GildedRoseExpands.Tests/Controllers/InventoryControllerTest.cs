using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GildedRoseExpands.Controllers;
using GildedRoseExpands.Models;
using GildedRoseExpands.Tests.Mocks;

namespace GildedRoseExpands.Tests.Controllers
{
    [TestClass]
    public class InventoryControllerTest
    {
        [TestMethod]
        public void EmptyInventoryReturnsNoItems()
        {
            // Arrange
            InventoryServiceMock mockInventory = new InventoryServiceMock(new List<Item>());
            InventoryController controller = new InventoryController(mockInventory);

            // Act
            IEnumerable<Item> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void PopulatedInventoryReturnsSameItems()
        {
            // Arrange
            Item tunic = new Item { Name = "Fine Tunic", Description = "A sleeveless, knee-length garment.", Price = 49.95M };
            Item banana = new Item { Name = "Fine Banana", Description = "A curved, yellow fruit.", Price = 1.95M };
            InventoryServiceMock mockInventory = new InventoryServiceMock(new List<Item> { tunic, banana});
            InventoryController controller = new InventoryController(mockInventory);

            // Act
            IEnumerable<Item> result = controller.Get();

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
    }
}

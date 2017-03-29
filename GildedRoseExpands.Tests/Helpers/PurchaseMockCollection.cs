using GildedRoseExpands.Controllers;
using GildedRoseExpands.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GildedRoseExpands.Tests.Helpers
{
    struct PurchaseMockCollection
    {
        public InventoryServiceMock inventory { get; set; }
        public PaymentServiceMock payment { get; set; }
        public ShippingServiceMock shipping { get; set; }
        public LoggingServiceMock logging { get; set; }
        public PurchaseController controller { get; set; }
    }
}

using System;
using System.Collections.Generic;
using GildedRoseExpands.Interfaces;

namespace GildedRoseExpands.Tests.Mocks
{
    class ShippingServiceMock : IShippingService
    {
        private List<Tuple<int, string>> ordersToShip = new List<Tuple<int, string>>();
        public void shipItem(int itemId, string user)
        {
            ordersToShip.Add(new Tuple<int, string>(itemId, user));
        }

        internal List<Tuple<int, string>> getOrders()
        {
            return ordersToShip;
        }
    }
}

using System;
using GildedRoseExpands.Interfaces;

namespace GildedRoseExpands.Tests.Mocks
{
    class PaymentServiceMock : IPaymentService
    {
        bool returnValue;

        internal PaymentServiceMock(bool desiredReturnValue)
        {
            returnValue = desiredReturnValue;
        }

        public bool processPayment()
        {
            return returnValue;
        }
    }
}

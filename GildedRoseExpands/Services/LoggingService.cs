using System;
using GildedRoseExpands.Interfaces;

namespace GildedRoseExpands.Services
{
    public class LoggingService : ILoggingService
    {
        public void logString(string log)
        {
            // This is not the best place to store logs, but it's easy to swap out
            // It's also easy to add logging to multiple places
            Console.WriteLine(log);
        }
    }
}
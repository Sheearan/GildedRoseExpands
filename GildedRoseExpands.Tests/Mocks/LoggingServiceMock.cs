using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GildedRoseExpands.Interfaces;

namespace GildedRoseExpands.Tests.Mocks
{
    class LoggingServiceMock : ILoggingService
    {
        private List<string> logEntries = new List<string>();

        public void logString(string log)
        {
            logEntries.Add(log);
        }

        internal List<string> GetLogEntries()
        {
            return logEntries;
        }
    }
}

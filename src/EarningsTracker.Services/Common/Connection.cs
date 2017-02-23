using System;

namespace EarningsTracker.Services.Common
{
    public class Connection : IConnection
    {
        public Connection(string connectionString)
        {
            // must use a guard clause to ensure something is injected
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString", "Connection expects constructor injection for connectionString param.");

            // we have a value by now so assign it
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }

    }
}

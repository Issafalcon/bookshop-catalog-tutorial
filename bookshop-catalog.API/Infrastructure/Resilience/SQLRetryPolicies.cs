using Microsoft.Data.SqlClient;
using Polly;
using Polly.Retry;

namespace bookshop_catalog.Infrastructure.Resilience
{
    internal static class SQLRetryPolicies
    {
        internal static AsyncRetryPolicy CreateSQLExceptionPolicy(int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        // TODO: Add suitable logging message with Serilog
                        Console.WriteLine($"SQLException: {exception.GetType()} with message {exception.Message} detected on attempt {retry} of {retries}");

                    }
                );
        }

        internal static RetryPolicy CreateDBSeedingPolicy(int retries = 3)
        {
            // TODO: Adjust timings here as SQL container takes a while to initialize when first setting up
            // Can't seed the DB until migration has been performed
            return Policy.Handle<SqlException>()
                .WaitAndRetry(new TimeSpan[]
                {
                    TimeSpan.FromSeconds(4),
                    TimeSpan.FromSeconds(8),
                    TimeSpan.FromSeconds(12),
                    TimeSpan.FromSeconds(24)
                });
        }
    }
}

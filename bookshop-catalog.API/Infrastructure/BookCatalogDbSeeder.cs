using bookshop_catalog.Infrastructure.Resilience;
using bookshop_catalog.Models;

namespace bookshop_catalog.Infrastructure
{
    internal class BookCatalogDbSeeder
    {
        /// <summary>
        /// Seeds the database with predefined books if none exist already
        /// </summary>
        /// <param name="context">The EF Core DB context</param>
        /// <returns></returns>
        internal async Task SeedAsync(BookContext context)
        {
            var policy = SQLRetryPolicies.CreateSQLExceptionPolicy();

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Books.Any())
                {
                    await context.Books.AddRangeAsync(new List<Book>()
                    {
                        new Book() { Author = "A. A. Milne", Title = "Winnie-the-pooh", Price = 19.25 },
                        new Book() { Author = "Jane Austin", Title = "Pride and Prejudice", Price = 19.25 },
                        new Book() { Author = "William Shakespeare", Title = "Romea and Juliet", Price = 19.25 }
                    });

                    await context.SaveChangesAsync();
                }
            });
        }
    }
}

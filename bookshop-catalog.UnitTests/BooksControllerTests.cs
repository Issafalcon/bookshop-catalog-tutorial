using bookshop_catalog.Controllers;
using bookshop_catalog.Infrastructure;
using bookshop_catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace bookshop_catalog.UnitTests
{
    public class BooksControllerTests
    {
        private readonly DbContextOptions<BookContext> _dbOptions;

        public BooksControllerTests()
        {
            _dbOptions = new DbContextOptionsBuilder<BookContext>()
                .UseInMemoryDatabase("test")
                .Options;
            using var dbContext = new BookContext(_dbOptions);
            dbContext.AddRange(GetFakeCatalog());
            dbContext.SaveChanges();
        }
        [Fact]
        public async Task GetBooks_SuccessfullyPaginates()
        {
            //Arrange
            var pageSize = 2;
            var pageIndex = 1;

            var expectedItemsInPage = 2;
            var expectedTotalItems = 4;

            var catalogContext = new BookContext(_dbOptions);

            //Act
            var booksController = new BooksController(catalogContext);
            var actionResult = await booksController.GetBooksAsync(Sortby.Title, pageSize, pageIndex);

            //Assert
            Assert.IsType<ActionResult<PaginatedItemsViewModel<Book>>>(actionResult);
            var page = Assert.IsAssignableFrom<PaginatedItemsViewModel<Book>>(actionResult.Value);
            Assert.Equal(expectedTotalItems, page.Count);
            Assert.Equal(pageIndex, page.PageIndex);
            Assert.Equal(pageSize, page.PageSize);
            Assert.Equal(expectedItemsInPage, page.Data.Count());
        }

        private List<Book> GetFakeCatalog()
        {
            return new List<Book>()
            {
                new()
                {
                    ID = 1,
                    Title = "fakeItemA",
                    Author = "FakeAuthorA",
                    Price = 8,
                },
                new()
                {
                    ID = 2,
                    Title = "fakeItemB",
                    Author = "FakeAuthorA",
                    Price = 6,
                },
                new()
                {
                    ID = 3,
                    Title = "fakeItemC",
                    Author = "FakeAuthorC",
                    Price = 20,
                },
                new()
                {
                    ID = 4,
                    Title = "fakeItemD",
                    Author = "FakeAuthorB",
                    Price = 4,
                },
            };
        }
    }
}
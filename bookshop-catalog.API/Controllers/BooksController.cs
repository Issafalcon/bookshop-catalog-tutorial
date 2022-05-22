using bookshop_catalog.Infrastructure;
using bookshop_catalog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace bookshop_catalog.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BooksController : ControllerBase
    {
        private readonly BookContext _bookContext;

        public BooksController(BookContext context)
        {
            _bookContext = context;
            // TODO: Inject logger instance with Serilog
        }

        /// <summary>Creates a new book</summary>
        /// <param name="body">A JSON object that represents a book.</param>
        /// <returns>Created</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<CreateBookResponse> CreateBook([FromBody][BindRequired] Book body)
        {
            throw new NotImplementedException();
        }

        /// <summary>Returns a list of books. Sorted by title by default.</summary>
        /// <returns>Success</returns>
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<Book>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<Book>>> GetBooksAsync([FromQuery] Sortby? sortby, [FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _bookContext.Books.LongCountAsync();
            Func<Book, object> orderByExp;
            switch (sortby)
            {
                case Sortby.Author:
                    orderByExp = b => b.Author;
                    break;
                case Sortby.Price:
                    orderByExp = b => b.Price;
                    break;
                case Sortby.Title:
                    orderByExp = b => b.Title;
                    break;
                default:
                    orderByExp = b => b.Title;
                    break;
            }
            var itemsOnPage = _bookContext.Books
                .OrderBy(orderByExp)
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();

            var model = new PaginatedItemsViewModel<Book>(pageIndex, pageSize, totalItems, itemsOnPage);

            return model;
        }

        /// <summary>Update an existing book</summary>
        /// <param name="body">A JSON object that represents a book.</param>
        /// <returns>Success</returns>
        [HttpPut, Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task UpdateBookById([FromBody][BindRequired] Book body, [BindRequired] long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>Gets a book by id</summary>
        /// <returns>Success</returns>
        [HttpGet, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<Book> GetBookById([BindRequired] long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <summary>Deletes a book by id</summary>
        /// <returns>Success</returns>
        [HttpDelete, Route("{id}")]
        public Task DeleteBookById([BindRequired] long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
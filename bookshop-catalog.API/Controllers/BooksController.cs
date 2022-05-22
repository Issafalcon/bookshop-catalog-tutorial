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
        public async Task<ActionResult> CreateBookAsync([FromBody][Bind("Author,Price,Title")] Book body)
        {
            var book = new Book
            {
                Author = body.Author,
                Price = body.Price,
                Title = body.Title
            };

            _bookContext.Books.Add(book);

            await _bookContext.SaveChangesAsync();

            return Created(nameof(GetBookById), new { id = book.ID });
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
        [HttpPut, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateBookById([FromBody] Book body, long id)
        {
            var bookItem = await _bookContext.Books.SingleOrDefaultAsync(i => i.ID == id);

            if (bookItem == null)
            {
                return NotFound(new { Message = $"Item with id {id} not found." });
            }

            _bookContext.Books.Update(body);
            await _bookContext.SaveChangesAsync();

            return Created(nameof(GetBookById), new { id = body.ID });
        }

        /// <summary>Gets a book by id</summary>
        /// <returns>Success</returns>
        [HttpGet, Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Book>> GetBookById(long id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var item = await _bookContext.Books.SingleOrDefaultAsync(b => b.ID == id);

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        /// <summary>Deletes a book by id</summary>
        /// <returns>Success</returns>
        [HttpDelete, Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBookById([BindRequired] long id)
        {
            var book = _bookContext.Books.SingleOrDefault(x => x.ID == id);

            if (book == null)
            {
                return NotFound();
            }

            _bookContext.Books.Remove(book);

            await _bookContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
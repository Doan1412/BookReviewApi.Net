using BookReview.Model;
using BookReview.Services.BookServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.Controllers
{
    [ApiController]
    [Route("/book")]
    public class BookController : Controller
    {
        private readonly IBookServices bookServices;
        public BookController (IBookServices bookServices)
        {
            this.bookServices = bookServices;
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost("addBook")]
        public async Task<ActionResult<Book>> addBook(Book book)
        {
            return Ok(new { status = "success", data =  await bookServices.addBook(book) });
        }
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        [NonAction]
        public async Task<ActionResult<List<Book>>> deleteBook(long id)
        {
            return Ok(new { status = "success", data = await bookServices.deleteBook((int)id) });
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public async Task<ActionResult<List<Book>>> updateBook(long id,Book book)
        {
            return Ok(new { status = "success", data = await bookServices.updateBook((int)id, book) });
        }
        [Authorize]
        [HttpGet("all")]
        public async Task<ActionResult<List<Book>>> getListBook()
        {
            return Ok(new { status = "success", data = await bookServices.getAllBooks() });
        }
        [Authorize]
        [HttpGet("id = {id}")]
        public async Task<ActionResult<List<Book>>> getBookById(int id)
        {
            return Ok(new { status = "success", data = await bookServices.getBookById(id) });
        }
    }
}

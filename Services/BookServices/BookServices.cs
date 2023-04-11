using BookReview.Data;
using BookReview.Model;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Services.BookServices
{
    public class BookServices : IBookServices
    {
        private readonly DataContext _dataContext;
        public BookServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<Book>> addBook(Book book)
        {
            _dataContext.Books.Add(book);
            await _dataContext.SaveChangesAsync();
            return await _dataContext.Books.ToListAsync();
        }
        public async Task<List<Book>?> deleteBook(int id)
        {
            var book=await _dataContext.Books.FindAsync((long)id);
            if (book is null) return null;
            _dataContext.Books.Remove(book);
            await _dataContext.SaveChangesAsync();
            return await _dataContext.Books.ToListAsync();
        }
        public async Task<List<Book>> getAllBooks()
        {
            var books = await _dataContext.Books.ToListAsync();
            return books;
        }
        public async Task<Book?> getBookById(int id)
        {
            var book = await _dataContext.Books
                .Include(book => book.Reviews)
                .FirstOrDefaultAsync(book => book.Id == id);
            if (book is null) return null;
            return book;
        }
        public async Task<Book> updateBook(Book request)
        {
            var book = await _dataContext.Books.FindAsync(request.Id);
            if (book is null) return null;
            book.Description = request.Description;
            book.Price = request.Price;
            book.Category = request.Category;
            book.Reviews = request.Reviews;
            book.Tags = request.Tags;
            book.Title = request.Title;
            book.Amount = request.Amount;
            book.Author = request.Author;
            await _dataContext.SaveChangesAsync();
            return book;
        }
        public async Task<Book?> addReviewToBook(int id, Review review)
        {
            var book = await _dataContext.Books.Include(book => book.Reviews)
                .FirstOrDefaultAsync(book => book.Id == id);
            if (book is null) return null;
            book.Reviews.Add(review);
            await _dataContext.SaveChangesAsync();
            return book;
        }
    }
}

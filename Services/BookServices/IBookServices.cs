using BookReview.Model;

namespace BookReview.Services.BookServices
{
    public interface IBookServices
    {
        Task<List<Book>> addBook(Book book);
        Task<List<Book>?> deleteBook(int id);
        Task<List<Book>> getAllBooks();
        Task<Book?> getBookById(int id);
        Task<Book> updateBook(Book request);
        Task<Book?> addReviewToBook(int id, Review review);
    }
}

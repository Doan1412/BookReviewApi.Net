using BookReview.Data;
using BookReview.DTO;
using BookReview.Model;
using Microsoft.EntityFrameworkCore;

namespace BookReview.Services.ReviewServices
{
    public class ReviewServices : IReviewServices
    {
        private readonly DataContext _dataContext;
        public ReviewServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<Review> addReview(ReviewDto reviewDto)
        {
            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.Id == reviewDto.UserId);
            var book = await _dataContext.Books.FirstOrDefaultAsync(b => b.Id == reviewDto.BookId);
            Review review = new Review()
            {
                Book = book,
                User = user,
                CreatedDate = reviewDto.CreatedDate,
                Content = reviewDto.Content,
                Rating = reviewDto.Rating,
            };
            await _dataContext.AddAsync(review);
            await _dataContext.SaveChangesAsync();
            return review;
        }
        public async void removeReview(long id)
        {
            var review = await _dataContext.Reviews.FindAsync(id);
            if (review is null) return;
            _dataContext.Reviews.Remove(review);
            await _dataContext.SaveChangesAsync();
        }
        public async Task<List<Review>> getListReviewByBook(long bookId)
        {
            var reviews = await _dataContext.Reviews
                .Where(r => r.Book.Id == bookId)
                .ToListAsync();
            return reviews;
        }
        public async Task<Review> getReviewById(long id)
        {
            var review = await _dataContext.Reviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == id);
            return review;
        }
        public async Task<Review> updateReview(long id, ReviewDto reviewDto)
        {
            var review = await _dataContext.Reviews.FirstOrDefaultAsync(r => r.Id == id);
            if (review == null) return null;
            review.Content = reviewDto.Content;
            review.Rating = reviewDto.Rating;
            await _dataContext.SaveChangesAsync();
            return review;
        }
        
    }
}

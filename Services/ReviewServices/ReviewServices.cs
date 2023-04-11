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
    }
}

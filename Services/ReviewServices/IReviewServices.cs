using BookReview.DTO;
using BookReview.Model;

namespace BookReview.Services.ReviewServices
{
    public interface IReviewServices
    {
        Task<Review> addReview(ReviewDto reviewDto);
        void removeReview(long id);
        Task<List<Review>> getListReviewByBook(long bookId);
        Task<Review> getReviewById(long id);
        Task<Review> updateReview (long id,ReviewDto reviewDto);
    }
}

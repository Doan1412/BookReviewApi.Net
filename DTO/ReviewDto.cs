using BookReview.Model;

namespace BookReview.DTO
{
    public class ReviewDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public long BookId { get; set; }
    }
}

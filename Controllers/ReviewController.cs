using BookReview.DTO;
using BookReview.Model;
using BookReview.Services.ReviewServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.Controllers
{
    [ApiController]
    [Route("api/review")]
    public class ReviewController : Controller
    {
        private readonly IReviewServices _reviewServices;
        public ReviewController(IReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }
        [Authorize]
        [HttpGet("/bookId/")]
        public async Task<ActionResult<List<Review>>> getListReviewByBook(long bookId)
        {
            return Ok(new { status = "success", data = await _reviewServices.getListReviewByBook(bookId) });
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> getReviewById(long id)
        {
            var r = await _reviewServices.getReviewById(id);
            ReviewDto review = new ReviewDto()
            {
                UserId = r.User.Id,
                BookId = r.Book.Id,
                CreatedDate = r.CreatedDate,
                Content = r.Content,
                Rating = r.Rating,
                Id = id
            };
            return Ok(new { status = "success", data = review });
        }
        [Authorize]
        [HttpPost("postReview")]
        public async Task<ActionResult<Review>> addReview(ReviewDto reviewDto)
        {
            var r = await _reviewServices.addReview(reviewDto);
            ReviewDto review = new ReviewDto()
            {
                UserId = r.User.Id,
                BookId = r.Book.Id,
                CreatedDate = r.CreatedDate,
                Content = r.Content,
                Rating = r.Rating,
                Id = r.Id,
            };
            return Ok(new { status = "success", data = review });
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> removeReview(long Id)
        {
            return Ok(new { status = "success" });
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Review>> updateReview(long id, ReviewDto reviewDto)
        {
            return Ok(new {status = "success",data = await _reviewServices.updateReview(id,reviewDto)});
        }
    }
}

namespace BookReview.Model
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Amount { get; set; }
        public string ImageUr1 { get; set; }
        public string Tags { get; set; }
        public string Author { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}

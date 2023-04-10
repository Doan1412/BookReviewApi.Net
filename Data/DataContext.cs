using BookReview.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BookReview.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("SERVER=LAPTOP-1JU8QPGU;Database=Book;User id = kid; password = 365471;Integrated Security = True;Trusted_Connection=true;TrustServerCertificate=true");
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
    }
}

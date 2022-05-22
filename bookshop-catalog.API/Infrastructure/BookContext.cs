using bookshop_catalog.Infrastructure.EntityConfiguration;
using bookshop_catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace bookshop_catalog.Infrastructure
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new BookEntityTypeConfiguration());
        }
    }
}

using bookshop_catalog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookshop_catalog.Infrastructure.EntityConfiguration
{
    /// <summary>
    /// Configuration for EF Core Entity representing a Book Item
    /// </summary>
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        /// <summary>
        /// Configuration implementation for the EF Core builder
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");
            builder.HasKey(b => b.ID);

            builder.Property(b => b.ID)
                .UseHiLo("book_hilo")
                .IsRequired();

            builder.Property(b => b.Author)
                .IsRequired();

            builder.Property(b => b.Price)
                .IsRequired();

            builder.Property(b => b.Title)
                .IsRequired();
        }
    }
}

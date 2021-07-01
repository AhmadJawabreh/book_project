namespace Data
{
    using Entities;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
                .HasOne(item => item.Book)
                .WithMany(item => item.BookAuthors)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookAuthor>().HasKey(item => new { item.BookId, item.AuthorId });
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<BookAuthor> BookAuthors { get; set; }
    }
}

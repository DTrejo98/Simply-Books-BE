using Microsoft.EntityFrameworkCore;
using SimplyBooks.Models;  // Make sure to adjust the namespace as needed


public class SimplyBooksDbContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    public SimplyBooksDbContext(DbContextOptions<SimplyBooksDbContext> context) : base(context)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId);

        modelBuilder.Entity<Author>()
            .Property(a => a.FirstName)
            .IsRequired();

        modelBuilder.Entity<Author>()
            .Property(a => a.LastName)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .IsRequired();

        modelBuilder.Entity<Book>()
            .Property(b => b.Image);

        modelBuilder.Entity<Book>()
            .Property(b => b.Description);

        modelBuilder.Entity<Author>().HasData(
            new Author
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Favorite = true,
                Image = "http://example.com/john.jpg",
                Uid = "googleUid1"
            },
            new Author
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Favorite = false,
                Image = "http://example.com/jane.jpg",
                Uid = "googleUid2"
            }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                Id = 1,
                Title = "Learning C#",
                AuthorId = 1,
                Image = "http://example.com/learning-csharp.jpg",
                Description = "A great book to get started with C# programming.",
                Uid = "googleUid1"
            },
            new Book
            {
                Id = 2,
                Title = "Advanced C#",
                AuthorId = 1,
                Image = "http://example.com/advanced-csharp.jpg",
                Description = "An in-depth guide to advanced C# concepts.",
                Uid = "googleUid1"
            },
            new Book
            {
                Id = 3,
                Title = "Mastering .NET",
                AuthorId = 2,
                Image = "http://example.com/mastering-dotnet.jpg",
                Description = "A comprehensive guide to .NET framework and libraries.",
                Uid = "googleUid2"
            }
        );
    }
}



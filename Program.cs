using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimplyBooks.Models;
using System.Text.Json.Serialization;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddNpgsql<SimplyBooksDbContext>(builder.Configuration["SimplyBooksDbConnectionString"]);

builder.Services.Configure<MvcJsonOptions>(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authors Endpoints

// GET ALL AUTHORS
app.MapGet("/api/authors", (SimplyBooksDbContext db) =>
{
    return db.Authors.ToList();
});


// CREATE AUTHOR
app.MapPost("/api/authors", ([FromServices] SimplyBooksDbContext db, Author newAuthor) =>
{
    db.Authors.Add(newAuthor);
    db.SaveChanges();
    return Results.Created($"/api/authors/{newAuthor.Id}", newAuthor);
});


// UPDATE AUTHOR
app.MapPatch("/api/authors/{id}", ([FromServices] SimplyBooksDbContext db, int id, Author updatedAuthor) =>
{
    var author = db.Authors.FirstOrDefault(a => a.Id == id);
    if (author == null) return Results.NotFound();

    author.FirstName = updatedAuthor.FirstName;
    author.LastName = updatedAuthor.LastName;
    author.Email = updatedAuthor.Email;
    author.Favorite = updatedAuthor.Favorite;
    author.Image = updatedAuthor.Image;
    author.Uid = updatedAuthor.Uid;

    db.SaveChanges();
    return Results.Ok(author);
});


// Filter Favorite Authors (by UID)
app.MapGet("/api/favorite-authors", ([FromServices] SimplyBooksDbContext db, string uid) =>
{
    var authors = db.Authors.Where(a => a.Uid == uid && a.Favorite).ToList();
    return authors.Any() ? Results.Ok(authors) : Results.NotFound("No favorite authors found.");
});


// Book Endpoints

// GET ALL BOOKS
app.MapGet("/api/books", (SimplyBooksDbContext db) =>
{
    return db.Books.ToList();
});


// CREATE BOOK
app.MapPost("/api/books", ([FromServices] SimplyBooksDbContext db, Book newBook) =>
{
    db.Books.Add(newBook);
    db.SaveChanges();
    return Results.Created($"/api/books/{newBook.Id}", newBook);
});

// UPDATE BOOK
app.MapPatch("/api/books/{id}", ([FromServices] SimplyBooksDbContext db, int id, Book updatedBook) =>
{
    var book = db.Books.FirstOrDefault(b => b.Id == id);
    if (book == null) return Results.NotFound();

    book.Title = updatedBook.Title;
    book.Description = updatedBook.Description;
    book.Image = updatedBook.Image;
    book.Price = updatedBook.Price;
    book.Sale = updatedBook.Sale;
    book.AuthorId = updatedBook.AuthorId;  // If you're updating the author

    db.SaveChanges();
    return Results.Ok(book);
});


// GET BOOKS ON SALE for a specific user (filtered by UID)
app.MapGet("/api/books/on-sale", ([FromServices] SimplyBooksDbContext db, string uid) =>
{
    var books = db.Books.Where(b => b.Uid == uid && b.Sale == true).ToList();
    return books.Any() ? Results.Ok(books) : Results.NotFound("No books on sale found for this user.");
});


// 1. View Book Details (with Author)
app.MapGet("/api/books/{bookId}", async ([FromServices] SimplyBooksDbContext db, int bookId) =>
{
    var book = await db.Books
                       .Where(b => b.Id == bookId)
                       .Select(b => new
                       {
                           b.Id,
                           b.Title,
                           b.Description,
                           b.Price,
                           b.Sale,
                           Author = new
                           {
                               b.Author.Id,
                               b.Author.FirstName,
                               b.Author.LastName,
                               b.Author.Email,
                               b.Author.Favorite,
                               b.Author.Image
                           }
                       })
                       .FirstOrDefaultAsync();

    if (book == null)
    {
        return Results.NotFound($"Book with ID {bookId} not found.");
    }

    return Results.Ok(book);
});


// 2. View Author Details (with Books)
app.MapGet("/api/authors/{authorId}", async ([FromServices] SimplyBooksDbContext db, int authorId) =>
{
    var author = await db.Authors
                         .Where(a => a.Id == authorId)
                         .Select(a => new
                         {
                             a.Id,
                             a.FirstName,
                             a.LastName,
                             a.Email,
                             a.Favorite,
                             a.Image,
                             Books = db.Books
                                        .Where(b => b.AuthorId == authorId)
                                        .Select(b => new
                                        {
                                            b.Id,
                                            b.Title,
                                            b.Description,
                                            b.Price,
                                            b.Sale
                                        }).ToList()
                         })
                         .FirstOrDefaultAsync();

    if (author == null)
    {
        return Results.NotFound($"Author with ID {authorId} not found.");
    }

    return Results.Ok(author);
});



// 3. Delete Author and All Books by Author
app.MapDelete("/api/authors/{authorId}", async ([FromServices] SimplyBooksDbContext db, int authorId) =>
{
    var author = await db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == authorId);
    if (author == null)
    {
        return Results.NotFound($"Author with ID {authorId} not found.");
    }

    // Delete all books by the author
    db.Books.RemoveRange(author.Books);
    await db.SaveChangesAsync();

    // Delete the author
    db.Authors.Remove(author);
    await db.SaveChangesAsync();

    return Results.Ok($"Author with ID {authorId} and their books have been deleted.");
});

// 4. Delete a Single Book
app.MapDelete("/api/books/{bookId}", async ([FromServices] SimplyBooksDbContext db, int bookId) =>
{
    var book = await db.Books.FirstOrDefaultAsync(b => b.Id == bookId);
    if (book == null)
    {
        return Results.NotFound($"Book with ID {bookId} not found.");
    }

    db.Books.Remove(book);
    await db.SaveChangesAsync();

    return Results.Ok($"Book with ID {bookId} has been deleted.");
});

app.Run();

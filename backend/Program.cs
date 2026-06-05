using backend.Data;

//importerar alla klasser från models mappen. Utan 'using' behöver man skriva ut hela sökvägen
using backend.Models;
using Microsoft.EntityFrameworkCore;

//creates a web application with default configuration
var builder = WebApplication.CreateBuilder(args);

//skapar en AppDbContext och kopplar den till PostgreSQL
//AppDbContext kommer att tas emot som parameter i endpoints
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Builds the configured WebApplication instance
var app = builder.Build();

// GET hämtar alla böcker
app.MapGet("/", async (AppDbContext db) =>
    await db.Books.ToListAsync()
);

// POST skapa en bok
app.MapPost("/", async (AppDbContext db, Book book) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/{book.Id}", book);
});

// DELETE ta bort en bok
app.MapDelete("/{id}", async (AppDbContext db, int id) =>
{
});

app.Run();
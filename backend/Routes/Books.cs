using backend.Data;
using Microsoft.EntityFrameworkCore;
//importerar alla klasser från models mappen. Utan 'using' behöver man skriva ut hela sökvägen
using backend.Models;

namespace backend.Routes;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this WebApplication app)
    {
        // GET hämtar alla böcker
        app.MapGet("/", async (AppDbContext db) =>
            await db.Books.ToListAsync()
        );

        // POST skapa en bok
        // Mappar det inkommande JSON objektet till ett Book objekt
        app.MapPost("/", async (AppDbContext db, Book book) =>
        {
            db.Books.Add(book);
            await db.SaveChangesAsync();
            return Results.Created($"/{book.Id}", book);
        });

        // DELETE ta bort en bok
        /*  app.MapDelete("/{id}", async (AppDbContext db, int id) =>
         {
         }); */
    }
}
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
        app.MapGet("/book", async (AppDbContext db) =>
        {
            //Hämtar alla böcker från databasen och returnerar dem som JSON
            //databas kontext + dbSet + EF metod
            try
            {
                var books = await db.Books.ToListAsync();
                return Results.Ok(books); //200
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // GET hämtar en specifik bok
        app.MapGet("/book/{id}", async (AppDbContext db, int id) =>
        {
            try
            {
                var book = await db.Books.FindAsync(id);
                if (book is null) return Results.NotFound();

                return Results.Ok(book);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // POST skapa en bok
        // Mappar det inkommande JSON objektet till ett Book objekt genom Book book
        app.MapPost("/book", async (AppDbContext db, Book book) =>
        {
            try
            {
                db.Books.Add(book);
                await db.SaveChangesAsync();
                return Results.Created($"/{book.Id}", book); //201
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // PUT uppdaterar en bok
        /* app.MapPut("/book/{id}", async (AppDbContext db, int id) =>
        {
            try
            {
                // Om det inte finns en bok med det angivna id:t
                if (!await db.Books.AnyAsync(e => e.Id == id))
                {
                    return Results.NotFound();
                }

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }); */

        // DELETE ta bort en bok
        /* app.MapDelete("/{id}", async (AppDbContext db, int id) =>
        {
            try{}
            catch (){}
        }); */
    }
}
using backend.Data;
using Microsoft.EntityFrameworkCore;
//importerar alla klasser från models mappen. Utan 'using' behöver man skriva ut hela sökvägen
using backend.Models;

namespace backend.Routes;

public static class QuotesEndpoints
{
    public static void MapQuoteEndpoints(this WebApplication app)
    {
        // GET hämtar alla citat
        app.MapGet("/quote", async (AppDbContext db) =>
        {
            try
            {
                var quotes = await db.Quotes.ToListAsync();
                return Results.Ok(quotes); //200
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // GET hämtar ett specifikt citat
        app.MapGet("/quote/{id}", async (AppDbContext db, int id) =>
        {
            try
            {
                var quote = await db.Quotes.FindAsync(id);
                if (quote is null) return Results.NotFound();

                return Results.Ok(quote);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // POST skapar ett citat
        // Mappar det inkommande JSON objektet till ett Book objekt genom Book book
        app.MapPost("/quote", async (AppDbContext db, Quote quote) =>
        {
            try
            {
                db.Quotes.Add(quote);
                await db.SaveChangesAsync();
                return Results.Created($"/{quote.Id}", quote); //201
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // PUT uppdaterar ett citat
        app.MapPut("/quote/{id}", async (AppDbContext db, int id, Quote quote) =>
        {
            try
            {
                // Om det inte finns en bok med det angivna id:t
                // AnyAsync returnerar ett boolean värde medan FindAsync returnerar ett helt objekt
                if (!await db.Quotes.AnyAsync(e => e.Id == id)) return Results.NotFound();

                //Sätter bokens Id från URL:en så att EF Core vet vilken bok som ska uppdateras. Kommer annars skapa en ny resurs
                quote.Id = id;

                db.Quotes.Update(quote);
                await db.SaveChangesAsync();
                return Results.NoContent(); //204 

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // DELETE tar bort ett citat
        app.MapDelete("/quote/{id}", async (AppDbContext db, int id) =>
        {
            try
            {
                var quote = await db.Quotes.FindAsync(id);

                if (quote is null) return Results.NotFound();

                db.Quotes.Remove(quote);
                await db.SaveChangesAsync();
                return Results.NoContent(); //204 
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });
    }
}
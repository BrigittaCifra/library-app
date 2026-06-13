//importerar alla klasser från models mappen. Utan 'using' behöver man skriva ut hela sökvägen
using backend.Data;
using backend.Models;
using backend.Services;

//Microsoft imports
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace backend.Routes;

public static class QuotesEndpoints
{
    public static void MapQuoteEndpoints(this WebApplication app)
    {
        // GET hämtar alla citat
        app.MapGet("/quote", async (AppDbContext db, ClaimsPrincipal user) =>
        {
            try
            {
                var userId = PasswordService.GetUserId(user);
                if (userId == null) return Results.Unauthorized();

                var quotes = await db.Quotes.Where(q => q.UserId == userId).ToListAsync();
                return Results.Ok(quotes); //200
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization();

        // GET hämtar ett specifikt citat
        app.MapGet("/quote/{id}", async (AppDbContext db, ClaimsPrincipal user, int id) =>
        {
            try
            {
                var userId = PasswordService.GetUserId(user);
                if (userId == null) return Results.Unauthorized();

                var quote = await db.Quotes.FirstOrDefaultAsync(q => q.UserId == userId && q.UserId == userId);
                if (quote is null) return Results.NotFound();

                return Results.Ok(quote);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization();

        // POST skapar ett citat
        // Mappar det inkommande JSON objektet till ett Book objekt genom Book book
        app.MapPost("/quote", async (AppDbContext db, ClaimsPrincipal user, Quote quote) =>
        {
            try
            {
                var userId = PasswordService.GetUserId(user);
                if (userId == null) return Results.Unauthorized();

                quote.UserId = userId.Value;
                db.Quotes.Add(quote);
                await db.SaveChangesAsync();
                return Results.Created($"/{quote.Id}", quote); //201
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization();

        // PUT uppdaterar ett citat
        app.MapPut("/quote/{id}", async (AppDbContext db, ClaimsPrincipal user, int id, Quote quote) =>
        {
            try
            {
                var userId = PasswordService.GetUserId(user);
                if (userId == null) return Results.Unauthorized();

                // Om det inte finns en bok med det angivna id:t
                // AnyAsync returnerar ett boolean värde medan FindAsync returnerar ett helt objekt
                if (!await db.Quotes.AnyAsync(q => q.Id == id && q.UserId == userId)) return Results.NotFound();

                //Sätter bokens Id från URL:en så att EF Core vet vilken bok som ska uppdateras. Kommer annars skapa en ny resurs
                quote.Id = id;
                quote.UserId = userId.Value;

                db.Quotes.Update(quote);
                await db.SaveChangesAsync();
                return Results.NoContent(); //204 

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization();

        // DELETE tar bort ett citat
        app.MapDelete("/quote/{id}", async (AppDbContext db, ClaimsPrincipal user, int id) =>
        {
            try
            {
                var userId = PasswordService.GetUserId(user);
                if (userId == null) return Results.Unauthorized();

                var quote = await db.Quotes.FirstOrDefaultAsync(q => q.Id == id && q.UserId == userId);

                if (quote is null) return Results.NotFound();

                db.Quotes.Remove(quote);
                await db.SaveChangesAsync();
                return Results.NoContent(); //204 
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization();
    }
}
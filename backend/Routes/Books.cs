//importerar alla klasser från models mappen. Utan 'using' behöver man skriva ut hela sökvägen
using backend.Data;
using backend.Models;
using backend.Services;

//Microsoft imports
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace backend.Routes;

public static class BookEndpoints
{

    public static void MapBookEndpoints(this WebApplication app)
    {
        // GET hämtar alla böcker
        app.MapGet("/book", async (AppDbContext db, ClaimsPrincipal user) =>
        {
            //Hämtar alla böcker från databasen och returnerar dem som JSON
            //databas kontext + dbSet + EF metod
            try
            {
                //Hämtar user id:t från den inkomna JWT tokenen med hjälp av ClaimsPrincipal
                //ClaimsPrincipal skapas genom app.UseAuthentication() och utgår från claims och har därför också samma strukturs som claims
                var userId = PasswordService.GetUserId(user);
                if (userId == null) return Results.Unauthorized();

                //Hämtar all böcker för en användare
                var books = await db.Books.Where(b => b.UserId == userId).ToListAsync();
                return Results.Ok(books); //200
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }).RequireAuthorization();

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
        app.MapPut("/book/{id}", async (AppDbContext db, int id, Book book) =>
        {
            try
            {
                // Om det inte finns en bok med det angivna id:t
                // AnyAsync returnerar ett boolean värde medan FindAsync returnerar ett helt objekt
                if (!await db.Books.AnyAsync(e => e.Id == id)) return Results.NotFound();

                //Sätter bokens Id från URL:en så att EF Core vet vilken bok som ska uppdateras. Kommer annars skapa en ny resurs
                book.Id = id;

                db.Books.Update(book);
                await db.SaveChangesAsync();
                return Results.NoContent(); //204 

            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        // DELETE ta bort en bok
        app.MapDelete("/book/{id}", async (AppDbContext db, int id) =>
        {
            try
            {
                var book = await db.Books.FindAsync(id);

                if (book is null) return Results.NotFound();

                db.Books.Remove(book);
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
using backend.Data;
using backend.Services;
using backend.DTOs;
using Microsoft.EntityFrameworkCore;
//importerar alla klasser från models mappen. Utan 'using' behöver man skriva ut hela sökvägen
using backend.Models;

namespace backend.Routes;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {

        // POST skapar en användare
        // Mappar det inkommande JSON objektet till ett Book objekt genom Book book
        app.MapPost("/user/registration", async (AppDbContext db, User user) =>
        {
            try
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/user/{user.Id}", user); // 201
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        app.MapPost("/user/login", async (AppDbContext db, LoginRequest loginRequest, IConfiguration config) =>
        {
            try
            {
                //Kollar om användaren finns i databasen
                var user = await db.Users.FirstOrDefaultAsync(e => e.Email == loginRequest.Email);

                //Om användaren inte finns eller lösenordet är fel
                if (user is null || !PasswordService.VerifyPassword(loginRequest.Password, user.PasswordHash)) return Results.Unauthorized();

                //Genererar token
                var token = PasswordService.GenerateJwtToken(user, config);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        //Bearer i header (login post)

        //TBA: 
        //Inloggning
        //Auth

    }
}
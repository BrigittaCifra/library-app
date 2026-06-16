//importerar alla klasser från models mappen. Utan 'using' behöver man skriva ut hela sökvägen
using backend.Data;
using backend.Models;
using backend.DTOs;
using backend.Services;

using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace backend.Routes;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {

        // POST skapar en användare
        // Mappar det inkommande JSON objektet till ett Book objekt genom Book book
        app.MapPost("/user/register", async (AppDbContext db, RegisterRequest request, IConfiguration config) =>
        {
            try
            {
                //Användarnamn
                if (string.IsNullOrEmpty(request.Username)) return Results.BadRequest("Användarnamn saknas");
                //Kollar om användarnamnet är tagen. Kollar om den inskickade användarnamnet matchar med någon av användarnamnen i databasen
                if (await db.Users.AnyAsync(e => e.Username == request.Username)) return Results.BadRequest("Användarnamnet finns redan");

                //Mejl
                if (string.IsNullOrEmpty(request.Email)) return Results.BadRequest("Mejl saknas");
                //Kollar om användarnamnet är tagen. Kollar om den inskickade användarnamnet matchar med någon av användarnamnen i databasen
                if (await db.Users.AnyAsync(e => e.Email == request.Email)) return Results.BadRequest("Mejlet används redan");

                //Lösenord
                if (string.IsNullOrEmpty(request.Password)) return Results.BadRequest("Lösenord saknas");

                // Skapar en ny användare från request och hashar lösenordet
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = PasswordService.HashPassword(request.Password)
                };

                //Lägger till användaren i databasen
                db.Users.Add(user);
                await db.SaveChangesAsync();

                //AuthResponse modellen på frontenden förväntar sig ett token i responsen
                var token = PasswordService.GenerateJwtToken(user, config);
                return Results.Created($"/user/{user.Id}", new AuthResponse { Token = token, Username = user.Username, Email = user.Email }); // 201
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        //Login endpoint. Ska alltid vara POST
        app.MapPost("/user/login", async (AppDbContext db, LoginRequest loginRequest, IConfiguration config) =>
        {
            try
            {
                //Kollar om mejl fältet är tom
                if (string.IsNullOrEmpty(loginRequest.Email)) return Results.BadRequest("Mejl saknas");

                //Kollar om användaren finns i databasen
                var user = await db.Users.FirstOrDefaultAsync(e => e.Email == loginRequest.Email);

                //Kollar om lösenord fältet är tom
                if (string.IsNullOrEmpty(loginRequest.Password)) return Results.BadRequest("Lösenord saknas");

                //Om användaren inte finns eller lösenordet är fel
                if (user is null || !PasswordService.VerifyPassword(loginRequest.Password, user.PasswordHash)) return Results.BadRequest("Lösenordet eller mejlet är fel");

                //Genererar token
                var token = PasswordService.GenerateJwtToken(user, config);
                return Results.Ok(new AuthResponse { Token = token, Username = user.Username, Email = user.Email });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

    }
}
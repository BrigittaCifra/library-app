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
        app.MapPost("/user/register", async (AppDbContext db, RegisterRequest request) =>
        {
            try
            {

                if (string.IsNullOrEmpty(request.Username)) return Results.BadRequest("Användarnamn saknas");

                //Kollar om användarnamnet är tagen. Kollar om den inskickade användarnamnet matchar med någon av användarnamnen i databasen
                if (await db.Users.AnyAsync(e => e.Username == request.Username)) return Results.BadRequest("Användarnamnet finns redan");

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
                return Results.Created($"/user/{user.Id}", user); // 201
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
                //Kollar om användaren finns i databasen
                var user = await db.Users.FirstOrDefaultAsync(e => e.Email == loginRequest.Email);

                //Om användaren inte finns eller lösenordet är fel
                if (user is null || !PasswordService.VerifyPassword(loginRequest.Password, user.PasswordHash)) return Results.Unauthorized();

                //Genererar token
                var token = PasswordService.GenerateJwtToken(user, config);
                return Results.Ok(new AuthResponse { Token = token, Username = user.Username, Email = user.Email });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        });

        //Bearer i header (login post)

    }
}
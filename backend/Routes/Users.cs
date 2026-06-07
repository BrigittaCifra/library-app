using backend.Data;
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
        app.MapPost("/user", async (AppDbContext db, User user) =>
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

        //GET -

        //Bearer i header (login post)

        //TBA: 
        //Inloggning
        //Auth

    }
}
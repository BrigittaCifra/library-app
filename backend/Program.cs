using backend.Data;
using backend.Routes;

//Microsoft nugets
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//Sätter CORS policy namnet till _myAllowSpecificOrigins
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
//creates a web application with default configuration
var builder = WebApplication.CreateBuilder(args);

//JWT auth
//Talar om för .NET att JWT används för inlogg
builder.Services.AddAuthentication(options =>
{
    // Talar om att JWT Bearer är standard autentiseringsmetoden
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Talar om vad som händer när någon försöker nå en skyddad endpoint utan token
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    // Sätter standard schema för hela appen
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    // Sparar token så att den kan hämtas senare i koden
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //Kontrollerar att Issuer stämmer
        ValidateIssuer = true,
        //Kontrollerar att Audience stämmer
        ValidateAudience = true,
        //Kontrollerar att token inte har gått ut
        ValidateLifetime = true,
        //Kontrollerar att token är signerad med rätt nyckel
        ValidateIssuerSigningKey = true,
        //Vem som skapade token
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        //Vem token är till för
        ValidAudience = builder.Configuration["Authentication:Audience"],
        //verifierar att token inte manipulerats
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"]!)),
        //If you are working across different servers their clocks might be a little bit out of sync.
        //ClockSkew adds a little buffer for these cases so both devices can validate the token
        ClockSkew = TimeSpan.Zero

    };
});
//Talar om för .NET att man vill kunna skydda endpoints med Authorize
builder.Services.AddAuthorization();

//skapar en AppDbContext och kopplar den till PostgreSQL
//AppDbContext kommer att tas emot som parameter i endpoints
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//CORS inställningar
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy
                          .WithOrigins("http://example.com", "http://localhost:4200")
                          .AllowAnyHeader()
                          .WithMethods("PUT", "DELETE", "GET", "POST");
                      });
});

//Builds the configured WebApplication instance
var app = builder.Build();

//Middleware för autentisering och auktorisering
//Kollar om användaren har en giltig token
app.UseAuthentication();
//Kollar om användaren har tillgång till den specifika endpointen
app.UseAuthorization();

//Anropar CORS policyn
app.UseCors(MyAllowSpecificOrigins);

//endpoints
BookEndpoints.MapBookEndpoints(app);
QuotesEndpoints.MapQuoteEndpoints(app);
UserEndpoints.MapUserEndpoints(app);

app.Run();
using backend.Data;
using backend.Routes;
using Microsoft.EntityFrameworkCore;

//Sätter CORS policy namnet till _myAllowSpecificOrigins
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

//creates a web application with default configuration
var builder = WebApplication.CreateBuilder(args);

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

//Anropar CORS policyn
app.UseCors(MyAllowSpecificOrigins);

//endpoints
BookEndpoints.MapBookEndpoints(app);
QuotesEndpoints.MapQuoteEndpoints(app);

app.Run();
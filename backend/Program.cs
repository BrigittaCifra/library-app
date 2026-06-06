using backend.Data;

using backend.Routes;

using Microsoft.EntityFrameworkCore;

//creates a web application with default configuration
var builder = WebApplication.CreateBuilder(args);

//skapar en AppDbContext och kopplar den till PostgreSQL
//AppDbContext kommer att tas emot som parameter i endpoints
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Builds the configured WebApplication instance
var app = builder.Build();

//endpoints
BookEndpoints.MapBookEndpoints(app);
QuotesEndpoints.MapQuoteEndpoints(app);

app.Run();
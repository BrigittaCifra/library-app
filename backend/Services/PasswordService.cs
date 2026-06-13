using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using backend.Models;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services;

public static class PasswordService
{
    //Hashar lösenord med SHA256
    public static string HashPassword(string Password)
    {
        //Skapar en SHA256 "maskin" som kan hasha text och ser till att städa minnet efteråt
        using var sha256 = SHA256.Create();

        //Omvandlar lösenordet till bytes
        //ComputeHash() kör SHA256 på byten och returnerar en ny byte array
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password));
        //Omvandlar bytes till en läsbar sträng som kan sparas i databasen
        return Convert.ToBase64String(hashedBytes);

    }

    //Veriferar lösenordet genom att jämföra lösenordet mot hashade lösenordet
    // Tar emot lösenordet användaren skriver in och den hashade versionen som ligger sparad i databasen
    public static bool VerifyPassword(string password, string passwordHash)
    {
        //Hashar lösenordet som användaren skrev in
        var hashOfInput = HashPassword(password);
        // Jämför de två hasharna. Returnerar true om de matchar
        return hashOfInput == passwordHash;
    }

    //Generar JWT token
    public static string GenerateJwtToken(User user, IConfiguration configuration)
    {
        //Hämtar Authentication sektionen från appsettings.Development.json
        var jwtSettings = configuration.GetSection("Authentication");
        //Hämtar nyckeln. Om den saknas kastas ett fel direkt
        var secretKey = jwtSettings["Key"] ?? throw new InvalidOperationException("Key saknas");

        //Verktyget som kan skapa och läsa JWT tokens
        var tokenHandler = new JwtSecurityTokenHandler();
        //Omvandlar nyckeln från text till bytes
        var key = Encoding.UTF8.GetBytes(secretKey);

        //SecurityTokenDescriptor är tillför att beskriva hur tokens ska se ut
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                //A claim is a statement about an entity made by an issuer

                //Sparar användarens id i token
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                //Sparar användarnamnet i token
                new Claim(ClaimTypes.Name, user.Username),
                //Sparar emailen i token
                new Claim(ClaimTypes.Email, user.Email)
            }),

            //Token går ut om 1 timme
            Expires = DateTime.UtcNow.AddHours(1),
            //Vem som skapade tokenen
            Issuer = jwtSettings["Issuer"],
            //Vem token är till för
            Audience = jwtSettings["Audience"],

            //Signerar token med din hemliga nyckel
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        //Skapar token objektet baserat på tokenDescriptor
        var token = tokenHandler.CreateToken(tokenDescriptor);
        //Omvandlar token objektet till en sträng som skickas tillbaka
        return tokenHandler.WriteToken(token);
    }

    //Hämtar användar id:t från JWT tokenen
    //Returnerar antingen int eller null
    public static int? GetUserId(ClaimsPrincipal user)
    {
        //Hämtar användar id:t från den inkomna JWT tokenen med hjälp av ClaimsPrincipal
        //ClaimsPrincipal skapas genom app.UseAuthentication() och utgår från claims och har därför också samma strukturs som claims
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return userId != null ? int.Parse(userId) : null;
    }

}
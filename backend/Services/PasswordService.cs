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
        var jwtSettings = configuration.GetSection("Authentication");
        var secretKey = jwtSettings["Key"] ?? throw new InvalidOperationException("Key saknas");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
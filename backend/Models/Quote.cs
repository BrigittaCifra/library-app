using System.ComponentModel.DataAnnotations;

//Gruperar allt inom modles mappen och ger tillgång till using nyckelordet i andra filer så att man slipper skriva ut hela sökvägen
namespace backend.Models;

//Databas tabell
public class Quote
{
    //Primära nyckeln
    [Key]
    public int Id { get; init; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Koppling till Book
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public ICollection<UserQuote> UserQuotes { get; set; } = new List<UserQuote>();
}
//Importerar ett bibliotek från .NET
using System.ComponentModel.DataAnnotations;

//Gruperar allt inom modles mappen och ger tillgång till using nyckelordet i andra filer så att man slipper skriva ut hela sökvägen
namespace backend.Models;

//Grunden för databas tabellen som EF kommer att översätta till SQL
public class User
{
    //Primära nyckeln
    [Key]
    public int Id { get; init; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Koppling citat och böcker
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
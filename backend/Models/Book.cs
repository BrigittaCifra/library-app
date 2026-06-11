using System.ComponentModel.DataAnnotations;

//Gruperar allt inom modles mappen och ger tillgång till using nyckelordet i andra filer så att man slipper skriva ut hela sökvägen
namespace backend.Models;

//Databas tabell
public class Book
{
    //Primära nyckeln
    [Key]
    public int Id { get; set; }
    public required string Title { get; set; }
    public string Author { get; set; } = string.Empty;
    public DateOnly PublishedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Koppling till användare
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
using System.ComponentModel.DataAnnotations;

//Gruperar allt inom modles mappen och ger tillgång till using nyckelordet i andra filer så att man slipper skriva ut hela sökvägen
namespace backend.Models;

//Databas tabell
public class Book
{
    //Primära nyckeln
    [Key]
    public int Id { get; init; }
    public required string Title { get; set; }
    public string Author { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
}
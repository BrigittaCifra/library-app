using System.ComponentModel.DataAnnotations;

//Gruperar allt inom modles mappen och ger tillgång till using nyckelordet i andra filer så att man slipper skriva ut hela sökvägen
namespace backend.Models;

//Databas tabell
public class UserQuote
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int QuoteId { get; set; }
    public Quote Quote { get; set; } = null!;
}
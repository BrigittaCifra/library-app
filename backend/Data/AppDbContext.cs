//importerar alla klasser från models mappen. Utan 'using' behöver man skriva ut hela sökvägen
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

//AppDbContext ärver från DbContext och innehåller alla DbSet
//Varje DbSet<> motsvarar en tabell i databasen

public class AppDbContext : DbContext
{
    //Kunstruktorn för AppDbContext klassen. körs när ett nytt AppDbContext objekt skapas
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //"brygga" till databasen. dbSet är en EF klass som representerar en tabell i databasen
    //<...> refererar till klasser i models mappen. Det talar om för DbSet vilken typ av objekt tabellen innehåller
    //Books exempelvis används för att namnge själva tabellerna
    //{ get; set; } innebär att egenskapen både kan läsas och skrivas
    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<Quote> Quotes { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

}

//migrations är c# filer som kör SQL
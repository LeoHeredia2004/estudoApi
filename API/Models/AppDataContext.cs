using Microsoft.EntityFrameworkCore; 

namespace API.Models;

public class AppDataContext : DbContext
{
    public DbSet<Time> Times  { get; set; }
    public DbSet<Jogador> Jogadores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=banco.db");
    }

    internal Time? Find(int? idTime)
    {
        throw new NotImplementedException();
    }
}

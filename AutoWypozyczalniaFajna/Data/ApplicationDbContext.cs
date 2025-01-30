using AutoWypozyczalniaFajna.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Marka> Marka { get; set; }
    public DbSet<TypPaliwa> TypPaliwa { get; set; }
    public DbSet<Samochod> Samochod { get; set; }
    public DbSet<Wypozyczenie> Wypozyczenie { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // This is important to configure Identity tables correctly

        modelBuilder.Entity<Samochod>()
            .Property(s => s.CenaZaDzien)
            .HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Wypozyczenie>()
        .Property(w => w.CenaCalkowita)
        .HasColumnType("decimal(18,2)");  // Adjust precision and scale as needed
        modelBuilder.Entity<Wypozyczenie>()
       .Property(w => w.CenaCalkowita)
       .HasColumnType("decimal(18, 2)");

        // You can add other custom model configurations here if needed
    }
}

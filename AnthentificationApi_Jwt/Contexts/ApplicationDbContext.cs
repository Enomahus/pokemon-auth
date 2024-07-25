using AnthentificationApi_Jwt.Models;
using Azure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;

namespace AnthentificationApi_Jwt.Contexts;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Pokemon> Pokemons { get; set; }
    public DbSet<Types> Types { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<Pokemon>(entity =>
        //{
        //    entity.HasKey(e => e.Id);
        //    entity.Property(e => e.Id).ValueGeneratedOnAdd();
        //    entity.HasMany(e => e.Types)
        //         .WithMany(p => p.Pokemons)
        //         .UsingEntity<PokemonType>(
        //            j => j.HasOne(pt => pt.Types).WithMany(t => t.PokemonTypes).HasForeignKey(pt => pt.TypesId),
        //            j => j.HasOne(pt => pt.Pokemons).WithMany(p => p.PokemonTypes).HasForeignKey(pt => pt.PokemonsId),
        //            j =>
        //            {
        //                j.HasKey(t => new { t.PokemonsId, t.TypesId });
        //            });
        //});

        //modelBuilder.Entity<Types>(entity =>
        //{
        //    entity.HasKey(t => t.Id);
        //    entity.Property(t => t.Id).ValueGeneratedOnAdd();

        //});     

        modelBuilder.Entity<Pokemon>()
        .HasMany(e => e.Types)
        .WithMany(e => e.Pokemons)
        .UsingEntity(
            "PokemonType",
            l => l.HasOne(typeof(Types)).WithMany().HasForeignKey("TypeId").HasPrincipalKey(nameof(Models.Types.Id)),
            r => r.HasOne(typeof(Pokemon)).WithMany().HasForeignKey("PokemonId").HasPrincipalKey(nameof(Pokemon.Id)),
            j => j.HasKey("PokemonId", "TypeId"));


    }
}

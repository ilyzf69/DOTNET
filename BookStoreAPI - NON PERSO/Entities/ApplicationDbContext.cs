using System;
using System.ComponentModel;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace BookStoreAPI.Entities;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Surcharge de la méthode OnConfiguring qui me permet de specifier la connection string à utiliser
    // pour la connexion à la base de données
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // recupere le chemin du dossier courant lors de l'execution de l'application
        var currentDir = Directory.GetCurrentDirectory();

        // combine le chemin du dossier courant avec le nom du fichier de la base de données
        var dbPath = Path.Combine(currentDir, "Bookstore.db");
        Console.WriteLine($"dbPath: {dbPath}");
        optionsBuilder.UseSqlite($"Filename={dbPath}");

    }


    // DbSet est une classe générique, chaque Dbset correspond à une table dans la base de données
    public DbSet<Book> Books { get; set; } = default!;
}
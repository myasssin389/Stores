using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Stores.Models;

namespace Stores.Data;

public class StoresDbContext : IdentityDbContext<ApplicationUser>
{
    public StoresDbContext(DbContextOptions<StoresDbContext> options) 
        : base(options)
    {
        
    }

    public StoresDbContext()
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = "Server=localhost;Port=3306;Database=StoresDB;Uid=root;Password=m1sql2m@2341com";
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
    
    public DbSet<ApplicationUser> Users { get; set; }
    
    public DbSet<Store> Stores { get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Category> Categories { get; set; }
}
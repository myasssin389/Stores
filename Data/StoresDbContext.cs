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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<CartProductMap>().HasKey(cpm => new { cpm.ProductId, cpm.CartId });
        builder.Entity<OrderItem>().HasKey(oi => new { oi.ProductId, oi.OrderId });
        builder.Entity<ApplicationUser>()
            .HasOne(a => a.Cart)
            .WithOne(c => c.User)
            .HasForeignKey<Cart>(c => c.UserId);
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
    
    public DbSet<Cart> Carts { get; set; }
    
    public DbSet<CartProductMap> CartProductMaps { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderItem> OrderItems { get; set; }
    
    public DbSet<Address> Addresses { get; set; }
    
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    
    public DbSet<VerificationStatus> VerificationStatuses { get; set; }
    
    public DbSet<StoreAccountApplication> StoreAccountApplications { get; set; }
}
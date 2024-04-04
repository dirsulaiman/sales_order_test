namespace sales_api.Models.DbContext;

using Microsoft.EntityFrameworkCore;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Customer> Customer { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Price> Price { get; set; }
    public DbSet<SalesOrder> SalesOrder { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>().HasKey(c => c.CustId);
        modelBuilder.Entity<Product>().HasKey(c => c.ProductCode);
        modelBuilder.Entity<SalesOrder>().HasKey(c => c.SalesOrderNo);
        
        modelBuilder.Entity<Price>()
            .HasOne(p => p.Product)
            .WithMany(p => p.Prices)
            .HasForeignKey(p => p.ProductCode)
            .IsRequired();
    }
}
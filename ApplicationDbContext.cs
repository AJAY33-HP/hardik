using Microsoft.EntityFrameworkCore;
using wipmanagement.api.Models;


namespace wipmanagement.api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Warehouse> Warehouses { get; set; } = null!;
        public DbSet<Rack> Racks { get; set; } = null!;
        public DbSet<WipInventory> WipInventories { get; set; } = null!;
        public DbSet<CheckIn> CheckIns { get; set; } = null!;
        public DbSet<CheckOut> CheckOuts { get; set; } = null!;
        public DbSet<Shift> Shifts { get; set; } = null!;
        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<Prediction> Predictions { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<AuditHistory> AuditHistories { get; set; } = null!;
        public DbSet<CheckoutRequest> CheckoutRequests { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeCode)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.ProductCode)
                .IsUnique();

            modelBuilder.Entity<Warehouse>()
                .HasMany(w => w.Racks)
                .WithOne(r => r.Warehouse)
                .HasForeignKey(r => r.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rack>()
                .HasMany(r => r.WipInventories)
                .WithOne(w => w.Rack)
                .HasForeignKey(w => w.RackId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.WipInventories)
                .WithOne(w => w.Product)
                .HasForeignKey(w => w.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WipInventory>()
                .HasMany(w => w.CheckIns)
                .WithOne(c => c.WipInventory)
                .HasForeignKey(c => c.WipInventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WipInventory>()
                .HasMany(w => w.CheckOuts)
                .WithOne(c => c.WipInventory)
                .HasForeignKey(c => c.WipInventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuditHistory>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.AuditHistories)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Employee)
                .WithMany()
                .HasForeignKey("EmployeeId")
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.GeneratedBy)
                .WithMany()
                .HasForeignKey("GeneratedByEmployeeId")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}



using Microsoft.EntityFrameworkCore;
using NEXUS_API.Models;

namespace NEXUS_API.Data
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.EquipmentType)
                .WithMany(t => t.Equipments)
                .HasForeignKey(e => e.TypeId);
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Discount)
                .WithMany(d => d.Equipments)
                .HasForeignKey(e => e.DiscountId);
            modelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.HasIndex(e => e.TypeName).IsUnique(true);
            });
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasIndex(d => d.EquipmentName).IsUnique(true);
            });
        }
    }
}

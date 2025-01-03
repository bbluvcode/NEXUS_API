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
        //public DbSet<Account> Accounts { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<ConnectionDiary> ConnectionDiarys { get; set; }
        public DbSet<CustomerRequest> CustomerRequests { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<InStockOrder> InStockOrders { get; set; }
        public DbSet<InStockOrderDetail> InStockOrderDetails { get; set; }
        public DbSet<InStockRequest> InStockRequests { get; set; }
        public DbSet<InStockRequestDetail> InStockRequestDetails { get; set; }
        public DbSet<News> NewsTB { get; set; }
        public DbSet<OutStockOrder> OutStockOrders { get; set; }
        public DbSet<OutStockOrderDetail> OutStockOrderDetails { get; set; }
        //public DbSet<Plan> Plans { get; set; }
        public DbSet<PlanFee> PlanFees { get; set; }
        //public DbSet<Region> Regions { get; set; }
        public DbSet<RetainShop> RetainShops { get; set; }
        public DbSet<ServiceBill> ServiceBills { get; set; }
        public DbSet<ServiceBillDetail> ServiceBillDetails { get; set; }
        public DbSet<ServiceOrder> ServiceOrders { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
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

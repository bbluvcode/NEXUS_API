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
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Customer> Customers { get; set; }
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
        public DbSet<PlanFee> PlanFees { get; set; }
        public DbSet<Plan> Plans { get; set; }    
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
            //Customer
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.CustomerRequests)
                .WithOne(cr => cr.Customer)
                .HasForeignKey(cr => cr.CustomerId);
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.ServiceOrders)
                .WithOne(so => so.Customer)
                .HasForeignKey(so => so.CustomerId);
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.FeedBacks)
                .WithOne(fb => fb.Customer)
                .HasForeignKey(fb => fb.CustomerId);

            // Employee
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeeRole)
                .WithMany(er => er.Employees)
                .HasForeignKey(e => e.EmployeeRoleId);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.RetainShop)
                .WithMany(rs => rs.Employees)
                .HasForeignKey(e => e.RetainShopId);
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.InStockOrders)
                .WithOne(iso => iso.Employee)
                .HasForeignKey(iso => iso.EmployeeId);
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.OutStockOrders)
                .WithOne(oso => oso.Employee)
                .HasForeignKey(oso => oso.EmployeeId);
            modelBuilder.Entity<Employee>()
                .HasMany(e => e.NewsList)
                .WithOne(n => n.Employee)
                .HasForeignKey(n => n.EmployeeId);

            // RetainShop
            modelBuilder.Entity<RetainShop>()
                .HasOne(rs => rs.Stock)
                .WithOne(s => s.RetainShop)
                .HasForeignKey<Stock>(s => s.RetainShopId);

            // Connection
            modelBuilder.Entity<Connection>()
                .HasOne(c => c.ServiceOrder)
                .WithMany(so => so.Connections)
                .HasForeignKey(c => c.ServiceOrderId);
            modelBuilder.Entity<Connection>()
                .HasOne(c => c.Plan)
                .WithMany(p => p.Connections)
                .HasForeignKey(c => c.PlanId);
            modelBuilder.Entity<Connection>()
                .HasMany(c => c.ConnectionDiaries)
                .WithOne(cd => cd.Connection)
                .HasForeignKey(cd => cd.ConnectionId);

            // Plan
            modelBuilder.Entity<Plan>()
                .HasMany(p => p.PlanFees)
                .WithOne(pf => pf.Plan)
                .HasForeignKey(pf => pf.PlanId);

            // Equipment
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.EquipmentType)
                .WithMany(et => et.Equipments)
                .HasForeignKey(e => e.EquipmentTypeId);
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Vendor)
                .WithMany(v => v.Equipments)
                .HasForeignKey(e => e.VendorId);
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Stock)
                .WithMany(s => s.Equipments)
                .HasForeignKey(e => e.StockId);
            modelBuilder.Entity<Equipment>()
                .HasOne(e => e.Discount)
                .WithMany(d => d.Equipments)
                .HasForeignKey(e => e.DiscountId);

            // Stock
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Region)
                .WithMany(r => r.Stocks)
                .HasForeignKey(s => s.RegionId);
            modelBuilder.Entity<Stock>()
                .HasMany(s => s.InStockOrders)
                .WithOne(iso => iso.Stock)
                .HasForeignKey(iso => iso.StockId);
            modelBuilder.Entity<Stock>()
                .HasMany(s => s.OutStockOrders)
                .WithOne(oso => oso.Stock)
                .HasForeignKey(oso => oso.StockId);

            // InStockOrder
            modelBuilder.Entity<InStockOrder>()
                .HasOne(iso => iso.Employee)
                .WithMany(e => e.InStockOrders)
                .HasForeignKey(iso => iso.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InStockOrder>()
                .HasOne(iso => iso.InStockRequest)
                .WithMany(isr => isr.InStockOrders)
                .HasForeignKey(iso => iso.InStockRequestId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InStockOrder>()
                .HasOne(iso => iso.Stock)
                .WithMany(s => s.InStockOrders)
                .HasForeignKey(iso => iso.StockId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InStockOrder>()
                .HasOne(iso => iso.Vendor)
                .WithMany(v => v.InStockOrders)
                .HasForeignKey(iso => iso.VendorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InStockOrder>()
                .HasMany(iso => iso.InStockOrderDetails)
                .WithOne(isod => isod.InStockOrder)
                .HasForeignKey(isod => isod.InStockOrderId);

            // InStockOrderDetail
            modelBuilder.Entity<InStockOrderDetail>()
                .HasOne(isod => isod.Equipment)
                .WithMany(e => e.InStockOrderDetails)
                .HasForeignKey(isod => isod.EquipmentId);

            //InStockRequestDetail
            modelBuilder.Entity<InStockRequestDetail>()
                .HasOne(isrd => isrd.Equipment)
                .WithMany() 
                .HasForeignKey(isrd => isrd.EquipmentId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<InStockRequestDetail>()
                .HasOne(isrd => isrd.InStockRequest)
                .WithMany(isr => isr.InStockRequestDetails)
                .HasForeignKey(isrd => isrd.InStockRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // OutStockOrder
            modelBuilder.Entity<OutStockOrder>()
                .HasMany(oso => oso.OutStockOrderDetails)
                .WithOne(osod => osod.OutStockOrder)
                .HasForeignKey(osod => osod.OutStockId);
            modelBuilder.Entity<OutStockOrder>()
                .HasOne(oso => oso.Employee)
                .WithMany(e => e.OutStockOrders)
                .HasForeignKey(oso => oso.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OutStockOrder>()
                .HasOne(oso => oso.Stock)
                .WithMany(s => s.OutStockOrders)
                .HasForeignKey(oso => oso.StockId)
                .OnDelete(DeleteBehavior.Restrict);
            // OutStockOrderDetail
            modelBuilder.Entity<OutStockOrderDetail>()
                .HasOne(osod => osod.Equipment)
                .WithMany(e => e.OutStockOrderDetails)
                .HasForeignKey(osod => osod.EquipmentId);

            // ServiceOrder
            modelBuilder.Entity<ServiceOrder>()
                .HasMany(so => so.SupportRequests)
                .WithOne(sr => sr.ServiceOrder)
                .HasForeignKey(sr => sr.ServiceOrderId);

            //ServiceBill
            modelBuilder.Entity<ServiceBill>()
                .HasOne(sb => sb.ServiceOrder)
                .WithOne(so => so.ServiceBill)
                .HasForeignKey<ServiceBill>(sb => sb.ServiceOrderId);

            //ServiceBillDetail
            modelBuilder.Entity<ServiceBillDetail>()
                .HasOne(sbd => sbd.ServiceBill)
                .WithMany(sb => sb.ServiceBillDetails)
                .HasForeignKey(sbd => sbd.BillId);

            // News
            modelBuilder.Entity<News>()
                .HasOne(n => n.Employee)
                .WithMany(e => e.NewsList)
                .HasForeignKey(n => n.EmployeeId);

            //Region
            modelBuilder.Entity<Region>()
                .HasMany(r => r.RetainShops)
                .WithOne(rs => rs.Region) 
                .HasForeignKey(rs => rs.RegionId);

            //SupportRequest
            modelBuilder.Entity<SupportRequest>()
                .HasOne(sr => sr.Employee)
                .WithMany(e => e.SupportRequests)
                .HasForeignKey(sr => sr.EmployeeId);

            //Vendor
            modelBuilder.Entity<Vendor>()
                .HasOne(v => v.Region)
                .WithMany(r => r.Vendors)
                .HasForeignKey(v => v.RegionId);

            //EquipmentType
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

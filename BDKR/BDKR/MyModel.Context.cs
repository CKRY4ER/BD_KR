﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BDKR
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BDKREntities : DbContext
    {

        private static BDKREntities _context;
        public BDKREntities()
            : base("name=BDKREntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public static BDKREntities GetContext()
        {
            if (_context == null)
                _context = new BDKREntities();
            return _context;
        }
    
        public DbSet<Brand> Brand { get; set; }
        public DbSet<Buyer> Buyer { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Invoice> Invoice { get; set; }
        public DbSet<InvoicePosition> InvoicePosition { get; set; }
        public DbSet<OrderBuyer> OrderBuyer { get; set; }
        public DbSet<Passport> Passport { get; set; }
        public DbSet<PositionOrderBuyer> PositionOrderBuyer { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<StoregeRoom> StoregeRoom { get; set; }
        public DbSet<Sypply> Sypply { get; set; }
        public DbSet<SypplyPosition> SypplyPosition { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
    }
}
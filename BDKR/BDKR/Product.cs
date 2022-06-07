//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.InvoicePosition = new HashSet<InvoicePosition>();
            this.PositionOrderBuyer = new HashSet<PositionOrderBuyer>();
            this.StoregeRoom = new HashSet<StoregeRoom>();
            this.SypplyPosition = new HashSet<SypplyPosition>();
        }
    
        public int ProductId { get; set; }
        public string ArticleNumber { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int PricePerUnit { get; set; }
    
        public virtual Brand Brand { get; set; }
        public virtual ICollection<InvoicePosition> InvoicePosition { get; set; }
        public virtual ICollection<PositionOrderBuyer> PositionOrderBuyer { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<StoregeRoom> StoregeRoom { get; set; }
        public virtual ICollection<SypplyPosition> SypplyPosition { get; set; }
    }
}
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
    
    public partial class Buyer
    {
        public Buyer()
        {
            this.OrderBuyer = new HashSet<OrderBuyer>();
        }
    
        public int BuyerId { get; set; }
        public int PassportId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public System.DateTime Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string CardNumber { get; set; }
    
        public virtual Passport Passport { get; set; }
        public virtual ICollection<OrderBuyer> OrderBuyer { get; set; }
    }
}

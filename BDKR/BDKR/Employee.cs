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
    
    public partial class Employee
    {
        public Employee()
        {
            this.Sypply = new HashSet<Sypply>();
        }
    
        public int EmployeeId { get; set; }
        public int PostId { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
    
        public virtual Post Post { get; set; }
        public virtual ICollection<Sypply> Sypply { get; set; }
    }
}

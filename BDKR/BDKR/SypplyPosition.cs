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
    
    public partial class SypplyPosition
    {
        public int SypplyId { get; set; }
        public int ProductId { get; set; }
        public int CountProduct { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Sypply Sypply { get; set; }
    }
}

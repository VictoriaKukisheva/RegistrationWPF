//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RegistrationWPF.ApplicationData
{
    using System;
    using System.Collections.Generic;
    
    public partial class BuyMaterials
    {
        public int Id { get; set; }
        public int IdSales { get; set; }
        public int IdMaterial { get; set; }
        public int Count { get; set; }
        public Nullable<decimal> Discount { get; set; }
    
        public virtual Materials Materials { get; set; }
        public virtual Sale Sale { get; set; }
    }
}

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
    
    public partial class Materials
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Materials()
        {
            this.BuyMaterials = new HashSet<BuyMaterials>();
        }
    
        public int Id { get; set; }
        public string Article { get; set; }
        public int IdType { get; set; }
        public int IdUnit { get; set; }
        public decimal Cost { get; set; }
        public int MaxDiscount { get; set; }
        public int IdProvider { get; set; }
        public int IdSupplire { get; set; }
        public int IdCategoty { get; set; }
        public int Discount { get; set; }
        public Nullable<int> Existance { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyMaterials> BuyMaterials { get; set; }
        public virtual Categoty Categoty { get; set; }
        public virtual Providers Providers { get; set; }
        public virtual Suppliers Suppliers { get; set; }
        public virtual Types Types { get; set; }
        public virtual Units Units { get; set; }
    }
}
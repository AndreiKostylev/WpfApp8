//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp8
{
    using System;
    using System.Collections.Generic;
    
    public partial class metals
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public metals()
        {
            this.products = new HashSet<products>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public Nullable<int> supplier_id { get; set; }
        public string photo_path { get; set; }
    
        public virtual suppliers suppliers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<products> products { get; set; }
    }
}

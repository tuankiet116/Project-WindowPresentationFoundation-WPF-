//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyProject.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductTable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductTable()
        {
            this.InputDetailTable = new HashSet<InputDetailTable>();
            this.OutPutDetailTable = new HashSet<OutPutDetailTable>();
        }
    
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public int ID_Unit { get; set; }
        public int ID_Supplier { get; set; }
        public int Price { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InputDetailTable> InputDetailTable { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OutPutDetailTable> OutPutDetailTable { get; set; }
        public virtual SupplierTable SupplierTable { get; set; }
        public virtual UnitTable UnitTable { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KermesseApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_arqueocaja
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_arqueocaja()
        {
            this.tbl_arqueocaja_det = new HashSet<tbl_arqueocaja_det>();
        }
    
        public int id_arqueocaja { get; set; }
        public int id_kermesse { get; set; }
        public System.DateTime fecha_arqueo { get; set; }
        public decimal gran_total { get; set; }
        public int usuario_creacion { get; set; }
        public Nullable<System.DateTime> fecha_creacion { get; set; }
        public Nullable<int> usuario_modificacion { get; set; }
        public Nullable<System.DateTime> fecha_modificacion { get; set; }
        public Nullable<int> usuario_eliminacion { get; set; }
        public Nullable<System.DateTime> fecha_eliminacion { get; set; }
        public int estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_arqueocaja_det> tbl_arqueocaja_det { get; set; }
        public virtual tbl_kermesse tbl_kermesse { get; set; }
    }
}

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
    
    public partial class tbl_kermesse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_kermesse()
        {
            this.tbl_arqueocaja = new HashSet<tbl_arqueocaja>();
            this.tbl_gastos = new HashSet<tbl_gastos>();
            this.tbl_ingreso_com = new HashSet<tbl_ingreso_com>();
            this.tbl_listaprecio = new HashSet<tbl_listaprecio>();
        }
    
        public int id_kermesse { get; set; }
        public int id_parroquia { get; set; }
        public string nombre { get; set; }
        public System.DateTime fecha_inicio { get; set; }
        public System.DateTime fecha_fin { get; set; }
        public string desc_general { get; set; }
        public int estado { get; set; }
        public int usuario_creacion { get; set; }
        public Nullable<int> usuario_modificacion { get; set; }
        public Nullable<int> usuario_eliminacion { get; set; }
        public System.DateTime fecha_creacion { get; set; }
        public Nullable<System.DateTime> fecha_modificacion { get; set; }
        public Nullable<System.DateTime> fecha_eliminacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_arqueocaja> tbl_arqueocaja { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_gastos> tbl_gastos { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ingreso_com> tbl_ingreso_com { get; set; }
        public virtual tbl_parroquia tbl_parroquia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_listaprecio> tbl_listaprecio { get; set; }
        public virtual tbl_usuario tbl_usuario { get; set; }
        public virtual tbl_usuario tbl_usuario1 { get; set; }
        public virtual tbl_usuario tbl_usuario2 { get; set; }
    }
}

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
    
    public partial class tbl_comunidad
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_comunidad()
        {
            this.tbl_ingreso_com = new HashSet<tbl_ingreso_com>();
            this.tbl_productos = new HashSet<tbl_productos>();
        }
    
        public int id_comunidad { get; set; }
        public string nombre { get; set; }
        public string responsable { get; set; }
        public string desc_contribucion { get; set; }
        public int estado { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_ingreso_com> tbl_ingreso_com { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_productos> tbl_productos { get; set; }
    }
}
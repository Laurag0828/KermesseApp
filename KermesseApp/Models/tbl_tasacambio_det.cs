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
    
    public partial class tbl_tasacambio_det
    {
        public int id_tasaCambio_det { get; set; }
        public int id_tasaCambio { get; set; }
        public System.DateTime fecha { get; set; }
        public decimal tipoCambio { get; set; }
        public int estado { get; set; }
    
        public virtual tbl_tasacambio tbl_tasacambio { get; set; }
    }
}

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
    
    public partial class vw_ingresocomunidad
    {
        public int id_ingresocom_det { get; set; }
        public decimal total_bonos { get; set; }
        public Nullable<decimal> precio_producto { get; set; }
        public int cant_producto { get; set; }
        public int id_ingresocom { get; set; }
        public string kermesse { get; set; }
        public string comunidad { get; set; }
        public string producto { get; set; }
        public string bonos { get; set; }
        public string denominacion { get; set; }
        public int cantidad { get; set; }
        public decimal subtotal { get; set; }
        public int estado { get; set; }
        public int id_producto { get; set; }
        public int id_comunidad { get; set; }
        public int id_kermesse { get; set; }
        public int id_bono { get; set; }
    }
}

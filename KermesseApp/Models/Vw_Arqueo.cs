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
    
    public partial class Vw_Arqueo
    {
        public int id_kermesse { get; set; }
        public string nombre { get; set; }
        public int id_arqueocaja { get; set; }
        public decimal gran_total { get; set; }
        public System.DateTime fecha_arqueo { get; set; }
        public int id_moneda { get; set; }
        public string moneda { get; set; }
        public int id_Denominacion { get; set; }
        public decimal valor { get; set; }
        public int id_arqueocaja_det { get; set; }
        public decimal cantidad { get; set; }
        public decimal subtotal { get; set; }
        public int estado { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Friendshipets.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DetalleFacturacion
    {
        public int IDDetalle { get; set; }
        public int IDFactura { get; set; }
        public int IDProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public Nullable<decimal> Monto { get; set; }
    
        public virtual Facturacion Facturacion { get; set; }
        public virtual Productos Productos { get; set; }
    }
}

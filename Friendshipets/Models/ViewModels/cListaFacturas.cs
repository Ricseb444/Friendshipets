using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class cListaFacturas
    {
        public int IDFactura { get; set; }
        public int IDCliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public DateTime FechaFactura { get; set; }
        public decimal MontoTotal { get; set; }
        public string EstadoFactura { get; set; }

        // Columnas de la tabla DetalleFacturación
        public int IDDetalle { get; set; }
        public int IDProducto { get; set; }
        public int CantidadProductos { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Monto { get; set; }
    }
}
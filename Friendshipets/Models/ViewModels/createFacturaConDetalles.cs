using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class createFacturaConDetalles
    {
        public class cDetalleFactura
        {
            public int IDProducto { get; set; }
            public int Cantidad { get; set; }
        }
        public class cAgregarFacturaConDetalles
        {
            public int IDCliente { get; set; }
            public List<cDetalleFactura> Detalles { get; set; }

            public cAgregarFacturaConDetalles()
            {
                Detalles = new List<cDetalleFactura>();
            }
        }
    }
}
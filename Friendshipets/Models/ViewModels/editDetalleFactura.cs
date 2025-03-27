using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class editDetalleFactura
    {
        // Columnas de la tabla Facturación
        public int IDFactura { get; set; }

        // Columnas de la tabla DetalleFacturación        
        public int NuevaCantidad { get; set; }
    }
}
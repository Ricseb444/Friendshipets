using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class cListaDetalleCarrito
    {
        public int IDDetalleCarrito { get; set; }
        public int IDCarrito { get; set; }
        public int IDProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public Decimal Precio { get; set; }
        public Decimal Total { get; set; }
    }
}
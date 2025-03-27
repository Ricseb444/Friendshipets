using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class cListaProductos
    {
        public int IDProducto { get; set; }
        public decimal Precio { get; set; }
        public string NombreProducto { get; set; }
        public string ExistenciaProducto { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
        public string NombreProveedor { get; set; }
    }
}
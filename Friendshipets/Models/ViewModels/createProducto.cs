using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class createProducto
    {
        public string NombreProducto { get; set; }
        public string NombreProveedor { get; set; }
        public string Categoria { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public string ImgProducto { get; set; }
        public HttpPostedFileBase ImagenArchivo { get; set; } // Nueva propiedad para el archivo de imagen
    }

}
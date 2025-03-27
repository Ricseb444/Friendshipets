using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class editProducto
    {
        public int IDProducto { get; set; }

        [Required]
        [Display(Name = "Nombre Producto")]
        public string NombreProducto { get; set; }

        [Required]
        [Display(Name = "Nombre Proveedor")]
        public string NombreProveedor { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [Required]
        [Display(Name = "Stock Producto")]
        public int Stock { get; set; }

        [Required]
        [Display(Name = "Precio Producto:")]
        public decimal Precio { get; set; }
    }
}
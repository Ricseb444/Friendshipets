using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class createDetalleCarrito
    {
        [Required]
        [Display(Name = "ID Carrito")]
        public int IDCarrito { get; set; }

        [Required]
        [Display(Name = "ID Producto")]
        public int IDProducto { get; set; }

        [Required]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }
    }
}
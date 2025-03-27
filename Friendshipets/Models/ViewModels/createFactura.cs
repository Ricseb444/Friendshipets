using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class createFactura
    {
        // Columnas de la tabla Facturación
        [Required]
        [Display(Name = "DNI del Cliente:")]
        public int DNI_Cliente { get; set; }

        // Columnas de la tabla DetalleFacturación

        [Required]
        [Display(Name = "ID del Producto:")]
        public int IDProducto { get; set; }

        [Required]
        [Display(Name = "Cantidad del Producto:")]
        public int Cantidad { get; set; }
    }
}
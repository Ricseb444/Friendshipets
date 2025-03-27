using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Friendshipets.Models.ViewModels
{
    public class createProveedor
    {
        [Required]
        [Display(Name = "Nombre Proveedor")]
        public string NombreProveedor { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string Telefono { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
    }
}
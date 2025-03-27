using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class editProveedor
    {
        public int IDProveedor { get; set; }

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
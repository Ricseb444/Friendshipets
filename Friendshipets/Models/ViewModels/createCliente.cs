using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class createCliente
    {
        [Required]
        [Display(Name = "Identificación")]
        public int DNI_Cliente { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string NombreCliente { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string ApellidosCliente { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }
    }
}
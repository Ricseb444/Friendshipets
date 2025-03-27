using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class createUsuario
    {
        [Required]
        [Display(Name = "DNI del Cliente")]
        public int DNI_Cliente { get; set; }

        [Required]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Required]
        [Display(Name = "Correo Electronico")]
        public string CorreoUsuario { get; set; }

        [Required]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }

        [Required]
        [Display(Name = "Rol del Usuario")]
        public string RolUsuario { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Friendshipets.Models.ViewModels
{
    public class cListaUsuarios
    {
        [Display(Name = "ID del usuario")]
        public int IDUsuario { get; set; }
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; }
        [Display(Name = "Correo")]
        public string CorreoUsuario { get; set; }
        [Display(Name = "Rol")]
        public string RolUsuario { get; set; }
        [Display(Name = "ID de Cliente")]
        public int IDCliente { get; set; }
        [Display(Name = "Nombre Cliente")]
        public string NombreCliente { get; set; }
        [Display(Name = "Apellidos Cliente")]
        public string ApellidosCliente { get; set; }

    }
}
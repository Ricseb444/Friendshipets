using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Friendshipets.Models.ViewModels
{
    public class cListaClientes
    {
        [Display(Name = "ID Cliente")]
        public int IDCliente { get; set; }
        [Display(Name = "No. Identificación")]
        public int DNI_Cliente { get; set; }
        [Display(Name = "Nombre")]
        public string NombreCliente { get; set; }
        [Display(Name = "Apellidos")]
        public string ApellidosCliente { get; set; }
        [Display(Name = "")]
        public string EstadoCliente { get; set; }
        [Display(Name = "Dirección")]
        public string Direccion {  get; set; }
    }
}
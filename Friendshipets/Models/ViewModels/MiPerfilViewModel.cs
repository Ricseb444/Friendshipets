using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
	public class MiPerfilViewModel
	{
        // Solo lectura
        public int IDCliente { get; set; }
        public int DNI_Cliente { get; set; }
        public string CorreoUsuario { get; set; }

        // Editables
        public string NombreUsuario { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public string Direccion { get; set; }
    }
}
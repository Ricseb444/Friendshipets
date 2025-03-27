using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class cListaCarritos
    {
        public int IDCarrito { get; set; }
        public int IDCliente { get; set; }
        public int DNI_Cliente { get; set; }
        public string NombreCliente { get; set; }
        public string ApellidosCliente { get; set; }
        public int CantidadProductos { get; set; }
        public decimal MontoTotal { get; set; }
        public DateTime FechaAgregado { get; set; }
    }
}
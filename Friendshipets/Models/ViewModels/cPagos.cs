using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class cPagos
    {
        public int IDCarrito { get; set; }
        public List<cCarrito> Productos { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
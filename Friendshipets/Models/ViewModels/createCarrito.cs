using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class createCarrito
    {
        [Required]
        [Display(Name = "ID del Cliente")]
        public int IDCliente { get; set; }
    }
}
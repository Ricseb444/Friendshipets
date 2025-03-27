using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Friendshipets.Models.ViewModels
{
    public class editFactura
    {
        public int IDFactura { get; set; }
        [Required]
        [Display(Name = "DNI del Cliente:")]
        public int DNI_Cliente { get; set; }
    }
}
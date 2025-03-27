using System.ComponentModel.DataAnnotations;

namespace Friendshipets.Models.ViewModels
{
    public class RegistroViewModel
    {
        // Datos del Cliente
        [Required(ErrorMessage = "El DNI es obligatorio")]
        [Display(Name = "DNI")]
        public int DNI_Cliente { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        [Display(Name = "Nombre")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [Display(Name = "Apellidos")]
        public string ApellidosCliente { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        // Datos del Usuario
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [Display(Name = "Correo Electrónico")]
        public string CorreoUsuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Contrasena { get; set; }
    }
}


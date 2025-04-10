namespace Friendshipets.Models.ViewModels
{
    public class cCarrito
    {
        public int IDDetalleCarrito { get; set; }  // ID del detalle del carrito
        public int IDCarrito { get; set; }         // ID del carrito
        public int IDProducto { get; set; }        // ID del producto
        public string NombreProducto { get; set; } // Nombre del producto
        public decimal Precio { get; set; }        // Precio del producto
        public int Cantidad { get; set; }          // Cantidad del producto
        public decimal Total { get; set; }         // Total (Cantidad * Precio)

        // Información del cliente
        public int IDCliente { get; set; }         // ID del cliente
        public string NombreCliente { get; set; }  // Nombre del cliente
        public string ApellidosCliente { get; set; } // Apellidos del cliente
        public string CorreoCliente { get; set; }   // Correo del cliente
    }
}

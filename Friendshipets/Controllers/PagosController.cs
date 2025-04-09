using Friendshipets.Models;
using Friendshipets.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Friendshipets.Controllers
{
    public class PagosController : Controller
    {
        public ActionResult Index()
        {
            // Verificar si el usuario está logueado
            if (Session["IDUsuario"] == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            int idUsuario = (int)Session["IDUsuario"];
            int idCliente = 0;

            // Obtener el IDCliente del usuario logueado desde la base de datos
            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                var result = db.Database.SqlQuery<int?>(
                    "SELECT IDCliente FROM Usuarios WHERE IDUsuario = @IDUsuario",
                    new SqlParameter("@IDUsuario", idUsuario)
                ).FirstOrDefault();

                if (result.HasValue)
                {
                    idCliente = result.Value; // Asignar el valor si es encontrado
                }
            }

            // Si no se encuentra el IDCliente, redirigimos al login
            if (idCliente == 0)
            {
                ViewBag.Mensaje = "No se pudo encontrar el cliente asociado. Vuelve a intentar.";
                return RedirectToAction("Index", "Login");
            }

            // Obtener el carrito activo del cliente
            int idCarrito = 0;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                var carritos = db.Database.SqlQuery<cCarrito>(
                    "EXEC spLeerCarritos"
                ).Where(c => c.IDCliente == idCliente) // Filtramos para obtener el carrito asociado al cliente
                 .FirstOrDefault();

                // Si no se encuentra el carrito, creamos uno nuevo
                if (carritos == null)
                {
                    // Crear un carrito nuevo para el cliente
                    db.Database.ExecuteSqlCommand(
                        "EXEC spAgregarCarrito @IDCliente",
                        new SqlParameter("@IDCliente", idCliente)
                    );

                    // Obtener el nuevo carrito activo
                    carritos = db.Database.SqlQuery<cCarrito>(
                        "EXEC spLeerCarritos"
                    ).Where(c => c.IDCliente == idCliente) // Obtener el carrito recién creado
                     .FirstOrDefault();
                }

                // Si encontramos un carrito activo, asignamos su ID
                if (carritos != null)
                {
                    idCarrito = carritos.IDCarrito;
                }
            }

            // Si no existe un carrito, mostramos un mensaje
            if (idCarrito == 0)
            {
                ViewBag.Mensaje = "No tienes un carrito activo. Agrega productos al carrito.";
                return View(); // Retornamos la vista si no hay carrito activo
            }

            // Obtener los productos del carrito utilizando el procedimiento almacenado 'spMostrarDetalleCarrito'
            List<cCarrito> productos;
            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                productos = db.Database.SqlQuery<cCarrito>(
                    "EXEC spMostrarDetalleCarrito @IDCarrito",
                    new SqlParameter("@IDCarrito", idCarrito)
                ).ToList();
            }

            // Si el carrito está vacío, mostramos un mensaje
            if (productos == null || productos.Count == 0)
            {
                ViewBag.Mensaje = "Tu carrito está vacío. Agrega productos al carrito para continuar.";
            }

            // Calcular el monto total
            decimal montoTotal = productos.Sum(p => p.Total);

            // Crear el ViewModel para la vista de pagos
            cPagos pagosModel = new cPagos
            {
                IDCarrito = idCarrito,
                Productos = productos,
                MontoTotal = montoTotal
            };

            // Retornamos la vista con el modelo cPagos
            return View(pagosModel);
        }

        [HttpPost]
        public ActionResult ProcesarPago(int idCarrito)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spCrearFacturaConDetalles @IDCarrito",
                         new SqlParameter("@IDCarrito", idCarrito)
                    );
                }
                TempData["SuccessMessage"] = "Factura generada exitosamente.";
                return RedirectToAction("Index", "Pagos");
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Debug.WriteLine("Error interno: " + mensajeError);

                TempData["ErrorMessage"] = "Ocurrió un error al generar la factura: " + ex.Message;
                return RedirectToAction("Index", "Pagos");
            }
        }
    }
}
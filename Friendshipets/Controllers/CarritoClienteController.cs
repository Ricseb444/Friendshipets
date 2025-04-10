using Friendshipets.Models;
using Friendshipets.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Friendshipets.Controllers
{
    public class CarritoClienteController : Controller
    {
        public ActionResult CarritoIndex()
        {
            // Verificar si el usuario está logueado
            if (Session["IDUsuario"] == null)
            {
                // Si no está logueado, redirigir al login
                return RedirectToAction("Login", "Auth");
            }

            // Obtener el ID del usuario logueado
            int idUsuario = (int)Session["IDUsuario"];

            int idCliente = 0; // 0 para manejar el caso en que no se encuentra el cliente

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
                return RedirectToAction("Login", "Auth");
            }

            int idCarrito = 0;

            // Obtener el carrito activo del cliente usando el procedimiento almacenado 'spLeerCarritos'
            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                // Obtener el carrito activo usando el procedimiento spLeerCarritos
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

            List<cCarrito> productos;

            // Obtener los productos del carrito utilizando el procedimiento almacenado 'spMostrarDetalleCarrito'
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

            return View(productos); // Retornamos la lista de productos del carrito
        }
        // Acción para actualizar la cantidad de un producto en el carrito
        [HttpPost]
        public ActionResult ActualizarCantidad(int idCarrito, int idProducto, int cantidad)
        {
            if (cantidad <= 0)
            {
                return RedirectToAction("CarritoIndex"); // No permitir cantidades negativas o cero
            }

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                db.Database.ExecuteSqlCommand(
                    "EXEC spInsertarDetalleCarrito @IDCarrito, @IDProducto, @Cantidad",
                    new SqlParameter("@IDCarrito", idCarrito),
                    new SqlParameter("@IDProducto", idProducto),
                    new SqlParameter("@Cantidad", cantidad)
                );
            }

            return RedirectToAction("CarritoIndex");
        }
        // Acción para eliminar un producto del carrito
        [HttpPost]
        public ActionResult EliminarProducto(int idCarrito, int idProducto)
        {
            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                db.Database.ExecuteSqlCommand(
                    "EXEC spEliminarProductoCarrito @IDProducto, @IDCarrito",
                    new SqlParameter("@IDProducto", idProducto),
                    new SqlParameter("@IDCarrito", idCarrito)
                );
            }

            return RedirectToAction("CarritoIndex");
        }

        [HttpGet]
        // Acción para redirigir al proceso de pago
        public ActionResult ProcederPagar()
        {
            return RedirectToAction("Index", "Pagos");
        }

        [HttpGet]
        public ActionResult ObtenerCantidadCarrito()
        {
            // Verifica que el usuario esté logueado
            if (Session["IDUsuario"] == null)
            {
                return Json(0, JsonRequestBehavior.AllowGet); // Usuario no logueado = 0 productos
            }

            int idUsuario = (int)Session["IDUsuario"];
            int idCliente = 0;
            int cantidadTotal = 0;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                // Obtener IDCliente
                var result = db.Database.SqlQuery<int?>(
                    "SELECT IDCliente FROM Usuarios WHERE IDUsuario = @IDUsuario",
                    new SqlParameter("@IDUsuario", idUsuario)
                ).FirstOrDefault();

                if (!result.HasValue)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                idCliente = result.Value;

                // Obtener IDCarrito activo
                var carrito = db.Database.SqlQuery<cCarrito>(
                    "EXEC spLeerCarritos"
                ).FirstOrDefault(c => c.IDCliente == idCliente);

                if (carrito == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                int idCarrito = carrito.IDCarrito;

                // Obtener la suma total de cantidades del carrito
                cantidadTotal = db.Database.SqlQuery<int?>(
                    "SELECT SUM(Cantidad) FROM DetalleCarrito WHERE IDCarrito = @IDCarrito",
                    new SqlParameter("@IDCarrito", idCarrito)
                ).FirstOrDefault() ?? 0;
            }

            return Json(cantidadTotal, JsonRequestBehavior.AllowGet);
        }

    }
}
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
    public class CarritoController : Controller
    {
        // GET: Carrito
        public ActionResult Index()
        {
            List<cListaCarritos> listCarr;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                listCarr = db.Database.SqlQuery<cListaCarritos>("spLeerCarritos").ToList();
            }
            return View(listCarr);
        }

        // GET: Carrito/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrito/Create
        [HttpPost]
        public ActionResult Create(createCarrito carrito)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(carrito);
                }

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spAgregarCarrito @IDCliente",
                        new SqlParameter("@IDCliente", carrito.IDCliente)
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Carrito agregado correctamente";
                }
                return View(carrito);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al agregar el Carrito: " + ex.Message;

                return View(carrito);

            }
        }

        [HttpGet]
        public ActionResult IndexDetalleCarrito(int idCarrito)
        {
            ViewBag.IDCarrito = idCarrito;
            List<cListaDetalleCarrito> listCarr;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                listCarr = db.Database.SqlQuery<cListaDetalleCarrito>(
                    "EXEC spMostrarDetalleCarrito @IDCarrito",
                    new SqlParameter("@IDCarrito", idCarrito)
                ).ToList();
            }
            return View(listCarr);
        }

        [HttpGet]
        public ActionResult CreateDetalleCarrito(int idCarrito)
        {
            ViewBag.IDCarrito = idCarrito;
            return View();
        }

        // POST: DetalleCarrito/Create
        [HttpPost]
        public ActionResult CreateDetalleCarrito(createDetalleCarrito carrito)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(carrito);
                }

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spInsertarDetalleCarrito @IDCarrito, @IDProducto, @Cantidad",
                        new SqlParameter("@IDCarrito", carrito.IDCarrito),
                        new SqlParameter("@IDProducto", carrito.IDProducto),
                        new SqlParameter("@Cantidad", carrito.Cantidad)
                    );

                    ViewBag.IDCarrito = carrito.IDCarrito;
                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Producto agregado correctamente";
                }
                return View(carrito);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al agregar el Producto " + ex.Message;

                return View(carrito);
            }
        }

        [HttpGet]
        public ActionResult VaciarCarrito(int idCarrito)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spVaciarCarrito @IDCarrito",
                         new SqlParameter("@IDCarrito", idCarrito)
                    );
                }

                return RedirectToAction("Index", "Carrito");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al vaciar el carrito: " + ex.Message;
                return RedirectToAction("Index", "Carrito");
            }
        }

        [HttpGet]
        public ActionResult EliminarProductoCarrito(int idProducto, int idCarrito)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spEliminarProductoCarrito @IDProducto, @IDCarrito",
                         new SqlParameter("@IDProducto", idProducto),
                         new SqlParameter("@IDCarrito", idCarrito)
                    );
                }

                return RedirectToAction("IndexDetalleCarrito", "Carrito", new { idCarrito = idCarrito });
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Debug.WriteLine("Error interno: " + mensajeError);

                return RedirectToAction("IndexDetalleCarrito", "Carrito", new { idCarrito = idCarrito });
            }
        }

        [HttpGet]
        public ActionResult CrearFacturaConDetalles(int idCarrito)
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
                return RedirectToAction("IndexDetalleCarrito", "Carrito", new { idCarrito = idCarrito });
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Debug.WriteLine("Error interno: " + mensajeError);

                TempData["ErrorMessage"] = "Ocurrió un error al generar la factura: " + ex.Message;
                return RedirectToAction("IndexDetalleCarrito", "Carrito", new { idCarrito = idCarrito });
            }
        }
    }
}

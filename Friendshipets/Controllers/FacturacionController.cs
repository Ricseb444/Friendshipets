using Friendshipets.Models;
using Friendshipets.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Friendshipets.Models.ViewModels.createFacturaConDetalles;

namespace Friendshipets.Controllers
{
    public class FacturacionController : Controller
    {
        // GET: Facturacion
        public ActionResult Index()
        {
            List<cListaFacturas> listaFacturas;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                listaFacturas = db.Database.SqlQuery<cListaFacturas>("spLeerFacturas").ToList();
            }
            return View(listaFacturas);
        }

        // GET: Facturacion/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                string facturaCadena;

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    var outputParameter = new SqlParameter
                    {
                        ParameterName = "@FacturaCadena",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        Size = -1,
                        Direction = System.Data.ParameterDirection.Output
                    };

                    db.Database.ExecuteSqlCommand(
                        "EXEC spImprimirFactura @IDFactura, @FacturaCadena OUTPUT",
                        new SqlParameter("@IDFactura", id),
                        outputParameter
                     );

                    facturaCadena = outputParameter.Value.ToString();
                }

                ViewBag.FacturaCadena = facturaCadena;
                return View();
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Debug.WriteLine("Error interno: " + mensajeError);

                TempData["ErrorMessage"] = "Ocurrio un error al consultar la factura: " + ex.Message;
                return RedirectToAction("Index", "Facturacion");
            }
        }

        // GET: Facturacion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Facturacion/Create
        [HttpPost]
        public ActionResult Create(createFactura Fact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Fact);
                }

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spCrearFactura @DNI_Cliente, @IDProducto, @Cantidad",
                        new SqlParameter("@DNI_Cliente", Fact.DNI_Cliente),
                        new SqlParameter("@IDProducto", Fact.IDProducto),
                        new SqlParameter("@Cantidad", Fact.Cantidad)
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Factura creada correctamente";
                }
                return View(Fact);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Fallo al crear la factura" + ex.Message;

                return View(Fact);

            }
        }

        // GET: Facturacion/Edit/5
        public ActionResult Edit(int id)
        {
            editFactura Fact = new editFactura();

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                Fact = db.Database.SqlQuery<editFactura>(
                    "EXEC spLeerFacturaPorId @IDFactura",
                    new SqlParameter("@IDFactura", id)
                ).FirstOrDefault();
            }
            return View(Fact);
        }

        // POST: Facturacion/Edit/5
        [HttpPost]
        public ActionResult Edit(editFactura Fact)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(Fact);
                }
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spActualizarFactura @IDFactura, @DNI_Cliente",
                        new SqlParameter("@IDFactura", Fact.IDFactura),
                        new SqlParameter("@DNI_Cliente", Fact.DNI_Cliente)
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Factura actualizada correctamente";
                }
                return View(Fact);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al actualizar la factura " + ex.Message;

                return View(Fact);
            }
        }


        // POST: Facturacion/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spEliminarFactura @IDFactura",
                         new SqlParameter("@IDFactura", id)
                    );
                }

                return RedirectToAction("Index", "Facturacion");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar la factura: " + ex.Message;
                return RedirectToAction("Index", "Facturacion");
            }
        }

        [HttpGet]
        public ActionResult createFacturaConDetalles()
        {
            return View();
        }
        // Necesita Corrección
        [HttpPost]
        public ActionResult createFacturaConDetalles(cAgregarFacturaConDetalles facturaConDetalles)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(facturaConDetalles);
                }
                var detallesTable = new DataTable();
                detallesTable.Columns.Add("IDProducto", typeof(int));
                detallesTable.Columns.Add("Cantidad", typeof(int));

                if (facturaConDetalles.Detalles != null)
                {
                    foreach (var detalle in facturaConDetalles.Detalles)
                    {
                        detallesTable.Rows.Add(detalle.IDProducto, detalle.Cantidad);
                    }
                }
                else
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "No se enviaron detalles de la factura.";
                    return View(facturaConDetalles);
                }

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    var paramCliente = new SqlParameter("@IDCliente", facturaConDetalles.IDCliente);
                    var paramDetalles = new SqlParameter("@Detalles", SqlDbType.Structured)
                    {
                        TypeName = "DetalleFacturaType",
                        Value = detallesTable
                    };

                    db.Database.ExecuteSqlCommand(
                        "EXEC spCrearFacturaConDetalles @IDCliente, @Detalles",
                        paramCliente,
                        paramDetalles
                    );
                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Factura creada correctamente con sus detalles.";
                }
                return View(facturaConDetalles);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Fallo al crear la factura con detalles: " + ex.Message;
                return View(facturaConDetalles);
            }
        }
    }
}

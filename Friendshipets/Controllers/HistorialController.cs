using Friendshipets.Models.ViewModels;
using Friendshipets.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Friendshipets.Controllers
{
    public class HistorialController : Controller
    {
        [HttpGet]
        public ActionResult HistorialCompras()
        {
            try
            {
                var idUsuario = Session["IDUsuario"];
                ViewBag.IDUsuario = idUsuario;
                List<cListaFacturas> listFact;

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    listFact = db.Database.SqlQuery<cListaFacturas>(
                        "EXEC spLeerHistorialCliente @IDUsuario",
                        new SqlParameter("@IDUsuario", idUsuario)
                    ).ToList();
                }
                return View(listFact);            
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Debug.WriteLine("Error interno: " + mensajeError);

                TempData["ErrorMessage"] = "Ocurrio un error al consultar el historial: " + ex.Message;
                return RedirectToAction("Index", "Tienda");
            }             
        }

        public ActionResult ImprimirFactura(int id)
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
    }
}
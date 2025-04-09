using Friendshipets.Models;
using Friendshipets.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Friendshipets.Controllers
{
    public class CatalogoController : Controller
    {
        // GET: Catalogo
        public ActionResult Catalogo(string categoria = "")
        {
            List<cCatalogo> productosList;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                
                if (string.IsNullOrEmpty(categoria))
                {
                    productosList = db.Database.SqlQuery<cCatalogo>("EXEC spLeerProductos").ToList();
                }
                else
                {
                    productosList = db.Database.SqlQuery<cCatalogo>(
                        "EXEC spLeerProductosPorCategoria @Categoria",
                        new SqlParameter("@Categoria", categoria.ToUpper())
                    ).ToList();
                }
            }            

            return View(productosList);
        }

        [HttpPost]
        public ActionResult AgregarProducto(int idProducto, int cantidad)
        {
            try
            {
                if (Session["CarritoId"] == null)
                {
                    //TempData["ErrorMessage"] = "Carrito no encontrado. Por favor, inicie sesión.";
                    return RedirectToAction("Login", "Auth"); // Redirige a una vista de error, si aplica 
                }
                int idCarrito = (int)Session["CarritoId"];

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spInsertarDetalleCarrito @IDCarrito, @IDProducto, @Cantidad",
                        new SqlParameter("@IDCarrito", idCarrito),
                        new SqlParameter("@IDProducto", idProducto),
                        new SqlParameter("@Cantidad", cantidad)
                    );
                }
                TempData["SuccessMessage"] = "Producto agregado al carrito correctamente.";
                return RedirectToAction("Catalogo", "Catalogo"); // Redirige a la página del carrito 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction("Catalogo", "Catalogo"); // Redirige a la página del carrito 
            }
        }

    }
}
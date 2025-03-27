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

    }


}
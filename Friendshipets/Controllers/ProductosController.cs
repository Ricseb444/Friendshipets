using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using Friendshipets.Models.ViewModels;
using Friendshipets.Models;
using System.IO;

namespace Friendshipets.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos
        public ActionResult Index()
        {
            List<cListaProductos> listaProductos;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                listaProductos = db.Database.SqlQuery<cListaProductos>("spLeerProductos").ToList();
            }
            return View(listaProductos);
        }

        // GET: Productos/Details/5
        public ActionResult Details(int id)
        {
            cListaProductos producto = new cListaProductos();

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                producto = db.Database.SqlQuery<cListaProductos>(
                    "EXEC spObtenerProductoPorId @IDProducto",
                    new SqlParameter("@IDProducto", id)
                ).FirstOrDefault();
            }
            return View(producto);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        
      
        [HttpPost]
        public ActionResult Create(createProducto producto, HttpPostedFileBase ImagenProducto)
        {
            try
            {
                if (ImagenProducto != null && ImagenProducto.ContentLength > 0)
                {
                    // Validar la extensión del archivo
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(ImagenProducto.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ViewBag.ValorMensaje = 0;
                        ViewBag.MensajeProceso = "Solo se permiten archivos de imagen (JPG, PNG, GIF).";
                        return View(producto);
                    }

                    // Guardar el archivo en la carpeta "Images"
                    var fileName = Path.GetFileName(ImagenProducto.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    ImagenProducto.SaveAs(path);

                    // Asignar la ruta de la imagen al modelo
                    producto.ImgProducto = "/Content/Images/" + fileName;
                }
                else
                {
                    ViewBag.ValorMensaje = 0;
                    ViewBag.MensajeProceso = "Debe cargar una imagen válida.";
                    return View(producto);
                }

                // Guardar los datos del producto en la base de datos
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spCrearProducto @Precio, @NombreProducto, @Categoria, @NombreProveedor, @Stock, @ImgProducto",
                        new SqlParameter("@Precio", producto.Precio),
                        new SqlParameter("@NombreProducto", producto.NombreProducto.ToUpper()),
                        new SqlParameter("@Categoria", producto.Categoria.ToUpper()),
                        new SqlParameter("@NombreProveedor", producto.NombreProveedor),
                        new SqlParameter("@Stock", producto.Stock),
                        new SqlParameter("@ImgProducto", producto.ImgProducto ?? string.Empty)
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Producto agregado correctamente";
                }

                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al agregar el Producto: " + ex.Message;
                return View(producto);
            }
        }


        // GET: Productos/Edit/5
        public ActionResult Edit(int id)
        {
            editProducto producto = new editProducto();

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                producto = db.Database.SqlQuery<editProducto>(
                    "EXEC spObtenerProductoPorId @IDProducto",
                    new SqlParameter("@IDProducto", id)
                ).FirstOrDefault();
            }
            return View(producto);
        }

        // POST: Productos/Edit/5
        [HttpPost]
        public ActionResult Edit(editProducto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(producto);
                }
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spActualizarProducto @IDProducto, @Precio, @NombreProducto, @TipoProducto, @NombreProveedor, @Stock",
                        new SqlParameter("@IDProducto", producto.IDProducto),
                        new SqlParameter("@Precio", producto.Precio),
                        new SqlParameter("@NombreProducto", producto.NombreProducto.ToUpper()),
                        new SqlParameter("@TipoProducto", producto.Categoria.ToUpper()),
                        new SqlParameter("@NombreProveedor", producto.NombreProveedor),
                        new SqlParameter("@Stock", producto.Stock)
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Producto actualizado correctamente";
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al actualizar el Producto " + ex.Message;

                return View(producto);
            }
        }

        
        // POST: Productos/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spEliminarProducto @IDProducto",
                         new SqlParameter("@IDProducto", id)
                    );
                }

                return RedirectToAction("Index", "Productos");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar el Producto: " + ex.Message;
                return RedirectToAction("Index", "Productos");
            }
        }
    }
}

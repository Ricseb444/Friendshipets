using Friendshipets.Models;
using Friendshipets.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Friendshipets.Controllers
{
    public class ProveedoresController : Controller
    {
        // GET: Proveedores
        public ActionResult Index()
        {
            List<cListaProveedores> listaProveedores;
            using (FriendshipetEntities db = new FriendshipetEntities())
            { 
                listaProveedores = db.Database.SqlQuery<cListaProveedores>("spLeerProveedores").ToList();
            }
            return View(listaProveedores);
        }

        // GET: Proveedores/Details/5
        public ActionResult Details(int id)
        {
            cListaProveedores proveedor = new cListaProveedores();
            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                proveedor = db.Database.SqlQuery<cListaProveedores>(
                    "EXEC spObtenerProveedorPorId @IDProveedor",
                    new SqlParameter("@IDProveedor", id)
                ).FirstOrDefault();
            }
            return View(proveedor);
        }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        public ActionResult Create(createProveedor proveedor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(proveedor);
                }
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spCrearProveedor @NombreProveedor, @Telefono, @Email, @Direccion",
                        new SqlParameter("@NombreProveedor", proveedor.NombreProveedor.ToUpper()),
                        new SqlParameter("@Telefono", proveedor.Telefono),
                        new SqlParameter("@Email", proveedor.Email),
                        new SqlParameter("@Direccion", proveedor.Direccion)
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Proveedor agregado correctamente";
                }
                return View(proveedor);
            }
            catch(Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al agregar el Proveedor " + ex.Message;
                return View(proveedor);
            }
        }

        // GET: Proveedores/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            editProveedor proveedor = new editProveedor();

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                proveedor = db.Database.SqlQuery<editProveedor>(
                    "EXEC spObtenerProveedorPorId @IDProveedor",
                    new SqlParameter("@IDProveedor", id)
                ).FirstOrDefault();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        public ActionResult Edit(editProveedor proveedor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(proveedor);
                }
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spActualizarProveedor @IDProveedor, @NombreProveedor, @Telefono, @Email, @Direccion",
                        new SqlParameter("@IDProveedor", proveedor.IDProveedor),
                        new SqlParameter("@NombreProveedor", proveedor.NombreProveedor.ToUpper()),
                        new SqlParameter("@Telefono", proveedor.Telefono),
                        new SqlParameter("@Email", proveedor.Email),
                        new SqlParameter("@Direccion", proveedor.Direccion)
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Proveedor actualizado correctamente";
                }
                return View(proveedor);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al actualizar el Proveedor " + ex.Message;
                return View(proveedor);
            }
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spEliminarProveedor @IDProveedor",
                         new SqlParameter("@IDProveedor", id)
                    );
                }

                return RedirectToAction("Index", "Proveedores");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar el proveedor: " + ex.Message;
                return RedirectToAction("Index", "Proveedores");
            }
        }
    }
}

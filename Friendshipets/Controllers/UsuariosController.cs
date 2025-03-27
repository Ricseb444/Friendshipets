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
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            List<cListaUsuarios> listaUsuarios;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                listaUsuarios = db.Database.SqlQuery<cListaUsuarios>("spLeerUsuarios").ToList();
            }
            return View(listaUsuarios);
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int id)
        {
            cListaUsuarios User = new cListaUsuarios();

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                User = db.Database.SqlQuery<cListaUsuarios>(
                    "EXEC spObtenerClienteYUsuario @IDUsuario",
                    new SqlParameter("@IDUsuario", id)
                ).FirstOrDefault();
            }
            return View(User);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        public ActionResult Create(createUsuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(usuario);
                }

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spCrearUsuario @DNI_Cliente, @NombreUsuario, @CorreoUsuario, @Contrasena, @RolUsuario",
                        new SqlParameter("@DNI_Cliente", usuario.DNI_Cliente),
                        new SqlParameter("@NombreUsuario", usuario.NombreUsuario.ToUpper()),
                        new SqlParameter("@CorreoUsuario", usuario.CorreoUsuario),
                        new SqlParameter("@Contrasena", usuario.Contrasena),
                        new SqlParameter("@RolUsuario", usuario.RolUsuario.ToUpper())
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Usuario agregado correctamente";
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al agregar el Usuario " + ex.Message;

                return View(usuario);
            }
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int id)
        {
            editUsuario User = new editUsuario();

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                User = db.Database.SqlQuery<editUsuario>(
                    "EXEC spObtenerUsuarioPorId @IDUsuario",
                    new SqlParameter("@IDUsuario", id)
                ).FirstOrDefault();
            }
            return View(User);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        public ActionResult Edit(editUsuario usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(usuario);
                }
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spActualizarUsuario @IDUsuario, @NombreUsuario, @CorreoUsuario, @Contrasena, @RolUsuario",
                        new SqlParameter("@IDUsuario", usuario.IDUsuario),
                        new SqlParameter("@NombreUsuario", usuario.NombreUsuario.ToUpper()),
                        new SqlParameter("@CorreoUsuario", usuario.CorreoUsuario),
                        new SqlParameter("@Contrasena", usuario.Contrasena),
                        new SqlParameter("@RolUsuario", usuario.RolUsuario.ToUpper())
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Usuario actualizado correctamente";
                }
                return View(usuario);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al actualizar el usuario " + ex.Message;

                return View(usuario);
            }
        }

        // POST: Usuarios/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spEliminarUsuario @IDUsuario",
                         new SqlParameter("@IDUsuario", id)
                    );
                }

                return RedirectToAction("Index", "Usuarios");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar el Usuario: " + ex.Message;
                return RedirectToAction("Index", "Usuarios");
            }
        }
    }
}

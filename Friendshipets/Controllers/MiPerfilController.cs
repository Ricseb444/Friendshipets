using System;
using System.Linq;
using System.Web.Mvc;
using Friendshipets.Models;
using Friendshipets.Models.ViewModels;

namespace Friendshipets.Controllers
{
    public class MiPerfilController : Controller
    {
        // GET: MiPerfil
        public ActionResult Index()
        {
            int idUsuario = Convert.ToInt32(Session["IDUsuario"]);

            using (var db = new FriendshipetEntities())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.IDUsuario == idUsuario);

                if (usuario != null)
                {
                    var cliente = db.Clientes.FirstOrDefault(c => c.IDCliente == usuario.IDCliente);

                    var model = new MiPerfilViewModel
                    {
                        IDCliente = cliente.IDCliente,
                        DNI_Cliente = cliente.DNI_Cliente,
                        CorreoUsuario = usuario.CorreoUsuario,
                        NombreUsuario = usuario.NombreUsuario,
                        NombreCliente = cliente.NombreCliente,
                        ApellidosCliente = cliente.ApellidosCliente,
                        Direccion = cliente.Direccion
                    };

                    return View("MiPerfil", model);
                }

                return RedirectToAction("Login", "Auth");
            }
        }

        // GET: MiPerfil/EditarPerfil
        public ActionResult EditarPerfil()
        {
            int idUsuario = Convert.ToInt32(Session["IDUsuario"]);

            using (var db = new FriendshipetEntities())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.IDUsuario == idUsuario);

                if (usuario != null)
                {
                    var cliente = db.Clientes.FirstOrDefault(c => c.IDCliente == usuario.IDCliente);

                    var model = new MiPerfilViewModel
                    {
                        IDCliente = cliente.IDCliente,
                        DNI_Cliente = cliente.DNI_Cliente,
                        CorreoUsuario = usuario.CorreoUsuario,
                        NombreUsuario = usuario.NombreUsuario,
                        NombreCliente = cliente.NombreCliente,
                        ApellidosCliente = cliente.ApellidosCliente,
                        Direccion = cliente.Direccion
                    };

                    return View(model);
                }

                return RedirectToAction("Login", "Auth");
            }
        }

        [HttpPost]
        public ActionResult EditarPerfil(MiPerfilViewModel model)
        {
            if (ModelState.IsValid)
            {
                int idUsuario = Convert.ToInt32(Session["IDUsuario"]);

                using (var db = new FriendshipetEntities())
                {
                    var usuario = db.Usuarios.FirstOrDefault(u => u.IDUsuario == idUsuario);

                    if (usuario != null)
                    {
                        var cliente = db.Clientes.FirstOrDefault(c => c.IDCliente == usuario.IDCliente);

                        // Actualizar datos del usuario
                        usuario.NombreUsuario = model.NombreUsuario;

                        // Actualizar datos del cliente
                        cliente.NombreCliente = model.NombreCliente;
                        cliente.ApellidosCliente = model.ApellidosCliente;
                        cliente.Direccion = model.Direccion;

                        db.SaveChanges();

                        TempData["Mensaje"] = "Perfil actualizado correctamente.";
                        return RedirectToAction("Index");
                    }
                }
            }

            ViewBag.Mensaje = "Ocurrió un error al actualizar tu perfil.";
            return View(model);
        }

        // GET: MiPerfil/CambiarContrasena
        public ActionResult CambiarContrasena()
        {
            return View();
        }

        // POST: MiPerfil/CambiarContrasena
        [HttpPost]
        public ActionResult CambiarContrasena(CambiarContrasenaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            int idUsuario = Convert.ToInt32(Session["IDUsuario"]);

            using (var db = new FriendshipetEntities())
            {
                var usuario = db.Usuarios.FirstOrDefault(u => u.IDUsuario == idUsuario);

                if (usuario == null)
                {
                    ModelState.AddModelError("", "Usuario no encontrado.");
                    return View(model);
                }

                if (usuario.Contrasena != model.ContrasenaActual)
                {
                    ModelState.AddModelError("ContrasenaActual", "La contraseña actual es incorrecta.");
                    return View(model);
                }

                if (model.NuevaContrasena == model.ContrasenaActual)
                {
                    ModelState.AddModelError("NuevaContrasena", "La nueva contraseña no puede ser igual a la actual.");
                    return View(model);
                }

                usuario.Contrasena = model.NuevaContrasena;
                db.SaveChanges();

                TempData["Mensaje"] = "Contraseña actualizada exitosamente.";
                return RedirectToAction("Index");
            }
        }
    }
}

using Friendshipets.Models;
using Friendshipets.Models.ViewModels;
using System.Linq;
using System.Web.Mvc;

public class AuthController : Controller
{
    // GET: Login
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    // POST: Login
    [HttpPost]
    public ActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                var correo = model.CorreoUsuario?.Trim().ToLower();
                var contrasena = model.Contrasena?.Trim();

                var usuario = db.Usuarios.FirstOrDefault(u =>
                    u.CorreoUsuario.Trim().ToLower() == correo &&
                    u.Contrasena.Trim() == contrasena &&
                    u.EstadoUsuario == "A");

                if (usuario != null)
                {
                    Session["IDUsuario"] = usuario.IDUsuario;
                    Session["NombreUsuario"] = usuario.NombreUsuario;
                    Session["RolUsuario"] = usuario.RolUsuario;

                    var rol = usuario.RolUsuario.Trim().ToUpper();

                    if (rol == "ADMINISTRADOR")
                        return RedirectToAction("Index", "Home"); // Panel admin

                    if (rol == "CLIENTE")
                        return RedirectToAction("Index", "Tienda"); // Vista cliente

                    // 🚨 Seguridad: rol inválido
                    Session.Clear();
                    ViewBag.Mensaje = "Rol de usuario no válido. Contacte al administrador.";
                    return View(model);
                }
            }

            ViewBag.Mensaje = "Credenciales incorrectas o cuenta inactiva.";
        }

        return View(model);
    }

    // POST: Logout
    [HttpPost]
    public ActionResult Logout()
    {
        Session.Clear();
        return RedirectToAction("Index", "Tienda");
    }

    // GET: Registro
    [HttpGet]
    public ActionResult Registro()
    {
        return View();
    }

    // POST: Registro
    [HttpPost]
    public ActionResult Registro(RegistroViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spCrearClienteUsuario @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7",
                        model.DNI_Cliente,
                        model.NombreCliente,
                        model.ApellidosCliente,
                        model.Direccion,
                        model.NombreUsuario,
                        model.CorreoUsuario,
                        model.Contrasena,
                        "CLIENTE"
                    );

                    TempData["RegistroExitoso"] = "¡Registro exitoso! Ahora puedes iniciar sesión.";
                    return RedirectToAction("Login");
                }
            }
            catch
            {
                ViewBag.Mensaje = "Ocurrió un error al registrar. Verifica que los datos sean únicos e intenta nuevamente.";
            }
        }

        return View(model);
    }
}
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
    public class ClientesController : Controller
    {
        // GET: Clientes
        public ActionResult Index()
        {
            List<cListaClientes> listaClientes;

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                listaClientes = db.Database.SqlQuery<cListaClientes>("spLeerClientes").ToList();
            }
            return View(listaClientes);
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int id)
        {
            cListaClientes cliente = new cListaClientes();

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                cliente = db.Database.SqlQuery<cListaClientes>(
                    "EXEC spObtenerClientePorId @IDCliente",
                    new SqlParameter("@IDCliente", id)
                ).FirstOrDefault();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        public ActionResult Create(createCliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(cliente);
                }

                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spCrearCliente @DNI_Cliente, @NombreCliente, @ApellidosCliente, @Direccion",
                        new SqlParameter("@DNI_Cliente", cliente.DNI_Cliente),
                        new SqlParameter("@NombreCliente", cliente.NombreCliente.ToUpper()),
                        new SqlParameter("@ApellidosCliente", cliente.ApellidosCliente.ToUpper()),
                        new SqlParameter("@Direccion", cliente.Direccion.ToUpper())
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Cliente agregado correctamente";
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al agregar el cliente " + ex.Message;

                return View(cliente);

            }
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int id)
        {
            editCliente Client = new editCliente();

            using (FriendshipetEntities db = new FriendshipetEntities())
            {
                Client = db.Database.SqlQuery<editCliente>(
                    "EXEC spObtenerClientePorId @IDCliente",
                    new SqlParameter("@IDCliente", id)
                ).FirstOrDefault();
            }
            return View(Client);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        public ActionResult Edit(editCliente cliente)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(cliente);
                }
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spActualizarCliente @IDCliente, @DNI_Cliente, @NombreCliente, @ApellidosCliente, @Direccion",
                        new SqlParameter("@IDCliente", cliente.IDCliente),
                        new SqlParameter("@DNI_Cliente", cliente.DNI_Cliente),
                        new SqlParameter("@NombreCliente", cliente.NombreCliente.ToUpper()),
                        new SqlParameter("@ApellidosCliente", cliente.ApellidosCliente.ToUpper()),
                        new SqlParameter("@Direccion", cliente.Direccion.ToUpper())
                    );

                    ViewBag.ValorMensaje = 1;
                    ViewBag.MensajeProceso = "Cliente actualizado correctamente";
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                ViewBag.ValorMensaje = 0;
                ViewBag.MensajeProceso = "Falló al actualizar el cliente " + ex.Message;

                return View(cliente);
            }
        }

        // POST: Clientes/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                using (FriendshipetEntities db = new FriendshipetEntities())
                {
                    db.Database.ExecuteSqlCommand(
                        "EXEC spEliminarCliente @IDCliente",
                         new SqlParameter("@IDCliente", id)
                    );
                }

                return RedirectToAction("Index", "Clientes");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error al eliminar el cliente: " + ex.Message;
                return RedirectToAction("Index", "Clientes");
            }
        }
    }
}

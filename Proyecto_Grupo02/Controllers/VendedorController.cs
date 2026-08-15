using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System.Data.Entity;
using System.Threading.Tasks;


namespace Proyecto_Grupo02.Controllers
{
    public class VendedorController : Controller
    {
        private readonly CatalogoDbContext _context =
            new CatalogoDbContext();

        private readonly KA_FASHION_BDEntities _usuariosContext =
            new KA_FASHION_BDEntities();


        private bool EsVendedor()
        {
            if (Session["ConsecutivoUsuario"] == null)
            {
                return false;
            }

            int idUsuario =
                Convert.ToInt32(
                    Session["ConsecutivoUsuario"]
                );

            var usuario =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == idUsuario);

            if (usuario == null)
            {
                return false;
            }

            var rol =
                _usuariosContext.tbRol
                .FirstOrDefault(r =>
                    r.Consecutivo ==
                    usuario.ConsecutivoRol);

            return rol != null &&
                   rol.Nombre == "Vendedor";
        }

        public async Task<ActionResult> Principal()
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idPendiente =
                await _context.Estados
                .Where(e =>
                    e.NombreEstado == "Pendiente")
                .Select(e => e.IdEstado)
                .FirstOrDefaultAsync();


            var modelo =
                new VendedorPrincipalViewModel
                {
                    SolicitudesPendientes =
                        await _context.SolicitudesShopper
                        .CountAsync(p =>
                            p.IdEstado == idPendiente),

                    ContactosPendientes =
                        await _context.Contactos
                        .CountAsync(c =>
                            c.IdEstado == idPendiente),

                    TotalClientes =
                        _usuariosContext.tbUsuario
                        .Count(u =>
                            u.ConsecutivoRol ==
                            _usuariosContext.tbRol
                            .Where(r =>
                                r.Nombre == "Cliente")
                            .Select(r =>
                                r.Consecutivo)
                            .FirstOrDefault())
                };


            return View(modelo);
        }

        public async Task<ActionResult> PersonalShopper()
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var solicitudes =
                await _context.SolicitudesShopper
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();


            var estados =
                await _context.Estados
                .ToListAsync();


            var usuarios =
                _usuariosContext.tbUsuario
                .ToList();


            var resultado =
                solicitudes.Select(p =>
                {
                    var usuario =
                        usuarios.FirstOrDefault(u =>
                            u.Consecutivo ==
                            p.IdUsuario);

                    var estado =
                        estados.FirstOrDefault(e =>
                            e.IdEstado ==
                            p.IdEstado);


                    return new VendedorPersonalShopperViewModel
                    {
                        IdSolicitud =
                            p.IdSolicitud,

                        Cliente =
                            usuario != null
                            ? usuario.Nombre + " " +
                              usuario.Apellido1
                            : "Cliente",

                        Correo =
                            usuario != null
                            ? usuario.CorreoElectronico
                            : "",

                        EstiloBuscado =
                            p.EstiloBuscado,

                        Talla =
                            p.Talla,

                        Presupuesto =
                            p.Presupuesto,

                        Necesidades =
                            p.Necesidades,

                        Fecha =
                            p.Fecha,

                        Estado =
                            estado != null
                            ? estado.NombreEstado
                            : ""
                    };

                }).ToList();


            return View(resultado);
        }


        [HttpPost]
        public async Task<ActionResult> AtenderPersonalShopper(
            int id)
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var solicitud =
                await _context.SolicitudesShopper
                .FirstOrDefaultAsync(p =>
                    p.IdSolicitud == id);


            if (solicitud != null)
            {
                int idAtendido =
                    await _context.Estados
                    .Where(e =>
                        e.NombreEstado ==
                        "Atendido")
                    .Select(e =>
                        e.IdEstado)
                    .FirstOrDefaultAsync();


                solicitud.IdEstado =
                    idAtendido;


                await _context
                    .SaveChangesAsync();


                TempData["MensajeExito"] =
                    "La solicitud fue marcada como atendida.";
            }


            return RedirectToAction(
                "PersonalShopper"
            );
        }

        public async Task<ActionResult> Contactos()
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var contactos =
                await _context.Contactos
                .OrderByDescending(c =>
                    c.Fecha)
                .ToListAsync();


            var estados =
                await _context.Estados
                .ToListAsync();


            var resultado =
                contactos.Select(c =>
                {
                    var estado =
                        estados.FirstOrDefault(e =>
                            e.IdEstado ==
                            c.IdEstado);


                    return new VendedorContactoViewModel
                    {
                        IdContacto =
                            c.IdContacto,

                        Nombre =
                            c.Nombre,

                        Correo =
                            c.Correo,

                        Asunto =
                            c.Asunto,

                        Mensaje =
                            c.Mensaje,

                        Fecha =
                            c.Fecha,

                        Estado =
                            estado != null
                            ? estado.NombreEstado
                            : ""
                    };

                }).ToList();


            return View(resultado);
        }


        [HttpPost]
        public async Task<ActionResult> AtenderContacto(
            int id)
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var contacto =
                await _context.Contactos
                .FirstOrDefaultAsync(c =>
                    c.IdContacto == id);


            if (contacto != null)
            {
                int idAtendido =
                    await _context.Estados
                    .Where(e =>
                        e.NombreEstado ==
                        "Atendido")
                    .Select(e =>
                        e.IdEstado)
                    .FirstOrDefaultAsync();


                contacto.IdEstado =
                    idAtendido;


                await _context
                    .SaveChangesAsync();


                TempData["MensajeExito"] =
                    "El contacto fue marcado como atendido.";
            }


            return RedirectToAction(
                "Contactos"
            );
        }


        public ActionResult Inventario()
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            var inventario =
                (from i in _context.Inventarios
                 join e in _context.Estados
                     on i.IdEstado equals e.IdEstado

                 select new AdministradorInventarioViewModel
                 {
                     IdInventario =
                         i.IdInventario,

                     Producto =
                         i.Producto.Nombre,

                     Sucursal =
                         i.Sucursal.Nombre,

                     Talla =
                         i.Talla,

                     Color =
                         i.Color,

                     Existencias =
                         i.Existencias,

                     FechaActualizacion =
                         i.FechaActualizacion,

                     Estado =
                         e.NombreEstado
                 })
                .OrderBy(i => i.Producto)
                .ThenBy(i => i.Sucursal)
                .ToList();


            return View(inventario);
        }


        public ActionResult Clientes()
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idCliente =
                _usuariosContext.tbRol
                .Where(r =>
                    r.Nombre == "Cliente")
                .Select(r =>
                    r.Consecutivo)
                .FirstOrDefault();


            var clientes =
                _usuariosContext.tbUsuario
                .Where(u =>
                    u.ConsecutivoRol ==
                    idCliente)
                .OrderBy(u =>
                    u.Nombre)
                .Select(u =>
                    new VendedorClienteViewModel
                    {
                        IdUsuario =
                            u.Consecutivo,

                        NombreCompleto =
                            u.Nombre + " " +
                            u.Apellido1 + " " +
                            u.Apellido2,

                        Correo =
                            u.CorreoElectronico,

                        Telefono =
                            u.Telefono,

                        Estado =
                            u.Estado
                    })
                .ToList();


            return View(clientes);
        }


        [HttpGet]
        public ActionResult Perfil()
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idUsuario =
                Convert.ToInt32(
                    Session["ConsecutivoUsuario"]
                );

            var usuario =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == idUsuario);

            if (usuario == null)
            {
                ViewBag.Mensaje =
                    "No se pudo cargar la información del usuario";

                return View(new UsuarioModel());
            }


            return View(new UsuarioModel
            {
                Consecutivo =
                    usuario.Consecutivo,

                Identificacion =
                    usuario.Identificacion,

                Nombre =
                    usuario.Nombre,

                Apellido1 =
                    usuario.Apellido1,

                Apellido2 =
                    usuario.Apellido2,

                CorreoElectronico =
                    usuario.CorreoElectronico,

                Telefono =
                    usuario.Telefono
            });
        }


        [HttpPost]
        public ActionResult Perfil(UsuarioModel modelo)
        {
            if (!EsVendedor())
            {
                return RedirectToAction(
                    "Index",
                    "Home"
                );
            }


            int idUsuario =
                Convert.ToInt32(
                    Session["ConsecutivoUsuario"]
                );

            var usuario =
                _usuariosContext.tbUsuario
                .FirstOrDefault(u =>
                    u.Consecutivo == idUsuario);

            if (usuario == null)
            {
                ViewBag.Mensaje =
                    "No se pudo validar la información";

                return View(modelo);
            }


            if (usuario.CorreoElectronico != modelo.CorreoElectronico)
            {
                var correoEnUso =
                    _usuariosContext.tbUsuario
                    .Any(u =>
                        u.CorreoElectronico == modelo.CorreoElectronico &&
                        u.Consecutivo != idUsuario);

                if (correoEnUso)
                {
                    ViewBag.Mensaje =
                        "Ese correo ya está en uso por otra cuenta";

                    return View(modelo);
                }
            }


            usuario.Nombre =
                modelo.Nombre;

            usuario.Apellido1 =
                modelo.Apellido1;

            usuario.Apellido2 =
                modelo.Apellido2;

            usuario.CorreoElectronico =
                modelo.CorreoElectronico;

            usuario.Telefono =
                modelo.Telefono;

            if (!string.IsNullOrWhiteSpace(modelo.Contrasenna))
            {
                usuario.Contrasenna =
                    modelo.Contrasenna;

                usuario.TieneContrasennaTemp = false;
                usuario.VigenciaContrasennaTemp = null;
            }


            var response =
                _usuariosContext.SaveChanges();

            if (response <= 0)
            {
                ViewBag.Mensaje =
                    "No se pudo actualizar la información";

                return View(modelo);
            }


            Session["NombreUsuario"] = usuario.Nombre;

            ViewBag.MensajeExito =
                "Tu información se actualizó correctamente";

            modelo.Contrasenna = null;
            modelo.ConfirmarContrasenna = null;

            return View(modelo);
        }
    }
}
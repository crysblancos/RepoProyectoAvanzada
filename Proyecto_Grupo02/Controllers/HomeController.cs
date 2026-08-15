using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using Proyecto_Grupo02.Services;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Controllers
{
    public class HomeController : Controller
    {
        readonly UtilitarioService utilitario = new UtilitarioService();

        #region Inicio de sesión

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShowErrors()
        {
            try
            {
                using (var context = new KA_FASHION_BDEntities())
                {
                    var errors = context.tbError.OrderByDescending(e => e.FechaHora).Take(20)
                        .Select(e => new { e.FechaHora, e.Lugar, e.Mensaje, e.ConsecutivoUsuario }).ToList();

                    return Json(errors, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                utilitario.RegistrarErrorBitacora(ex.GetBaseException().Message, MethodBase.GetCurrentMethod().Name);
                return Content("Error reading tbError: " + ex.GetBaseException().Message);
            }
        }

        [HttpGet]
        public ActionResult ShowUsers()
        {
            try
            {
                using (var context = new KA_FASHION_BDEntities())
                {
                    var users = context.tbUsuario.OrderByDescending(u => u.Consecutivo).Take(20)
                        .Select(u => new { u.Consecutivo, u.Identificacion, u.CorreoElectronico, u.Nombre, u.Estado }).ToList();
                    return Json(users, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                utilitario.RegistrarErrorBitacora(ex.GetBaseException().Message, MethodBase.GetCurrentMethod().Name);
                return Content("Error reading tbUsuario: " + ex.GetBaseException().Message);
            }
        }

        [HttpPost]
        public ActionResult Index(UsuarioModel model)
        {
            try
            {
                using (var context = new KA_FASHION_BDEntities())
                {
                    
                    var usuario = (from U in context.tbUsuario
                                   where U.CorreoElectronico == model.CorreoElectronico
                                   && U.Contrasenna == model.Contrasenna
                                   && U.Estado == true
                                   select U).FirstOrDefault();

                    if (usuario == null)
                    {
                        ViewBag.Mensaje = "Correo electrónico o contraseña incorrectos";
                        return View();
                    }

                    if (usuario.TieneContrasennaTemp && usuario.VigenciaContrasennaTemp < DateTime.Now)
                    {
                        ViewBag.Mensaje = "Su contraseña temporal ya venció, solicite una nueva";
                        return View();
                    }

                    
                    Session["ConsecutivoUsuario"] = usuario.Consecutivo;
                    Session["NombreUsuario"] = usuario.Nombre;
                    Session["NombreRol"] = usuario.tbRol.Nombre;
                    Session["ConsecutivoRol"] = usuario.ConsecutivoRol;

                    if (usuario.TieneContrasennaTemp)
                    {
                        return RedirectToAction("Perfil", "Usuario");
                    }

                    
                    if (usuario.tbRol != null)
                    {
                        if (usuario.tbRol.Nombre == "Vendedor")
                        {
                            return RedirectToAction("Principal", "Vendedor");
                        }

                        if (usuario.tbRol.Nombre == "Administrador")
                        {
                            return RedirectToAction("Principal", "Administrador");
                        }
                    }

                    return RedirectToAction("Principal", "Home");
                }
            }
            catch (Exception ex)
            {
                utilitario.RegistrarErrorBitacora(ex.Message, MethodBase.GetCurrentMethod().Name);
                ViewBag.Mensaje = "Ocurrió un error al iniciar sesión";
                return View();
            }
        }

        #endregion

        
        [HttpGet]
        public ActionResult TestDb()
        {
            try
            {
                using (var context = new KA_FASHION_BDEntities())
                {
                    var cs = context.Database.Connection.ConnectionString;
                    var exists = context.Database.Exists();
                    return Content($"DB reachable: {exists}\nConnectionString: {cs}");
                }
            }
            catch (Exception ex)
            {
                utilitario.RegistrarErrorBitacora(ex.GetBaseException().Message, MethodBase.GetCurrentMethod().Name);
                return Content("DB error: " + ex.GetBaseException().ToString());
            }
        }

        #region Registro de usuarios

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(UsuarioModel model)
        {
            try
            {
                using (var context = new KA_FASHION_BDEntities())
                {
                    var existe = (from U in context.tbUsuario
                                  where U.Identificacion == model.Identificacion
                                  || U.CorreoElectronico == model.CorreoElectronico
                                  select U).FirstOrDefault();

                    if (existe != null)
                    {
                        ViewBag.Mensaje = "Ya existe un usuario con esa identificación o correo";
                        return View();
                    }

                    context.tbUsuario.Add(new tbUsuario
                    {
                        Identificacion = model.Identificacion,
                        Nombre = model.Nombre,
                        Apellido1 = model.Apellido1,
                        Apellido2 = model.Apellido2,
                        CorreoElectronico = model.CorreoElectronico,
                        Telefono = model.Telefono,
                        Contrasenna = model.Contrasenna,
                        Estado = true,
                        TieneContrasennaTemp = false,
                        ConsecutivoRol = 1 
                    });

                    var response = context.SaveChanges();

                    if (response <= 0)
                    {
                        ViewBag.Mensaje = "No se pudo completar el registro";
                        return View();
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                utilitario.RegistrarErrorBitacora(ex.Message, MethodBase.GetCurrentMethod().Name);
                ViewBag.Mensaje = "Ocurrió un error al registrar el usuario";
                return View();
            }
        }

        #endregion

        #region Recuperar acceso

        [HttpGet]
        public ActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarAcceso(UsuarioModel model)
        {
            try
            {
                using (var context = new KA_FASHION_BDEntities())
                {
                    var usuario = (from U in context.tbUsuario
                                   where U.CorreoElectronico == model.CorreoElectronico
                                   && U.Estado == true
                                   select U).FirstOrDefault();

                    if (usuario == null)
                    {
                        ViewBag.Mensaje = "No existe una cuenta activa con ese correo";
                        return View();
                    }

                    var temporal = utilitario.GenerarContrasenna();
                    var vigenciaMinutos = int.Parse(ConfigurationManager.AppSettings["VigenciaMinutos"]);

                    usuario.Contrasenna = temporal;
                    usuario.TieneContrasennaTemp = true;
                    usuario.VigenciaContrasennaTemp = DateTime.Now.AddMinutes(vigenciaMinutos);

                    var response = context.SaveChanges();

                    if (response > 0)
                    {
                        var ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "RecuperacionContrasena.html");
                        var contenido = System.IO.File.ReadAllText(ruta);

                        contenido = contenido.Replace("{{NOMBRE}}", usuario.Nombre);
                        contenido = contenido.Replace("{{PASSWORD}}", temporal);
                        contenido = contenido.Replace("{{MINUTOS}}", vigenciaMinutos.ToString());

                        utilitario.EnviarCorreo(usuario.CorreoElectronico, "Recuperación de acceso - KA Fashion", contenido);

                        ViewBag.MensajeExito = "Te enviamos un correo con tu contraseña temporal";
                        return View();
                    }

                    ViewBag.Mensaje = "No se pudo recuperar el acceso";
                    return View();
                }
            }
            catch (Exception ex)
            {
                utilitario.RegistrarErrorBitacora(ex.Message, MethodBase.GetCurrentMethod().Name);
                ViewBag.Mensaje = "Ocurrió un error al recuperar el acceso";
                return View();
            }
        }

        #endregion

        [LogActionFilter]
        [HttpGet]
        public ActionResult Principal()
        {
            using (var context = new CatalogoDbContext())
            {
                var ahora = DateTime.Now;

                var promocionActiva = context.Promociones
                    .Where(p =>
                        p.IdEstado == EstadosConsts.Activo &&
                        p.FechaInicio <= ahora &&
                        p.FechaFin >= ahora)
                    .OrderByDescending(p => p.Descuento)
                    .FirstOrDefault();

                ViewBag.PromocionActiva = promocionActiva;
            }

            return View();
        }

        [LogActionFilter]
        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}
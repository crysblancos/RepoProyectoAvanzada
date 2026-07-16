using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using Proyecto_Grupo02.Services;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Controllers
{
    [LogActionFilter]
    public class UsuarioController : Controller
    {
        readonly UtilitarioService utilitario = new UtilitarioService();

        [HttpGet]
        public ActionResult Perfil()
        {
            try
            {
                var consecutivo = int.Parse(Session["ConsecutivoUsuario"].ToString());

                using (var context = new KA_FASHION_BDEntities())
                {
                    var usuario = (from U in context.tbUsuario
                                   where U.Consecutivo == consecutivo
                                   select U).FirstOrDefault();

                    if (usuario == null)
                    {
                        ViewBag.Mensaje = "No se pudo cargar la información del usuario";
                        return View(new UsuarioModel());
                    }

                    return View(new UsuarioModel
                    {
                        Consecutivo = usuario.Consecutivo,
                        Identificacion = usuario.Identificacion,
                        Nombre = usuario.Nombre,
                        Apellido1 = usuario.Apellido1,
                        Apellido2 = usuario.Apellido2,
                        CorreoElectronico = usuario.CorreoElectronico,
                        Telefono = usuario.Telefono
                    });
                }
            }
            catch (Exception ex)
            {
                utilitario.RegistrarErrorBitacora(ex.Message, MethodBase.GetCurrentMethod().Name);
                return View(new UsuarioModel());
            }
        }

        [HttpPost]
        public ActionResult Perfil(UsuarioModel model)
        {
            try
            {
                var consecutivo = int.Parse(Session["ConsecutivoUsuario"].ToString());

                using (var context = new KA_FASHION_BDEntities())
                {
                    var usuario = (from U in context.tbUsuario
                                   where U.Consecutivo == consecutivo
                                   select U).FirstOrDefault();

                    if (usuario == null)
                    {
                        ViewBag.Mensaje = "No se pudo validar la información";
                        return View(model);
                    }

                    // Si el correo cambió, validar que no esté en uso por otro usuario
                    if (usuario.CorreoElectronico != model.CorreoElectronico)
                    {
                        var correoEnUso = (from U in context.tbUsuario
                                           where U.CorreoElectronico == model.CorreoElectronico
                                           && U.Consecutivo != consecutivo
                                           select U).Any();

                        if (correoEnUso)
                        {
                            ViewBag.Mensaje = "Ese correo ya está en uso por otra cuenta";
                            return View(model);
                        }
                    }

                    // Actualizar datos del perfil
                    usuario.Nombre = model.Nombre;
                    usuario.Apellido1 = model.Apellido1;
                    usuario.Apellido2 = model.Apellido2;
                    usuario.CorreoElectronico = model.CorreoElectronico;
                    usuario.Telefono = model.Telefono;

                    // Solo cambiar la contraseña si el usuario escribió una nueva
                    if (!string.IsNullOrWhiteSpace(model.Contrasenna))
                    {
                        usuario.Contrasenna = model.Contrasenna;
                        usuario.TieneContrasennaTemp = false;
                        usuario.VigenciaContrasennaTemp = null;
                    }

                    context.Entry(usuario).State = EntityState.Modified;
                    var response = context.SaveChanges();

                    if (response <= 0)
                    {
                        ViewBag.Mensaje = "No se pudo actualizar la información";
                        return View(model);
                    }

                    // Refrescar el nombre en sesión, por si lo cambió
                    Session["NombreUsuario"] = usuario.Nombre;

                    ViewBag.MensajeExito = "Tu información se actualizó correctamente";

                    // Volvemos a cargar el modelo limpio (sin contraseñas) para la vista
                    model.Contrasenna = null;
                    model.ConfirmarContrasenna = null;
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                utilitario.RegistrarErrorBitacora(ex.Message, MethodBase.GetCurrentMethod().Name);
                ViewBag.Mensaje = "Ocurrió un error al actualizar la información";
                return View(model);
            }
        }
    }
}
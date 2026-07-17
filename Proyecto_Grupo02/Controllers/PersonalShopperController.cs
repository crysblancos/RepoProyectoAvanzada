using Proyecto_Grupo02.Models;
using Proyecto_Grupo02.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Controllers
{
    public class PersonalShopperController : Controller
    {
        private readonly IPersonalShopperService _personalShopperService = new PersonalShopperService();

        [HttpGet]
        public ActionResult PersonalShopper() => View(new PersonalShopperModel());

        [HttpPost]
        public async Task<ActionResult> PersonalShopper(PersonalShopperModel model)
        {
            if (Session["ConsecutivoUsuario"] == null) return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid) return View(model);

            var idUsuario = Convert.ToInt32(Session["ConsecutivoUsuario"]);
            var exito = await _personalShopperService.RegistrarSolicitudAsync(idUsuario, model);
            ViewBag.MensajeExito = exito ? "Tu solicitud fue enviada correctamente" : null;
            ViewBag.Mensaje = exito ? null : "No se pudo enviar la solicitud";
            return View(exito ? new PersonalShopperModel() : model);
        }
    }
}
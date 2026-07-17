using Proyecto_Grupo02.Models;
using Proyecto_Grupo02.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Controllers
{
    public class ContactoController : Controller
    {
        private readonly IContactoService _contactoService = new ContactoService();

        [HttpGet]
        public ActionResult Contacto() => View(new ContactoModel());

        [HttpPost]
        public async Task<ActionResult> Contacto(ContactoModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var exito = await _contactoService.RegistrarAsync(model);
            ViewBag.MensajeExito = exito ? "Tu mensaje fue enviado correctamente" : null;
            ViewBag.Mensaje = exito ? null : "No se pudo enviar el mensaje";
            return View(exito ? new ContactoModel() : model);
        }
    }
}
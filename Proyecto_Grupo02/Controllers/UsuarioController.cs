using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Perfil()
        {
            return View();
        }
    }
}
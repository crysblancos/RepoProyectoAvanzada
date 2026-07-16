using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Proyecto_Grupo02.Models
{
    public class LogActionFilter : ActionFilterAttribute
    {
        // Se ejecuta antes de cada acción marcada con [LogActionFilter]
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["ConsecutivoUsuario"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Index" }
                    });

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
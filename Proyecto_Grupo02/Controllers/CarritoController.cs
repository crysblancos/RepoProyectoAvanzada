using Proyecto_Grupo02.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ICarritoService _carritoService = new CarritoService();

        private int ObtenerIdUsuario() => Convert.ToInt32(Session["ConsecutivoUsuario"]);

        public async Task<ActionResult> Carrito()
        {
            if (Session["ConsecutivoUsuario"] == null) return RedirectToAction("Index", "Home");
            var carrito = await _carritoService.ObtenerCarritoAsync(ObtenerIdUsuario());
            return View(carrito);
        }

        [HttpPost]
        public async Task<ActionResult> Agregar(int idProducto, int cantidad, string talla, string color)
        {
            if (Session["ConsecutivoUsuario"] == null) return RedirectToAction("Index", "Home");
            await _carritoService.AgregarProductoAsync(ObtenerIdUsuario(), idProducto, cantidad, talla, color);
            return RedirectToAction("Carrito");
        }

        [HttpPost]
        public async Task<ActionResult> ActualizarCantidad(int idDetalleCarrito, int cantidad)
        {
            await _carritoService.ActualizarCantidadAsync(idDetalleCarrito, cantidad);
            return RedirectToAction("Carrito");
        }

        [HttpPost]
        public async Task<ActionResult> Eliminar(int idDetalleCarrito)
        {
            await _carritoService.EliminarDetalleAsync(idDetalleCarrito);
            return RedirectToAction("Carrito");
        }
    }
}
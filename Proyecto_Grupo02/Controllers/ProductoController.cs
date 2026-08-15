using Proyecto_Grupo02.Services;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Proyecto_Grupo02.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService =
            new ProductoService();

        public async Task<ActionResult> Producto(int? idCategoria)
        {
            var categorias =
                await _productoService.ObtenerCategoriasAsync();

            ViewBag.Categorias = categorias;
            ViewBag.IdCategoria = idCategoria;

            var catalogo =
                await _productoService.ObtenerCatalogoAsync(idCategoria);

            return View(catalogo);
        }

        public async Task<ActionResult> DetalleProducto(int? id)
        {
            if (!id.HasValue)
            {
                TempData["MensajeError"] =
                    "Debe seleccionar un producto para ver su detalle.";

                return RedirectToAction("Producto");
            }


            var detalle =
                await _productoService.ObtenerDetalleAsync(id.Value);

            if (detalle == null)
            {
                return RedirectToAction("Producto");
            }

            return View(detalle);
        }

        [HttpPost]
        public async Task<ActionResult> AgregarResena(int idProducto, int calificacion, string comentario)
        {
            if (Session["ConsecutivoUsuario"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int idUsuario =
                Convert.ToInt32(Session["ConsecutivoUsuario"]);

            if (calificacion >= 1 && calificacion <= 5 && !string.IsNullOrWhiteSpace(comentario))
            {
                await _productoService.AgregarResenaAsync(
                    idUsuario,
                    idProducto,
                    calificacion,
                    comentario
                );

                TempData["MensajeExito"] =
                    "Gracias por tu reseña.";
            }
            else
            {
                TempData["MensajeError"] =
                    "Debe seleccionar una calificación e ingresar un comentario.";
            }

            return RedirectToAction("DetalleProducto", new { id = idProducto });
        }
    }
}

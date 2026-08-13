using Proyecto_Grupo02.Services;
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

        public async Task<ActionResult> DetalleProducto(int id)
        {
            var detalle =
                await _productoService.ObtenerDetalleAsync(id);

            if (detalle == null)
            {
                return RedirectToAction("Producto");
            }

            return View(detalle);
        }
    }
}
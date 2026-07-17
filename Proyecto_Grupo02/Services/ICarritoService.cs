using Proyecto_Grupo02.Models;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Services
{
    public interface ICarritoService
    {
        Task<CarritoViewModel> ObtenerCarritoAsync(int idUsuario);
        Task AgregarProductoAsync(int idUsuario, int idProducto, int cantidad, string talla, string color);
        Task ActualizarCantidadAsync(int idDetalleCarrito, int cantidad);
        Task EliminarDetalleAsync(int idDetalleCarrito);
    }
}
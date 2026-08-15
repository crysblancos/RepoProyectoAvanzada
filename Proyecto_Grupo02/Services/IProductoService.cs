using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Services
{
    public interface IProductoService
    {
        Task<List<ProductoListItemViewModel>> ObtenerCatalogoAsync(int? idCategoria = null);

        Task<List<tbCategoria>> ObtenerCategoriasAsync();

        Task<ProductoDetalleViewModel> ObtenerDetalleAsync(int idProducto);

        Task AgregarResenaAsync(int idUsuario, int idProducto, int calificacion, string comentario);
    }
}

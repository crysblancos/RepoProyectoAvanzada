using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Services
{
    public class ProductoService : IProductoService
    {
        private readonly CatalogoDbContext _context;

        public ProductoService()
        {
            _context = new CatalogoDbContext();
        }

        public async Task<List<ProductoListItemViewModel>> ObtenerCatalogoAsync(int? idCategoria = null)
        {
            var consulta = _context.Productos
                .Where(p => p.IdEstado == EstadosConsts.Activo);

            if (idCategoria.HasValue)
            {
                consulta = consulta.Where(p => p.IdCategoria == idCategoria.Value);
            }

            return await consulta
                .OrderByDescending(p => p.Novedad)
                .ThenBy(p => p.Nombre)
                .Select(p => new ProductoListItemViewModel
                {
                    IdProducto = p.IdProducto,
                    Nombre = p.Nombre,
                    Precio = p.Precio,
                    Imagen = p.Imagen,
                    Destacado = p.Destacado,
                    Novedad = p.Novedad
                })
                .ToListAsync();
        }

        public async Task<List<tbCategoria>> ObtenerCategoriasAsync()
        {
            return await _context.Categorias
                .Where(c => c.IdEstado == EstadosConsts.Activo)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<ProductoDetalleViewModel> ObtenerDetalleAsync(int idProducto)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.IdProducto == idProducto &&
                            p.IdEstado == EstadosConsts.Activo)
                .FirstOrDefaultAsync();

            if (producto == null)
            {
                return null;
            }

            var existencias = await _context.Inventarios
                .Where(i => i.IdProducto == idProducto &&
                            i.IdEstado == EstadosConsts.Activo)
                .SumAsync(i => (int?)i.Existencias) ?? 0;

            return new ProductoDetalleViewModel
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Imagen = producto.Imagen,
                Talla = producto.Talla,
                Color = producto.Color,
                Categoria = producto.Categoria?.Nombre,
                Existencias = existencias
            };
        }
    }
}
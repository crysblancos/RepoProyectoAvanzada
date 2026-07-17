using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Grupo02.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly CatalogoDbContext _context;

        public CarritoService()
        {
            _context = new CatalogoDbContext();
        }

        private async Task<tbCarrito> ObtenerOCrearCarritoAsync(int idUsuario)
        {
            var carrito = await _context.Carritos
                .Where(c => c.IdUsuario == idUsuario && c.IdEstado == EstadosConsts.Activo)
                .FirstOrDefaultAsync();

            if (carrito != null) return carrito;

            carrito = new tbCarrito
            {
                IdUsuario = idUsuario,
                FechaCreacion = DateTime.Now,
                IdEstado = EstadosConsts.Activo
            };

            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();
            return carrito;
        }

        public async Task<CarritoViewModel> ObtenerCarritoAsync(int idUsuario)
        {
            var carrito = await ObtenerOCrearCarritoAsync(idUsuario);

            var items = await _context.CarritoDetalles
                .Include(d => d.Producto)
                .Where(d => d.IdCarrito == carrito.IdCarrito)
                .Select(d => new CarritoItemViewModel
                {
                    IdDetalleCarrito = d.IdDetalleCarrito,
                    IdProducto = d.IdProducto,
                    Nombre = d.Producto.Nombre,
                    Imagen = d.Producto.Imagen,
                    Precio = d.Producto.Precio,
                    Talla = d.Talla,
                    Color = d.Color,
                    Cantidad = d.Cantidad
                })
                .ToListAsync();

            return new CarritoViewModel { Items = items };
        }

        public async Task AgregarProductoAsync(int idUsuario, int idProducto, int cantidad, string talla, string color)
        {
            var carrito = await ObtenerOCrearCarritoAsync(idUsuario);

            var existente = await _context.CarritoDetalles
                .Where(d => d.IdCarrito == carrito.IdCarrito && d.IdProducto == idProducto && d.Talla == talla && d.Color == color)
                .FirstOrDefaultAsync();

            if (existente != null)
            {
                existente.Cantidad += cantidad;
            }
            else
            {
                _context.CarritoDetalles.Add(new tbCarritoDetalle
                {
                    IdCarrito = carrito.IdCarrito,
                    IdProducto = idProducto,
                    Cantidad = cantidad,
                    Talla = talla,
                    Color = color
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task ActualizarCantidadAsync(int idDetalleCarrito, int cantidad)
        {
            var detalle = await _context.CarritoDetalles
                .Where(d => d.IdDetalleCarrito == idDetalleCarrito)
                .FirstOrDefaultAsync();

            if (detalle == null) return;

            if (cantidad <= 0)
            {
                _context.CarritoDetalles.Remove(detalle);
            }
            else
            {
                detalle.Cantidad = cantidad;
            }

            await _context.SaveChangesAsync();
        }

        public async Task EliminarDetalleAsync(int idDetalleCarrito)
        {
            var detalle = await _context.CarritoDetalles
                .Where(d => d.IdDetalleCarrito == idDetalleCarrito)
                .FirstOrDefaultAsync();

            if (detalle == null) return;

            _context.CarritoDetalles.Remove(detalle);
            await _context.SaveChangesAsync();
        }
    }
}
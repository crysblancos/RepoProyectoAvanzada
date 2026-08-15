using Proyecto_Grupo02.Data;
using Proyecto_Grupo02.EF;
using Proyecto_Grupo02.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Proyecto_Grupo02.Controllers
{
    public class PedidoController : Controller
    {
        private readonly CatalogoDbContext _context =
            new CatalogoDbContext();

        public async Task<ActionResult> Pedido()
        {
            if (Session["ConsecutivoUsuario"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var sucursales = await _context.Sucursales
                .Where(s => s.IdEstado == EstadosConsts.Activo)
                .OrderBy(s => s.Nombre)
                .ToListAsync();

            ViewBag.Sucursales = new SelectList(
                sucursales,
                "IdSucursal",
                "Nombre"
            );

            var modelo = new PedidoModel
            {
                MetodoEntrega = "Retiro en sucursal"
            };

            return View(modelo);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirmar(PedidoModel model)
        {
            if (Session["ConsecutivoUsuario"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int idUsuario =
                Convert.ToInt32(Session["ConsecutivoUsuario"]);

            if (!ModelState.IsValid)
            {
                var sucursales = await _context.Sucursales
                    .Where(s => s.IdEstado == EstadosConsts.Activo)
                    .OrderBy(s => s.Nombre)
                    .ToListAsync();

                ViewBag.Sucursales = new SelectList(
                    sucursales,
                    "IdSucursal",
                    "Nombre",
                    model.IdSucursal
                );

                return View("Pedido", model);
            }


            using (var transaccion =
                _context.Database.BeginTransaction())
            {
                try
                {

                    var carrito = await _context.Carritos
                        .Where(c =>
                            c.IdUsuario == idUsuario &&
                            c.IdEstado == EstadosConsts.Activo)
                        .FirstOrDefaultAsync();


                    if (carrito == null)
                    {
                        TempData["MensajeError"] =
                            "No se encontró un carrito activo.";

                        return RedirectToAction(
                            "Carrito",
                            "Carrito"
                        );
                    }


                    var detallesCarrito =
                        await _context.CarritoDetalles
                        .Where(d =>
                            d.IdCarrito == carrito.IdCarrito)
                        .ToListAsync();


                    if (detallesCarrito.Count == 0)
                    {
                        TempData["MensajeError"] =
                            "El carrito está vacío.";

                        return RedirectToAction(
                            "Carrito",
                            "Carrito"
                        );
                    }


                    decimal subtotalPedido = 0;


                    foreach (var detalle in detallesCarrito)
                    {
                        var producto =
                            await _context.Productos
                            .Where(p =>
                                p.IdProducto ==
                                detalle.IdProducto)
                            .FirstOrDefaultAsync();

                        if (producto == null)
                        {
                            throw new Exception(
                                "Uno de los productos ya no está disponible."
                            );
                        }

                        subtotalPedido +=
                            producto.Precio *
                            detalle.Cantidad;
                    }

                    var promocionActiva =
                        await _context.Promociones
                        .Where(p =>
                            p.IdEstado == EstadosConsts.Activo &&
                            p.FechaInicio <= DateTime.Now &&
                            p.FechaFin >= DateTime.Now)
                        .OrderByDescending(p =>
                            p.Descuento)
                        .FirstOrDefaultAsync();

                    decimal descuentoPedido =
                        promocionActiva != null
                            ? subtotalPedido *
                              promocionActiva.Descuento / 100
                            : 0;

                    decimal costoEntrega = 2500;

                    decimal totalPedido =
                        subtotalPedido -
                        descuentoPedido +
                        costoEntrega;


                    var pedido = new tbPedido
                    {
                        MetodoEntrega =
                            model.MetodoEntrega,

                        Observaciones =
                            model.Observaciones,

                        Total =
                            totalPedido,

                        FechaPedido =
                            DateTime.Now,

                        IdUsuario =
                            idUsuario,

                        IdSucursal =
                            model.IdSucursal.Value,

                        IdEstado =
                            EstadosConsts.Activo
                    };


                    _context.Pedidos.Add(pedido);

                    await _context.SaveChangesAsync();


                    foreach (var detalle in detallesCarrito)
                    {
                        var producto =
                            await _context.Productos
                            .Where(p =>
                                p.IdProducto ==
                                detalle.IdProducto)
                            .FirstOrDefaultAsync();


                        var inventario =
                            await _context.Inventarios
                            .Where(i =>
                                i.IdProducto ==
                                    detalle.IdProducto &&

                                i.IdSucursal ==
                                    model.IdSucursal.Value &&

                                i.IdEstado ==
                                    EstadosConsts.Activo)
                            .FirstOrDefaultAsync();


                        if (inventario == null)
                        {
                            throw new Exception(
                                "No se encontró inventario para el producto " +
                                producto.Nombre +
                                " en la sucursal seleccionada."
                            );
                        }


                        if (inventario.Existencias <
                            detalle.Cantidad)
                        {
                            throw new Exception(
                                "No hay suficientes existencias de " +
                                producto.Nombre +
                                " en la sucursal seleccionada."
                            );
                        }


                        decimal subtotal =
                            producto.Precio *
                            detalle.Cantidad;


                        var detallePedido =
                            new tbPedidoDetalle
                            {
                                Cantidad =
                                    detalle.Cantidad,

                                Talla =
                                    detalle.Talla,

                                Color =
                                    detalle.Color,

                                PrecioUnitario =
                                    producto.Precio,

                                Subtotal =
                                    subtotal,

                                IdPedido =
                                    pedido.IdPedido,

                                IdProducto =
                                    detalle.IdProducto
                            };


                        _context.PedidoDetalles
                            .Add(detallePedido);


                        inventario.Existencias -=
                            detalle.Cantidad;

                        inventario.FechaActualizacion =
                            DateTime.Now;
                    }


                    var estadoInactivo =
                        await _context.Estados
                        .Where(e =>
                            e.NombreEstado == "Inactivo")
                        .FirstOrDefaultAsync();


                    if (estadoInactivo == null)
                    {
                        throw new Exception(
                            "No se encontró el estado Inactivo."
                        );
                    }


                    carrito.IdEstado =
                        estadoInactivo.IdEstado;


                    await _context.SaveChangesAsync();


                    transaccion.Commit();


                    return RedirectToAction(
                        "Confirmacion",
                        new
                        {
                            id = pedido.IdPedido
                        }
                    );
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();

                    TempData["MensajeError"] =
                        ex.Message;

                    return RedirectToAction("Pedido");
                }
            }
        }

        public async Task<ActionResult> Confirmacion(int id)
        {
            if (Session["ConsecutivoUsuario"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            int idUsuario =
                Convert.ToInt32(
                    Session["ConsecutivoUsuario"]
                );


            var pedido =
                await _context.Pedidos
                .Where(p =>
                    p.IdPedido == id &&
                    p.IdUsuario == idUsuario)
                .FirstOrDefaultAsync();


            if (pedido == null)
            {
                return RedirectToAction(
                    "Principal",
                    "Home"
                );
            }


            var sucursal =
                await _context.Sucursales
                .Where(s =>
                    s.IdSucursal ==
                    pedido.IdSucursal)
                .FirstOrDefaultAsync();


            var modelo =
                new PedidoConfirmacionViewModel
                {
                    IdPedido =
                        pedido.IdPedido,

                    FechaPedido =
                        pedido.FechaPedido,

                    MetodoEntrega =
                        pedido.MetodoEntrega,

                    Sucursal =
                        sucursal != null
                            ? sucursal.Nombre
                            : "",

                    Observaciones =
                        pedido.Observaciones,

                    Total =
                        pedido.Total
                };


            return View(modelo);
        }


        public async Task<ActionResult> MisPedidos()
        {
            if (Session["ConsecutivoUsuario"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            int idUsuario =
                Convert.ToInt32(
                    Session["ConsecutivoUsuario"]
                );


            var pedidos =
                await _context.Pedidos
                .Where(p =>
                    p.IdUsuario == idUsuario)
                .OrderByDescending(p =>
                    p.FechaPedido)
                .Select(p =>
                    new PedidoHistorialViewModel
                    {
                        IdPedido =
                            p.IdPedido,

                        FechaPedido =
                            p.FechaPedido,

                        MetodoEntrega =
                            p.MetodoEntrega,

                        Sucursal =
                            p.Sucursal.Nombre,

                        Total =
                            p.Total,

                        Estado =
                            p.Estado.NombreEstado
                    }
                )
                .ToListAsync();


            return View(pedidos);
        }


        public async Task<ActionResult> DetallePedido(int id)
        {
            if (Session["ConsecutivoUsuario"] == null)
            {
                return RedirectToAction("Index", "Home");
            }


            int idUsuario =
                Convert.ToInt32(
                    Session["ConsecutivoUsuario"]
                );


            var pedido =
                await _context.Pedidos
                .Where(p =>
                    p.IdPedido == id &&
                    p.IdUsuario == idUsuario)
                .FirstOrDefaultAsync();


            if (pedido == null)
            {
                return RedirectToAction("MisPedidos");
            }


            var sucursal =
                await _context.Sucursales
                .Where(s =>
                    s.IdSucursal ==
                    pedido.IdSucursal)
                .FirstOrDefaultAsync();


            var estado =
                await _context.Estados
                .Where(e =>
                    e.IdEstado ==
                    pedido.IdEstado)
                .FirstOrDefaultAsync();


            var detalles =
                await _context.PedidoDetalles
                .Include(d => d.Producto)
                .Where(d =>
                    d.IdPedido == id)
                .Select(d =>
                    new PedidoHistorialDetalleItemViewModel
                    {
                        Producto =
                            d.Producto.Nombre,

                        Imagen =
                            d.Producto.Imagen,

                        Talla =
                            d.Talla,

                        Color =
                            d.Color,

                        Cantidad =
                            d.Cantidad,

                        PrecioUnitario =
                            d.PrecioUnitario,

                        Subtotal =
                            d.Subtotal
                    }
                )
                .ToListAsync();


            var modeloDetalle =
                new PedidoHistorialDetalleViewModel
                {
                    IdPedido =
                        pedido.IdPedido,

                    FechaPedido =
                        pedido.FechaPedido,

                    MetodoEntrega =
                        pedido.MetodoEntrega,

                    Sucursal =
                        sucursal != null
                            ? sucursal.Nombre
                            : "",

                    Observaciones =
                        pedido.Observaciones,

                    Estado =
                        estado != null
                            ? estado.NombreEstado
                            : "",

                    Total =
                        pedido.Total,

                    Detalles =
                        detalles
                };


            return View(modeloDetalle);
        }
    }
}
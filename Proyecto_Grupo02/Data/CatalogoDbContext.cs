using Proyecto_Grupo02.EF;
using System.Data.Entity;

namespace Proyecto_Grupo02.Data
{
    public class CatalogoDbContext : DbContext
    {
        public CatalogoDbContext() : base("name=KA_FASHION_BD_SQL")
        {
            Database.SetInitializer<CatalogoDbContext>(null);
        }

        public DbSet<tbEstado> Estados { get; set; }
        public DbSet<tbCategoria> Categorias { get; set; }
        public DbSet<tbProducto> Productos { get; set; }
        public DbSet<tbSucursal> Sucursales { get; set; }
        public DbSet<tbInventario> Inventarios { get; set; }
        public DbSet<tbCarrito> Carritos { get; set; }
        public DbSet<tbCarritoDetalle> CarritoDetalles { get; set; }
        public DbSet<tbPedido> Pedidos { get; set; }
        public DbSet<tbPedidoDetalle> PedidoDetalles { get; set; }
        public DbSet<tbPromocion> Promociones { get; set; }
        public DbSet<tbPromoProducto> PromoProductos { get; set; }
        public DbSet<tbResena> Resenas { get; set; }
        public DbSet<tbContacto> Contactos { get; set; }
        public DbSet<tbSolicitudShopper> SolicitudesShopper { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbEstado>().ToTable("FIDE_ESTADOS_TB");
            modelBuilder.Entity<tbEstado>().HasKey(e => e.IdEstado);
            modelBuilder.Entity<tbEstado>().Property(e => e.IdEstado).HasColumnName("ID_ESTADO");
            modelBuilder.Entity<tbEstado>().Property(e => e.NombreEstado).HasColumnName("NOMBRE_ESTADO");

            modelBuilder.Entity<tbCategoria>().ToTable("FIDE_CATEGORIAS_TB");
            modelBuilder.Entity<tbCategoria>().HasKey(c => c.IdCategoria);
            modelBuilder.Entity<tbCategoria>().Property(c => c.IdCategoria).HasColumnName("ID_CATEGORIA");
            modelBuilder.Entity<tbCategoria>().Property(c => c.Nombre).HasColumnName("NOMBRE");
            modelBuilder.Entity<tbCategoria>().Property(c => c.Descripcion).HasColumnName("DESCRIPCION");
            modelBuilder.Entity<tbCategoria>().Property(c => c.IdEstado).HasColumnName("ID_ESTADO");
            modelBuilder.Entity<tbCategoria>().HasRequired(c => c.Estado).WithMany(e => e.Categorias).HasForeignKey(c => c.IdEstado);

            modelBuilder.Entity<tbProducto>().ToTable("FIDE_PRODUCTOS_TB");
            modelBuilder.Entity<tbProducto>().HasKey(p => p.IdProducto);
            modelBuilder.Entity<tbProducto>().Property(p => p.IdProducto).HasColumnName("ID_PRODUCTO");
            modelBuilder.Entity<tbProducto>().Property(p => p.Nombre).HasColumnName("NOMBRE");
            modelBuilder.Entity<tbProducto>().Property(p => p.Descripcion).HasColumnName("DESCRIPCION");
            modelBuilder.Entity<tbProducto>().Property(p => p.Precio).HasColumnName("PRECIO").HasPrecision(18, 2);
            modelBuilder.Entity<tbProducto>().Property(p => p.Imagen).HasColumnName("IMAGEN");
            modelBuilder.Entity<tbProducto>().Property(p => p.Talla).HasColumnName("TALLA");
            modelBuilder.Entity<tbProducto>().Property(p => p.Color).HasColumnName("COLOR");
            modelBuilder.Entity<tbProducto>().Property(p => p.Destacado).HasColumnName("DESTACADO");
            modelBuilder.Entity<tbProducto>().Property(p => p.Novedad).HasColumnName("NOVEDAD");
            modelBuilder.Entity<tbProducto>().Property(p => p.IdCategoria).HasColumnName("ID_CATEGORIA");
            modelBuilder.Entity<tbProducto>().Property(p => p.IdEstado).HasColumnName("ID_ESTADO");
            modelBuilder.Entity<tbProducto>().HasRequired(p => p.Categoria).WithMany(c => c.Productos).HasForeignKey(p => p.IdCategoria);
            modelBuilder.Entity<tbProducto>().HasRequired(p => p.Estado).WithMany(e => e.Productos).HasForeignKey(p => p.IdEstado);

            modelBuilder.Entity<tbSucursal>().ToTable("FIDE_SUCURSALES_TB");
            modelBuilder.Entity<tbSucursal>().HasKey(s => s.IdSucursal);
            modelBuilder.Entity<tbSucursal>().Property(s => s.IdSucursal).HasColumnName("ID_SUCURSAL");
            modelBuilder.Entity<tbSucursal>().Property(s => s.Nombre).HasColumnName("NOMBRE");
            modelBuilder.Entity<tbSucursal>().Property(s => s.Direccion).HasColumnName("DIRECCION");
            modelBuilder.Entity<tbSucursal>().Property(s => s.Telefono).HasColumnName("TELEFONO");
            modelBuilder.Entity<tbSucursal>().Property(s => s.Horario).HasColumnName("HORARIO");
            modelBuilder.Entity<tbSucursal>().Property(s => s.IdEstado).HasColumnName("ID_ESTADO");

            modelBuilder.Entity<tbInventario>().ToTable("FIDE_INVENTARIO_TB");
            modelBuilder.Entity<tbInventario>().HasKey(i => i.IdInventario);
            modelBuilder.Entity<tbInventario>().Property(i => i.IdInventario).HasColumnName("ID_INVENTARIO");
            modelBuilder.Entity<tbInventario>().Property(i => i.Talla).HasColumnName("TALLA");
            modelBuilder.Entity<tbInventario>().Property(i => i.Color).HasColumnName("COLOR");
            modelBuilder.Entity<tbInventario>().Property(i => i.Existencias).HasColumnName("EXISTENCIAS");
            modelBuilder.Entity<tbInventario>().Property(i => i.FechaActualizacion).HasColumnName("FECHA_ACTUALIZACION");
            modelBuilder.Entity<tbInventario>().Property(i => i.IdProducto).HasColumnName("ID_PRODUCTO");
            modelBuilder.Entity<tbInventario>().Property(i => i.IdSucursal).HasColumnName("ID_SUCURSAL");
            modelBuilder.Entity<tbInventario>().Property(i => i.IdEstado).HasColumnName("ID_ESTADO");
            modelBuilder.Entity<tbInventario>().HasRequired(i => i.Producto).WithMany(p => p.Inventarios).HasForeignKey(i => i.IdProducto);
            modelBuilder.Entity<tbInventario>().HasRequired(i => i.Sucursal).WithMany(s => s.Inventarios).HasForeignKey(i => i.IdSucursal);

            modelBuilder.Entity<tbCarrito>().ToTable("FIDE_CARRITOS_TB");
            modelBuilder.Entity<tbCarrito>().HasKey(c => c.IdCarrito);
            modelBuilder.Entity<tbCarrito>().Property(c => c.IdCarrito).HasColumnName("ID_CARRITO");
            modelBuilder.Entity<tbCarrito>().Property(c => c.FechaCreacion).HasColumnName("FECHA_CREACION");
            modelBuilder.Entity<tbCarrito>().Property(c => c.IdUsuario).HasColumnName("ID_USUARIO");
            modelBuilder.Entity<tbCarrito>().Property(c => c.IdEstado).HasColumnName("ID_ESTADO");

            modelBuilder.Entity<tbCarritoDetalle>().ToTable("FIDE_CARRITO_DETALLES_TB");
            modelBuilder.Entity<tbCarritoDetalle>().HasKey(d => d.IdDetalleCarrito);
            modelBuilder.Entity<tbCarritoDetalle>().Property(d => d.IdDetalleCarrito).HasColumnName("ID_DETALLE_CARRITO");
            modelBuilder.Entity<tbCarritoDetalle>().Property(d => d.Cantidad).HasColumnName("CANTIDAD");
            modelBuilder.Entity<tbCarritoDetalle>().Property(d => d.Talla).HasColumnName("TALLA");
            modelBuilder.Entity<tbCarritoDetalle>().Property(d => d.Color).HasColumnName("COLOR");
            modelBuilder.Entity<tbCarritoDetalle>().Property(d => d.IdCarrito).HasColumnName("ID_CARRITO");
            modelBuilder.Entity<tbCarritoDetalle>().Property(d => d.IdProducto).HasColumnName("ID_PRODUCTO");
            modelBuilder.Entity<tbCarritoDetalle>().HasRequired(d => d.Carrito).WithMany(c => c.Detalles).HasForeignKey(d => d.IdCarrito);
            modelBuilder.Entity<tbCarritoDetalle>().HasRequired(d => d.Producto).WithMany(p => p.CarritoDetalles).HasForeignKey(d => d.IdProducto);

            modelBuilder.Entity<tbPedido>().ToTable("FIDE_PEDIDOS_TB");
            modelBuilder.Entity<tbPedido>().HasKey(p => p.IdPedido);
            modelBuilder.Entity<tbPedido>().Property(p => p.IdPedido).HasColumnName("ID_PEDIDO");
            modelBuilder.Entity<tbPedido>().Property(p => p.MetodoEntrega).HasColumnName("METODO_ENTREGA");
            modelBuilder.Entity<tbPedido>().Property(p => p.Observaciones).HasColumnName("OBSERVACIONES");
            modelBuilder.Entity<tbPedido>().Property(p => p.Total).HasColumnName("TOTAL").HasPrecision(18, 2);
            modelBuilder.Entity<tbPedido>().Property(p => p.FechaPedido).HasColumnName("FECHA_PEDIDO");
            modelBuilder.Entity<tbPedido>().Property(p => p.IdUsuario).HasColumnName("ID_USUARIO");
            modelBuilder.Entity<tbPedido>().Property(p => p.IdSucursal).HasColumnName("ID_SUCURSAL");
            modelBuilder.Entity<tbPedido>().Property(p => p.IdEstado).HasColumnName("ID_ESTADO");
            modelBuilder.Entity<tbPedido>().HasRequired(p => p.Sucursal).WithMany(s => s.Pedidos).HasForeignKey(p => p.IdSucursal);

            modelBuilder.Entity<tbPedidoDetalle>().ToTable("FIDE_PEDIDOS_DETALLE_TB");
            modelBuilder.Entity<tbPedidoDetalle>().HasKey(d => d.IdDetalle);
            modelBuilder.Entity<tbPedidoDetalle>().Property(d => d.IdDetalle).HasColumnName("ID_DETALLE");
            modelBuilder.Entity<tbPedidoDetalle>().Property(d => d.Cantidad).HasColumnName("CANTIDAD");
            modelBuilder.Entity<tbPedidoDetalle>().Property(d => d.Talla).HasColumnName("TALLA");
            modelBuilder.Entity<tbPedidoDetalle>().Property(d => d.Color).HasColumnName("COLOR");
            modelBuilder.Entity<tbPedidoDetalle>().Property(d => d.PrecioUnitario).HasColumnName("PRECIO_UNITARIO").HasPrecision(18, 2);
            modelBuilder.Entity<tbPedidoDetalle>().Property(d => d.Subtotal).HasColumnName("SUBTOTAL").HasPrecision(18, 2);
            modelBuilder.Entity<tbPedidoDetalle>().Property(d => d.IdPedido).HasColumnName("ID_PEDIDO");
            modelBuilder.Entity<tbPedidoDetalle>().Property(d => d.IdProducto).HasColumnName("ID_PRODUCTO");
            modelBuilder.Entity<tbPedidoDetalle>().HasRequired(d => d.Pedido).WithMany(p => p.Detalles).HasForeignKey(d => d.IdPedido);
            modelBuilder.Entity<tbPedidoDetalle>().HasRequired(d => d.Producto).WithMany(p => p.PedidoDetalles).HasForeignKey(d => d.IdProducto);

            modelBuilder.Entity<tbPromocion>().ToTable("FIDE_PROMOCIONES_TB");
            modelBuilder.Entity<tbPromocion>().HasKey(p => p.IdPromocion);
            modelBuilder.Entity<tbPromocion>().Property(p => p.IdPromocion).HasColumnName("ID_PROMOCION");
            modelBuilder.Entity<tbPromocion>().Property(p => p.Nombre).HasColumnName("NOMBRE");
            modelBuilder.Entity<tbPromocion>().Property(p => p.Descripcion).HasColumnName("DESCRIPCION");
            modelBuilder.Entity<tbPromocion>().Property(p => p.Descuento).HasColumnName("DESCUENTO").HasPrecision(18, 2);
            modelBuilder.Entity<tbPromocion>().Property(p => p.FechaInicio).HasColumnName("FECHA_INICIO");
            modelBuilder.Entity<tbPromocion>().Property(p => p.FechaFin).HasColumnName("FECHA_FIN");
            modelBuilder.Entity<tbPromocion>().Property(p => p.IdEstado).HasColumnName("ID_ESTADO");

            modelBuilder.Entity<tbPromoProducto>().ToTable("FIDE_PROMO_PRODUCTOS_TB");
            modelBuilder.Entity<tbPromoProducto>().HasKey(pp => new { pp.IdProducto, pp.IdPromocion });
            modelBuilder.Entity<tbPromoProducto>().Property(pp => pp.IdProducto).HasColumnName("ID_PRODUCTO");
            modelBuilder.Entity<tbPromoProducto>().Property(pp => pp.IdPromocion).HasColumnName("ID_PROMOCION");
            modelBuilder.Entity<tbPromoProducto>().Property(pp => pp.IdEstado).HasColumnName("ID_ESTADO");
            modelBuilder.Entity<tbPromoProducto>().HasRequired(pp => pp.Producto).WithMany(p => p.PromoProductos).HasForeignKey(pp => pp.IdProducto);
            modelBuilder.Entity<tbPromoProducto>().HasRequired(pp => pp.Promocion).WithMany(p => p.PromoProductos).HasForeignKey(pp => pp.IdPromocion);

            modelBuilder.Entity<tbResena>().ToTable("FIDE_RESEÑAS_TB");
            modelBuilder.Entity<tbResena>().HasKey(r => r.IdResena);
            modelBuilder.Entity<tbResena>().Property(r => r.IdResena).HasColumnName("ID_RESEÑA");
            modelBuilder.Entity<tbResena>().Property(r => r.Calificacion).HasColumnName("CALIFICACION");
            modelBuilder.Entity<tbResena>().Property(r => r.Comentario).HasColumnName("COMENTARIO");
            modelBuilder.Entity<tbResena>().Property(r => r.Fecha).HasColumnName("FECHA");
            modelBuilder.Entity<tbResena>().Property(r => r.IdUsuario).HasColumnName("ID_USUARIO");
            modelBuilder.Entity<tbResena>().Property(r => r.IdProducto).HasColumnName("ID_PRODUCTO");
            modelBuilder.Entity<tbResena>().Property(r => r.IdEstado).HasColumnName("ID_ESTADO");
            modelBuilder.Entity<tbResena>().HasRequired(r => r.Producto).WithMany(p => p.Resenas).HasForeignKey(r => r.IdProducto);

            modelBuilder.Entity<tbContacto>().ToTable("FIDE_CONTACTOS_TB");
            modelBuilder.Entity<tbContacto>().HasKey(c => c.IdContacto);
            modelBuilder.Entity<tbContacto>().Property(c => c.IdContacto).HasColumnName("ID_CONTACTO");
            modelBuilder.Entity<tbContacto>().Property(c => c.Nombre).HasColumnName("NOMBRE");
            modelBuilder.Entity<tbContacto>().Property(c => c.Correo).HasColumnName("CORREO");
            modelBuilder.Entity<tbContacto>().Property(c => c.Fecha).HasColumnName("FECHA");
            modelBuilder.Entity<tbContacto>().Property(c => c.Asunto).HasColumnName("ASUNTO");
            modelBuilder.Entity<tbContacto>().Property(c => c.Mensaje).HasColumnName("MENSAJE");
            modelBuilder.Entity<tbContacto>().Property(c => c.IdEstado).HasColumnName("ID_ESTADO");

            modelBuilder.Entity<tbSolicitudShopper>().ToTable("FIDE_PERSONAL_SHOPPER_TB");
            modelBuilder.Entity<tbSolicitudShopper>().HasKey(s => s.IdSolicitud);
            modelBuilder.Entity<tbSolicitudShopper>().Property(s => s.IdSolicitud).HasColumnName("ID_SOLICITUD");
            modelBuilder.Entity<tbSolicitudShopper>().Property(s => s.EstiloBuscado).HasColumnName("ESTILO_BUSCADO");
            modelBuilder.Entity<tbSolicitudShopper>().Property(s => s.Talla).HasColumnName("TALLA");
            modelBuilder.Entity<tbSolicitudShopper>().Property(s => s.Presupuesto).HasColumnName("PRESUPUESTO").HasPrecision(18, 2);
            modelBuilder.Entity<tbSolicitudShopper>().Property(s => s.Necesidades).HasColumnName("NECESIDADES");
            modelBuilder.Entity<tbSolicitudShopper>().Property(s => s.Fecha).HasColumnName("FECHA");
            modelBuilder.Entity<tbSolicitudShopper>().Property(s => s.IdUsuario).HasColumnName("ID_USUARIO");
            modelBuilder.Entity<tbSolicitudShopper>().Property(s => s.IdEstado).HasColumnName("ID_ESTADO");
        }
    }
}
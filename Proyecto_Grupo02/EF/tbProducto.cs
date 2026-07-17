using System.Collections.Generic;

namespace Proyecto_Grupo02.EF
{
    public class tbProducto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public bool Destacado { get; set; }
        public bool Novedad { get; set; }
        public int IdCategoria { get; set; }
        public int IdEstado { get; set; }
        public virtual tbCategoria Categoria { get; set; }
        public virtual tbEstado Estado { get; set; }
        public virtual ICollection<tbInventario> Inventarios { get; set; }
        public virtual ICollection<tbCarritoDetalle> CarritoDetalles { get; set; }
        public virtual ICollection<tbPedidoDetalle> PedidoDetalles { get; set; }
        public virtual ICollection<tbResena> Resenas { get; set; }
        public virtual ICollection<tbPromoProducto> PromoProductos { get; set; }
    }
}
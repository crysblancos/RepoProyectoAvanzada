using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Grupo02.Models
{
    public class CarritoItemViewModel
    {
        public int IdDetalleCarrito { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public decimal Precio { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Precio * Cantidad;
    }

    public class CarritoViewModel
    {
        public List<CarritoItemViewModel> Items { get; set; } = new List<CarritoItemViewModel>();
        public decimal CostoEntrega { get; set; } = 2500;
        public decimal Subtotal => Items.Sum(i => i.Subtotal);
        public decimal Total => Items.Count > 0 ? Subtotal + CostoEntrega : 0;
    }
}
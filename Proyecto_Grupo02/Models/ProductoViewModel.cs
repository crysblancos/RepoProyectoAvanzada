using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Grupo02.Models
{
    public class ProductoListItemViewModel
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public bool Destacado { get; set; }
        public bool Novedad { get; set; }
    }

    public class ResenaItemViewModel
    {
        public string NombreUsuario { get; set; }
        public int Calificacion { get; set; }
        public string Comentario { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class ProductoDetalleViewModel
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string Imagen { get; set; }
        public string Talla { get; set; }
        public string Color { get; set; }
        public string Categoria { get; set; }
        public int Existencias { get; set; }
        public bool Disponible => Existencias > 0;

        public List<ResenaItemViewModel> Resenas { get; set; } = new List<ResenaItemViewModel>();
        public double PromedioCalificacion => Resenas.Count > 0 ? Resenas.Average(r => r.Calificacion) : 0;
    }
}

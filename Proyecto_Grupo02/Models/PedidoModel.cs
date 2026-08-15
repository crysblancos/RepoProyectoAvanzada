using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Grupo02.Models
{
    public class PedidoModel
    {
        [Required(ErrorMessage = "Seleccione una sucursal.")]
        public int? IdSucursal { get; set; }

        [Required(ErrorMessage = "Seleccione el método de entrega.")]
        public string MetodoEntrega { get; set; }

        public string Observaciones { get; set; }

        public decimal Subtotal { get; set; }

        public decimal CostoEntrega { get; set; }

        public decimal Total { get; set; }
    }


    public class PedidoHistorialViewModel
    {
        public int IdPedido { get; set; }

        public DateTime FechaPedido { get; set; }

        public string MetodoEntrega { get; set; }

        public string Sucursal { get; set; }

        public decimal Total { get; set; }

        public string Estado { get; set; }
    }


    public class PedidoHistorialDetalleItemViewModel
    {
        public string Producto { get; set; }

        public string Imagen { get; set; }

        public string Talla { get; set; }

        public string Color { get; set; }

        public int Cantidad { get; set; }

        public decimal PrecioUnitario { get; set; }

        public decimal Subtotal { get; set; }
    }


    public class PedidoHistorialDetalleViewModel
    {
        public int IdPedido { get; set; }

        public DateTime FechaPedido { get; set; }

        public string MetodoEntrega { get; set; }

        public string Sucursal { get; set; }

        public string Observaciones { get; set; }

        public string Estado { get; set; }

        public decimal Total { get; set; }

        public List<PedidoHistorialDetalleItemViewModel> Detalles { get; set; } =
            new List<PedidoHistorialDetalleItemViewModel>();
    }
}
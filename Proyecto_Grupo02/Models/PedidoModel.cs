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
}
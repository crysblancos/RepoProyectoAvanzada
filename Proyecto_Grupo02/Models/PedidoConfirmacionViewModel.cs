using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proyecto_Grupo02.Models
{
    public class PedidoConfirmacionViewModel
    {
        public int IdPedido { get; set; }

        public DateTime FechaPedido { get; set; }

        public string MetodoEntrega { get; set; }

        public string Sucursal { get; set; }

        public string Observaciones { get; set; }

        public decimal Total { get; set; }
    }
}
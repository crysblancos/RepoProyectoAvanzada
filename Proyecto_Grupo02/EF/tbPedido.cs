using System;
using System.Collections.Generic;

namespace Proyecto_Grupo02.EF
{
    public class tbPedido
    {
        public int IdPedido { get; set; }
        public string MetodoEntrega { get; set; }
        public string Observaciones { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaPedido { get; set; }
        public int IdUsuario { get; set; }
        public int IdSucursal { get; set; }
        public int IdEstado { get; set; }
        public virtual tbSucursal Sucursal { get; set; }
        public virtual tbEstado Estado { get; set; }
        public virtual ICollection<tbPedidoDetalle> Detalles { get; set; }
    }
}
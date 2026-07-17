using System.Collections.Generic;

namespace Proyecto_Grupo02.EF
{
    public class tbSucursal
    {
        public int IdSucursal { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Horario { get; set; }
        public int IdEstado { get; set; }
        public virtual tbEstado Estado { get; set; }
        public virtual ICollection<tbInventario> Inventarios { get; set; }
        public virtual ICollection<tbPedido> Pedidos { get; set; }
    }
}
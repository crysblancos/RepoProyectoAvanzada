using System;

namespace Proyecto_Grupo02.EF
{
    public class tbSolicitudShopper
    {
        public int IdSolicitud { get; set; }
        public string EstiloBuscado { get; set; }
        public string Talla { get; set; }
        public decimal Presupuesto { get; set; }
        public string Necesidades { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuario { get; set; }
        public int IdEstado { get; set; }
        public virtual tbEstado Estado { get; set; }
    }
}
using System;

namespace Proyecto_Grupo02.EF
{
    public class tbContacto
    {
        public int IdContacto { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public DateTime Fecha { get; set; }
        public string Asunto { get; set; }
        public string Mensaje { get; set; }
        public int IdEstado { get; set; }
        public virtual tbEstado Estado { get; set; }
    }
}
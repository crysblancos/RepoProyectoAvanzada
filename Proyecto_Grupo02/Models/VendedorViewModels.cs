using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proyecto_Grupo02.Models
{
    public class VendedorPrincipalViewModel
    {
        public int SolicitudesPendientes { get; set; }

        public int ContactosPendientes { get; set; }

        public int TotalClientes { get; set; }
    }


    public class VendedorPersonalShopperViewModel
    {
        public int IdSolicitud { get; set; }

        public string Cliente { get; set; }

        public string Correo { get; set; }

        public string EstiloBuscado { get; set; }

        public string Talla { get; set; }

        public decimal Presupuesto { get; set; }

        public string Necesidades { get; set; }

        public DateTime Fecha { get; set; }

        public string Estado { get; set; }
    }


    public class VendedorContactoViewModel
    {
        public int IdContacto { get; set; }

        public string Nombre { get; set; }

        public string Correo { get; set; }

        public string Asunto { get; set; }

        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; }

        public string Estado { get; set; }
    }


    public class VendedorClienteViewModel
    {
        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; }

        public string Correo { get; set; }

        public string Telefono { get; set; }

        public bool Estado { get; set; }
    }
}
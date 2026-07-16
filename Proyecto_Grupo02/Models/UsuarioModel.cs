using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Grupo02.Models
{
    public class UsuarioModel
    {
        public int Consecutivo { get; set; }

        [Required(ErrorMessage = "La identificación es obligatoria")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string CorreoElectronico { get; set; }

        public string Telefono { get; set; }

        [DataType(DataType.Password)]
        public string Contrasenna { get; set; }

        [DataType(DataType.Password)]
        [Compare("Contrasenna", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarContrasenna { get; set; }
    }
}
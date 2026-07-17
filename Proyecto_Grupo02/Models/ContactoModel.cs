using System.ComponentModel.DataAnnotations;

namespace Proyecto_Grupo02.Models
{
    public class ContactoModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un correo válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El asunto es obligatorio")]
        public string Asunto { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio")]
        public string Mensaje { get; set; }
    }
}
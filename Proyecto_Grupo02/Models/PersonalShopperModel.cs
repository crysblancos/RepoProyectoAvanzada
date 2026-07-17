using System.ComponentModel.DataAnnotations;

namespace Proyecto_Grupo02.Models
{
    public class PersonalShopperModel
    {
        [Required(ErrorMessage = "El estilo buscado es obligatorio")]
        public string EstiloBuscado { get; set; }

        [Required(ErrorMessage = "La talla es obligatoria")]
        public string Talla { get; set; }

        [Required(ErrorMessage = "El presupuesto es obligatorio")]
        public decimal Presupuesto { get; set; }

        public string Necesidades { get; set; }
    }
}
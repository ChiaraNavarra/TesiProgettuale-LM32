using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.Models
{
    public class EventoViewModel
    {
        public long Id { get; set; }
        [DisplayName("Titolo")]
        [Required(ErrorMessage = "Il Titolo è obbligatorio")]
        public string? Titolo { get; set; }
        [DisplayName("Comune")]
        [Required(ErrorMessage = "Il Comune è obbligatorio")]
        public string? Comune { get; set; }
        [DisplayName("DataEvento")]
        [Required(ErrorMessage = "Il DataEvento è obbligatorio")]
        public DateTime? DataEvento { get; set; }
        public string? Descrizione { get; set; }
        public string? TipoEvento { get; set; }

    }
}

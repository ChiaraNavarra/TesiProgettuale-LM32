using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.Models
{   

    public class VeterinarioViewModel
    {
        public string? Id { get; set; }
        [DisplayName("Nome")]
        [Required(ErrorMessage = "Il Nome è obbligatorio")]
        public string? Nome { get; set; }
        [DisplayName("Cognome")]
        [Required(ErrorMessage = "Il Cognome è obbligatorio")]
        public string? Cognome { get; set; }
        public string? NumeroTelefono { get; set; }
        public string? Note { get; set; }
        public string? Comune { get; set; }
        public string? Indirizzo { get; set; }
        public string? TipoVeterinario { get; set; }
        public string? Email { get; set; }
    }
}

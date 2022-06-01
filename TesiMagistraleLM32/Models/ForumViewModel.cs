using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.Models
{
    public class ForumViewModel
    {
        public long Id { get; set; }
        [DisplayName("Testo")]
        [Required(ErrorMessage = "Il Testo è obbligatorio")]
        public string? Testo { get; set; }
        [DisplayName("TipoForum")]
        [Required(ErrorMessage = "Il Tipo Forum è obbligatorio")]
        public string? TipoForum { get; set; }
        public string? IdUtente { get; set; }
        public DateTimeOffset? DataInizio { get; set; }
        public string? UsernameUtente { get; set; }
        [DisplayName("Titolo")]
        [Required(ErrorMessage = "Il Titolo è obbligatorio")]
        public string? Titolo { get; set; }
        public List<CommentoViewModel> Commenti { get; set; }


    }
}

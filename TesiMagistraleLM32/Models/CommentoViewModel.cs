using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.Models
{
    public class CommentoViewModel
    {
        public long Id { get; set; }
        public long IdForum { get; set; }
        public string? Testo { get; set; }
        public string? IdUtente { get; set; }
        public DateTimeOffset? DataInizio { get; set; }
        public string? UsernameUtente { get; set; }


    }
}

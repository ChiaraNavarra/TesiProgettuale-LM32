using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.ApiSql.Models
{

    public class CommentoModel
    {
        public long Id { get; set; }
        public string? Testo { get; set; }
        public string? IdUtente { get; set; }
        public string? UsernameUtente { get; set; }
        public long? IdForum { get; set; }
        public DateTimeOffset? DataInizio { get; set; }
    }
}

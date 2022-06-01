using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.ApiSql.Models
{

    public class ForumModel
    {
        public long Id { get; set; }
        public string? Testo { get; set; }
        public string? TipoForum { get; set; }
        public string? IdUtente { get; set; }
        public string? UsernameUtente { get; set; }
        public DateTimeOffset? DataInizio { get; set; }
        public string? Titolo { get; set; }
    }
}

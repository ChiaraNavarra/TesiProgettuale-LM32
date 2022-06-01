using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.ApiSql.Models
{

    public class EventoModel
    {
        public long Id { get; set; }
        public string? Titolo { get; set; }
        public string? Comune { get; set; }
        public DateTimeOffset? DataEvento { get; set; }
        public string? Descrizione { get; set; }
        public string? TipoEvento { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.ApiSql.Models
{
    public class PetsitterModel
    {
        public string? Id { get; set; }
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? NumeroTelefono { get; set; }
        public string? Note { get; set; }
        public string? Comune { get; set; }
        public string? Email { get; set; }

    }
}

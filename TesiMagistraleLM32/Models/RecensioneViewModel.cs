using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.Models
{   

    public class RecensioneViewModel
    {
        public long? Id { get; set; }
        public string? Testo { get; set; }
        public string? IdAddestratore { get; set; }
        public string? IdPetsitter { get; set; }
        public string? IdVeterinario { get; set; }
        public string? IdUtente { get; set; }
        public string? UsernameUtente { get; set; }
    }
}

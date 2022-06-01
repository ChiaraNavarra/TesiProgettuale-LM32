using System.ComponentModel.DataAnnotations;

namespace TesiMagistraleLM32.Models
{
    public class RazzaCaninaViewModel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? bred_for { get; set; }
        public string? breed_group { get; set; }
        public string? life_span { get; set; }
        public string? temperament { get; set; }
        public string? img { get; set; }
        public string? origin { get; set; }
    }
}

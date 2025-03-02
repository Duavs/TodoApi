using System.ComponentModel.DataAnnotations;

namespace TodoApi.Config
{
    public class JwtSettings
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Issuer { get; set; }

        [Required]
        public string Audience { get; set; }
    }
}
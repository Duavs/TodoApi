using System.ComponentModel.DataAnnotations;

namespace TodoApi.Config
{
    public class JwtSettings
    {
        [Required]
        public required string  Key { get; set; }

        [Required]
        public required string Issuer { get; set; }

        [Required]
        public required string Audience { get; set; }
        
        public double ExpirationInMinutes { get; set; }  
    }
}
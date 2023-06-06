using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallets.Dtos
{
    public class WalletUpdateDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string AccountNumber { get; set; }
        [Required]
        public string AccountScheme { get; set; }
        [Required]
        public string OwnerPhoneNumber { get; set; }
    }
}

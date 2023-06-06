using System;
using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallets.Models
{
    public class WalletModel
    {
        //decoration with data annotations to show data is not nullable
        //wallet id
        [Key] //key annotation
        public int Id { get; set; }

        //name of wallet
        [Required]
        public string Name { get; set; }

        //type of wallet, card or momo
        [Required]
        public string Type { get; set; }

        //momo number or card number
        [Required]
        public string AccountNumber { get; set; }

        //account provider, mastercard/visa/airteltigo/vodafone/mtn
        [Required]
        public string AccountScheme { get; set; }

        //Phone numbe of wallet owner
        [Required]
        public string OwnerPhoneNumber { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }


    }
}


using System;

namespace Hubtel.Wallets.Dtos
{
    public class WalletReadDto
    {
     
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string AccountNumber { get; set; }

        public string AccountScheme { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}

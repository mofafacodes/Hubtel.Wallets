using Hubtel.Wallets.Models;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallets.Data
{
    public class WalletContext : DbContext
    {
        public WalletContext(DbContextOptions<WalletContext> options) : base(options) { }

        public DbSet<WalletModel> Wallets { get; set; }

    }

}

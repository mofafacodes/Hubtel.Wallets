using Hubtel.Wallets.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hubtel.Wallets.Interfaces
{
        public interface IWalletRepoAsync
        {
            Task CreateWalletAsync(WalletModel wallet);

            Task<WalletModel> GetWalletByIdAsync(int id);

            Task UpdateWalletAsync(WalletModel wallet);

            Task DeleteWalletAsync(WalletModel wallet);

            Task<IEnumerable<WalletModel>> GetAllWalletsByAdminAsync();

            Task<IEnumerable<WalletModel>> GetAllWalletsByOwnerAsync(string ownerPhoneNumber);

            Task<bool> SaveChangesAsync();
   
        }
    
}

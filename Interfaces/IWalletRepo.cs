using Hubtel.Wallets.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hubtel.Wallets.Interfaces
{
    public interface IWalletRepo
    {
        void CreateWallet(WalletModel wallet);

        WalletModel GetWalletById(int id);

        void UpdateWallet(WalletModel wallet);

        void DeleteWallet(WalletModel wallet);

        IEnumerable<WalletModel> GetAllWalletsByAdmin();

        IQueryable<WalletModel> GetAllWalletsByOwner(string ownerPhoneNumber);

        bool SaveChanges();

    }
}
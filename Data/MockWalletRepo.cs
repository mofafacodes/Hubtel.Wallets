using Hubtel.Wallets.Interfaces;
using Hubtel.Wallets.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hubtel.Wallets.Data
{
    public class MockWalletRepo : IWalletRepo
    {
        public WalletModel CreateWallet(WalletModel wallet)
        {
            var walletItem = new WalletModel{ 
                Id = wallet.Id, 
                Name = wallet.Name, 
                Type = wallet.Type,
                AccountNumber = wallet.AccountNumber,
                AccountScheme = wallet.AccountScheme,
                OwnerPhoneNumber = wallet.OwnerPhoneNumber
                };
        return walletItem;
        }

        public WalletModel GetWalletById(int id)
        {
            throw new System.NotImplementedException();
        }

        public WalletModel UpdateWalletById(WalletModel wallet, int id)
        {
            throw new System.NotImplementedException();
        }

        public WalletModel DeleteWalletById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WalletModel> GetAllWalletsByAdmin()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<WalletModel> GetAllWalletsByOwner(string ownerPhoneNumber)
        {
            throw new System.NotImplementedException();
        }

        void IWalletRepo.CreateWallet(WalletModel wallet)
        {
            throw new System.NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateWalletById(WalletModel wallet)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateWallet(WalletModel wallet)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteWallet(WalletModel wallet)
        {
            throw new System.NotImplementedException();
        }

        IQueryable<WalletModel> IWalletRepo.GetAllWalletsByOwner(string ownerPhoneNumber)
        {
            throw new System.NotImplementedException();
        }
    }
}

using Hubtel.Wallets.Interfaces;
using Hubtel.Wallets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Hubtel.Wallets.Data
{
    public class SqlWalletRepo : IWalletRepo
    {
        private readonly WalletContext _context;

        //making use of DBContext calls to get acces to DB by injection it in constructur class

        public SqlWalletRepo(WalletContext context)
        {
            _context = context;
        }
        public void CreateWallet(WalletModel wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }
           
            _context.Wallets.Add(wallet);
        }

        public WalletModel GetWalletById(int id)
        {
            return _context.Wallets.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateWallet(WalletModel wallet)
        {
            //Nothing
        }

        public void DeleteWallet(WalletModel wallet)
        {
            if(wallet == null)
            {
                throw new ArgumentNullException();
            }
            _context.Wallets.Remove(wallet);

        }

        public IEnumerable<WalletModel> GetAllWalletsByAdmin()
        {
            return _context.Wallets;
        }

        public IQueryable<WalletModel> GetAllWalletsByOwner(string ownerPhoneNumber)
        {
            return _context.Wallets.Where(x => x.OwnerPhoneNumber == ownerPhoneNumber);
        }

        public bool SaveChanges()
        {
            //operations  will only be replicated in the db of you this save changes
            return (_context.SaveChanges() >= 0);

        }
    }
}

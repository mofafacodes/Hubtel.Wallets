using Hubtel.Wallets.Interfaces;
using Hubtel.Wallets.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel.Wallets.Data
{
    public class SqlWalletRepoAsync: IWalletRepoAsync
    {
        private readonly WalletContext _context;

        //making use of DBContext calls to get acces to DB by injection it in constructur class

        public SqlWalletRepoAsync(WalletContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Creates a new wallet.
        /// </summary>
        /// <param name="wallet">The wallet to create.</param>
        public async Task CreateWalletAsync(WalletModel wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentNullException(nameof(wallet));
            }

            await _context.Wallets.AddAsync(wallet);
        }

        /// <summary>
        /// Retrieves a wallet by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to retrieve.</param>
        /// <returns>The wallet with the specified ID.</returns>
        public async Task<WalletModel> GetWalletByIdAsync(int id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Updates a wallet.
        /// </summary>
        /// <param name="wallet">The updated wallet.</param>
        public Task UpdateWalletAsync(WalletModel wallet)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a wallet.
        /// </summary>
        /// <param name="wallet">The wallet to delete.</param>
        public async Task DeleteWalletAsync(WalletModel wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentNullException();
            }
            _context.Wallets.Remove(wallet);
          await  Task.CompletedTask;

        }

        /// <summary>
        /// Retrieves all wallets (admin view).
        /// </summary>
        /// <returns>A collection of all wallets.</returns>
        public async Task<IEnumerable<WalletModel>> GetAllWalletsByAdminAsync()
        {
            return await _context.Wallets.ToListAsync();
        }

        /// <summary>
        /// Retrieves all wallets owned by a specific owner.
        /// </summary>
        /// <param name="ownerPhoneNumber">The owner of the wallets.</param>
        /// <returns>A collection of wallets owned by the specified owner.</returns>
        public async Task<IEnumerable<WalletModel>> GetAllWalletsByOwnerAsync(string ownerPhoneNumber)
        {
            return await _context.Wallets.Where(x => x.OwnerPhoneNumber == ownerPhoneNumber).ToListAsync();
        }

        /// <summary>
        /// Saves changes made to the database.
        /// </summary>
        /// <returns>True if the changes were successfully saved, otherwise false.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) >= 0;
        }
    }
}

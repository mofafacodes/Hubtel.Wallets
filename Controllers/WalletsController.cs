using AutoMapper;
using FluentValidation;
using Hubtel.Wallets.Data;
using Hubtel.Wallets.Dtos;
using Hubtel.Wallets.Interfaces;
using Hubtel.Wallets.Models;
using Hubtel.Wallets.Utilities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hubtel.Wallets.Controllers
{
    //api/wallets
    [ApiController]
    [Route("api/wallets")]
    public class WalletsController : ControllerBase
    {
        //private readonly MockWalletRepo _mockWalletRepo = new MockWalletRepo();
        private readonly IWalletRepoAsync _repository;
        private readonly IMapper _mapper;
        private readonly IValidator<WalletCreateDto> _validator;

        //dependency injected the Iwallet Repostory
        public WalletsController(IWalletRepoAsync respository, IMapper mapper, IValidator<WalletCreateDto> validator )
        {
            _repository = respository;
            _mapper = mapper;
            _validator = validator;
        }

        //POST api/wallets/create
        /// <summary>
        /// Creates a new wallet.
        /// </summary>
        /// <param name="wallet">The wallet model to create.</param>
        /// <returns>The created wallet.</returns>
        [HttpPost("create")]
        public async Task<ActionResult<WalletReadDto>> CreateWallet([FromBody] WalletCreateDto wallet)
        {
            var validateResult = await _validator.ValidateAsync(wallet);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var ownerWallets = await _repository.GetAllWalletsByOwnerAsync(wallet.OwnerPhoneNumber);

            if (ownerWallets.Count() >= 5)
            {
                return BadRequest("You have a limit of 5 wallets. You cannot add additional wallets at this time.");
            }

            if (ownerWallets.Count() > 1)
            {
                foreach (var item in ownerWallets)
                {
                    if (item.Name != wallet.Name)
                    {
                        return BadRequest("Wallet name should be the same for all wallets.");
                    }
                }
            }

            var allWallets = await _repository.GetAllWalletsByAdminAsync();

            WalletModel existingAccountNumber = null;
            if (wallet.Type == "momo")
            {
                existingAccountNumber = allWallets.FirstOrDefault(x => x.AccountNumber == wallet.AccountNumber);
            }
            else if (wallet.Type == "card")
            {
                existingAccountNumber = allWallets.FirstOrDefault(x => x.AccountNumber == Helpers.HashBankCard(wallet.AccountNumber));
            }

            if (existingAccountNumber != null)
            {
                return BadRequest("A wallet with this account number already exists.");
            }

            WalletModel newWalletItem = new WalletModel();

            if (wallet.Type == "momo")
            {
                newWalletItem.AccountNumber = wallet.AccountNumber;
            }
            else if (wallet.Type == "card")
            {
                newWalletItem.AccountNumber = Helpers.HashBankCard(wallet.AccountNumber);
            }

            newWalletItem.Name = wallet.Name;
            newWalletItem.Type = wallet.Type;
            newWalletItem.AccountScheme = wallet.AccountScheme;
            newWalletItem.OwnerPhoneNumber = wallet.OwnerPhoneNumber;
            newWalletItem.CreatedAt = DateTimeOffset.UtcNow;
            newWalletItem.UpdatedAt = DateTimeOffset.UtcNow;

            var walletCreateDto = _mapper.Map<WalletModel>(newWalletItem);
            await _repository.CreateWalletAsync(walletCreateDto);
            await _repository.SaveChangesAsync();

            var walletReadDto = _mapper.Map<WalletReadDto>(walletCreateDto);

            return CreatedAtRoute(nameof(GetWalletByIdAsync), new { Id = walletReadDto.Id }, walletReadDto);
        }

        //GET api/wallets/{id}
        /// <summary>
        /// Retrieves a wallet by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to retrieve.</param>
        /// <returns>The wallet with the specified ID.</returns>
        [HttpGet("{id}", Name = "GetWalletByIdAsync")]
        public async Task<ActionResult<WalletReadDto>> GetWalletByIdAsync([FromRoute] int id)
        {
            var wallet = await _repository.GetWalletByIdAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalletReadDto>(wallet));
        }

        // <summary>
        /// Updates a wallet by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to update.</param>
        /// <param name="wallet">The updated wallet data.</param>
        /// <returns>The updated wallet.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateWalletAsync([FromRoute] int id, [FromBody] WalletUpdateDto wallet)
        {
            var walletFromRepo = await _repository.GetWalletByIdAsync(id);
            if (walletFromRepo == null)
            {
                return NotFound();
            }
            var allWallets = await _repository.GetAllWalletsByAdminAsync();

            WalletModel existingAccountNumber = null;
            if (wallet.Type == "momo")
            {
                existingAccountNumber = allWallets.FirstOrDefault(x => x.AccountNumber == wallet.AccountNumber);
            }
            else if (wallet.Type == "card")
            {
                existingAccountNumber = allWallets.FirstOrDefault(x => x.AccountNumber == Helpers.HashBankCard(wallet.AccountNumber));
            }
            if (existingAccountNumber != null)
            {
                return BadRequest("A wallet with this account number already exists.");
            }

            WalletModel newWalletItem = new WalletModel();

            if (wallet.Type == "momo")
            {
                newWalletItem.AccountNumber = wallet.AccountNumber;
            }
            else if (wallet.Type == "card")
            {
                newWalletItem.AccountNumber = Helpers.HashBankCard(wallet.AccountNumber);
            }

            newWalletItem.Name = wallet.Name;
            newWalletItem.Type = wallet.Type;
            newWalletItem.AccountScheme = wallet.AccountScheme;
            newWalletItem.OwnerPhoneNumber = wallet.OwnerPhoneNumber;
            newWalletItem.CreatedAt = walletFromRepo.CreatedAt;
            newWalletItem.UpdatedAt = DateTimeOffset.UtcNow;

            _mapper.Map(newWalletItem, walletFromRepo);
            await _repository.UpdateWalletAsync(walletFromRepo);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        //DELETE api/wallets/{id}
        /// <summary>
        /// Deletes a wallet by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to delete.</param>
        /// <returns>The deleted wallet.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWalletByIdAsync([FromRoute] int id)
        {
            var walletFromRepo = await _repository.GetWalletByIdAsync(id);
            if (walletFromRepo == null)
            {
                return NotFound();
            }
            await _repository.DeleteWalletAsync(walletFromRepo);
            await _repository.SaveChangesAsync();
            return NoContent();
        }

        //PATCH api/wallets/{id}
        /// <summary>
        /// Updates a wallet document partially by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to update.</param>
        /// <returns>No content.</returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartialUpdateOfWalletAsync([FromRoute] int id, [FromBody] JsonPatchDocument<WalletUpdateDto> patchDoc)
        {
            var walletFromRepo = await _repository.GetWalletByIdAsync(id);
            if (walletFromRepo == null)
            {
                return NotFound();
            }
            var walletToPatch = _mapper.Map<WalletUpdateDto>(walletFromRepo);
            // Model state makes sure validations are valid
            patchDoc.ApplyTo(walletToPatch, ModelState);

            if (!TryValidateModel(walletToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(walletToPatch, walletFromRepo);
            await _repository.UpdateWalletAsync(walletFromRepo);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        //GET api/wallets/owner
        /// <summary>
        /// Retrieves all wallets owned by a specific owner.
        /// </summary>
        /// <param name="ownerPhoneNumber">The owner of the wallets.</param>
        /// <returns>A collection of wallets owned by the specified owner.</returns>
        [HttpGet("owner")]
        public async Task<ActionResult<IEnumerable<WalletReadDto>>> GetAllWalletsByOwnerAsync([FromQuery] string ownerPhoneNumber)
        {
            var wallets = await _repository.GetAllWalletsByOwnerAsync(ownerPhoneNumber);

            if (wallets == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<WalletReadDto>>(wallets));
        }

        //GET api/wallets/all
        /// <summary>
        /// Retrieves all wallets (admin view).
        /// </summary>
        /// <returns>A collection of all wallets.</returns>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<WalletReadDto>>> GetAllWalletsByAdminAsync()
        {
            var wallets = await _repository.GetAllWalletsByAdminAsync();
            if (wallets == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<WalletReadDto>>(wallets));
        }

    }
}

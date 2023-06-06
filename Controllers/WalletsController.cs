using AutoMapper;
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

namespace Hubtel.Wallets.Controllers
{
    //api/wallets
    [ApiController]
    [Route("api/wallets")]
    public class WalletsController : ControllerBase
    {
        //private readonly MockWalletRepo _mockWalletRepo = new MockWalletRepo();
        private readonly IWalletRepo _repository;
        private readonly IMapper _mapper;

        //dependency injected the Iwallet Repostory
        public WalletsController(IWalletRepo respository, IMapper mapper)
        {
            _repository = respository;
            _mapper = mapper;
        }


        //POST api/wallets/create
        /// <summary>
        /// Creates a new wallet.
        /// </summary>
        /// <param name="wallet">The wallet model to create.</param>
        /// <returns>The created wallet.</returns>
        [HttpPost("create")]
        public ActionResult<WalletReadDto> CreateWallet( [FromBody] WalletCreateDto wallet)
        {
            var ownerWallets = _repository.GetAllWalletsByOwner(wallet.OwnerPhoneNumber);
            if (ownerWallets.Count() >= 5)
            {
                return BadRequest("You have a limit of 5 wallets. You cannot additional wallets at this time.");
            }
            var allWallets = _repository.GetAllWalletsByAdmin();

            WalletModel existingAccountNumber = new WalletModel();
            if(wallet.Type == "momo")
            {
                existingAccountNumber = allWallets.FirstOrDefault(x => x.AccountNumber == wallet.AccountNumber);
            }
            if (wallet.Type == "card")
            {
                existingAccountNumber = allWallets.FirstOrDefault(x => x.AccountNumber == Helpers.HashBankCard(wallet.AccountNumber));
            }

            if (existingAccountNumber != null)
            {
                return BadRequest("A wallet with the this account number already exists.");
            }

            WalletModel newWalletItem = new WalletModel();


            if(wallet.Type == "momo")
            {
                newWalletItem.AccountNumber = wallet.AccountNumber;
            }

            if (wallet.Type == "card")
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
            _repository.CreateWallet(walletCreateDto);
            _repository.SaveChanges();

            var walletReadDto = _mapper.Map<WalletReadDto>(walletCreateDto);

            return CreatedAtRoute(nameof(GetWalletById), new {Id = walletReadDto.Id}, walletReadDto);

        }

        //GET api/wallets/{id}
        /// <summary>
        /// Retrieves a wallet by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to retrieve.</param>
        /// <returns>The wallet with the specified ID.</returns>
        [HttpGet("{id}", Name = "GetWalletById")]
        public ActionResult<WalletReadDto> GetWalletById( [FromRoute] int id)
        {
            var wallet = _repository.GetWalletById(id);
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
        public ActionResult UpdateWallet( [FromRoute] int id, [FromBody] WalletUpdateDto wallet)
        {
            var walletFromRepo = _repository.GetWalletById(id);
            if(walletFromRepo == null) 
            {
                return NotFound();
            }
            var allWallets = _repository.GetAllWalletsByAdmin();

            WalletModel existingAccountNumber = new WalletModel();
            if (wallet.Type == "momo")
            {
                existingAccountNumber = allWallets.FirstOrDefault(x => x.AccountNumber == wallet.AccountNumber);
            }
            if (wallet.Type == "card")
            {
                existingAccountNumber = allWallets.FirstOrDefault(x => x.AccountNumber == Helpers.HashBankCard(wallet.AccountNumber));
            }
            if (existingAccountNumber != null)
            {
                return BadRequest("A wallet with the this account number already exists.");
            }

            WalletModel newWalletItem = new WalletModel();

            if (wallet.Type == "momo")
            {
                newWalletItem.AccountNumber = wallet.AccountNumber;
            }

            if (wallet.Type == "card")
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
            _repository.UpdateWallet(walletFromRepo);
            _repository.SaveChanges();
   
            return NoContent();
        }

        //DELETE api/wallets/{id}
        /// <summary>
        /// Deletes a wallet by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to delete.</param>
        /// <returns>The deleted wallet.</returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteWalletById([FromRoute] int id)
        {
            var walletFromRepo = _repository.GetWalletById(id);
            if (walletFromRepo == null)
            {
                return NotFound();
            }
            _repository.DeleteWallet(walletFromRepo);
            _repository.SaveChanges();
            return NoContent();
        }

        //PATCH api/wallets/{id}
        /// <summary>
        /// Updates a wallet document partially by its ID.
        /// </summary>
        /// <param name="id">The ID of the wallet to update.</param>
        /// <returns>No cntent.</returns>
        [HttpPatch("{id}")]
        public ActionResult PartialUpdateOfWallet([FromRoute] int id, [FromBody] JsonPatchDocument<WalletUpdateDto> patchDoc)
        {
            var walletFromRepo = _repository.GetWalletById(id);
            if (walletFromRepo == null)
            {
                return NotFound();
            }
            var walletToPatch = _mapper.Map<WalletUpdateDto>(walletFromRepo);
            //model state makes sure validations are valid
            patchDoc.ApplyTo(walletToPatch, ModelState);

            if (!TryValidateModel(walletToPatch))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(walletToPatch, walletFromRepo);
            _repository.UpdateWallet(walletFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        //GET api/wallets/owner
        /// <summary>
        /// Retrieves all wallets owned by a specific owner.
        /// </summary>
        /// <param name="owner">The owner of the wallets.</param>
        /// <returns>A collection of wallets owned by the specified owner.</returns>
        [HttpGet("owner")]
        public ActionResult<IEnumerable<WalletReadDto>> GetAllWalletsByOwner([FromBody] string ownerPhoneNumber)
        {
            var wallets = _repository.GetAllWalletsByOwner(ownerPhoneNumber).ToList();
 
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
        public ActionResult<IEnumerable<WalletReadDto>> GetAllWalletsByAdmin()
        {
            var wallets = _repository.GetAllWalletsByAdmin().ToList();
            if (wallets == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<WalletReadDto>>(wallets));

        }

    }
}

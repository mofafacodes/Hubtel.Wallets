using AutoMapper;
using Hubtel.Wallets.Dtos;
using Hubtel.Wallets.Models;

namespace Hubtel.Wallets.Profiles
{
    public class WalletProfile : Profile
    {
        public WalletProfile() 
        {
            //source -> target
            CreateMap<WalletModel, WalletReadDto>();
            CreateMap<WalletCreateDto, WalletModel>();
            CreateMap<WalletUpdateDto, WalletModel>();
            CreateMap<WalletModel, WalletUpdateDto>();
        }
    }
}

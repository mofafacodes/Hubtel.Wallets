using FluentValidation;
using Hubtel.Wallets.Dtos;

namespace Hubtel.Wallets.Utilities
{
    public class CreateValidation: AbstractValidator<WalletCreateDto>
    {
        public CreateValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Type of wallet is required")
                .Must(x => x == "momo" || x == "card")
                .WithMessage("Type must be either 'momo' or 'card'");

            RuleFor(x => x.AccountNumber)
                .Must(x => x.Length >= 6 && x.Length <= 16)
                .When(x => x.Type == "card")
                .WithMessage("Card number must be between 6 and 16 characters")
                .Must(x => Helpers.BankCardNumberValidator(x))
                .When(x => x.Type == "card")
                .WithMessage("Invalid card number");

            RuleFor(x => x.AccountNumber)
                .Must(x => Helpers.PhoneNumberValidator(x))
                .When(x => x.Type == "momo")
                .WithMessage("Invalid phone number");

            RuleFor(x => x.AccountScheme)
                .Must(x => x == "mtn" || x == "airteltigo" || x == "vodafone")
                .When(x => x.Type == "momo")
                .WithMessage("Momo must be one of 'mtn', 'airteltigo', 'vodafone'");

            RuleFor(x => x.AccountScheme)
                .Must(x => x == "visa" || x == "mastercard")
                .When(x => x.Type == "card")
                .WithMessage("Card must be one of 'visa', 'mastercard'");

            RuleFor(x => x.OwnerPhoneNumber)
                .NotEmpty()
                .WithMessage("Owner phone number is required")
                .Must(x => Helpers.PhoneNumberValidator(x))
                .WithMessage("Invalid phone number");
        }
    }
}

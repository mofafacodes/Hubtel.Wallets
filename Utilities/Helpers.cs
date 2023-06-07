using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Text;

namespace Hubtel.Wallets.Utilities
{
    public static class Helpers
    {
        private static readonly Regex OwnerPhoneNumberPattern = new Regex(@"^(?:[0-9] ?){6,14}[0-9]$");

        public static bool PhoneNumberValidator(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            return OwnerPhoneNumberPattern.IsMatch(phoneNumber);
        }

        public static bool CardNumberValidator(string cardNumber)
        {
            return Regex.Match(cardNumber.Substring(0, 6), @"^\d{6}$").Success;
        }

        public static bool MomoSchemeValidator(string value)
        {
            return value == "mtn" || value == "airteltigo" || value == "vodafone";
        }

        public static bool CardSchemeValidator(string value)
        {
            return value == "visa" || value == "mastercard";
        }

        public static string HashGenerator(string cardNumber)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] hash = sha256Hash.ComputeHash(
                    System.Text.Encoding.UTF8.GetBytes(cardNumber)
                );
                string hashedCardNumber = BitConverter.ToString(hash).Replace("-", string.Empty);
                return hashedCardNumber;
            }
        }

        public static bool BankCardNumberValidator(string number)
        {
            // Remove any non-digit characters from the number
            string cleanedNumber = new string(number.Where(char.IsDigit).ToArray());

            // Check if the number matches any common bank card number patterns
            string[] patterns = {
            "^4[0-9]{12}(?:[0-9]{3})?$",               // Visa
            "^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$",   // Mastercard
            //"^3[47][0-9]{13}$",                         // American Express
            //"^6(?:011|5[0-9]{2})[0-9]{12}$"             // Discover
            // Add more patterns for other card types as needed
            };

            foreach (string pattern in patterns)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(cleanedNumber, pattern))
                {
                    return true; // Number matches a known bank card pattern
                }
            }
            return false; // Number does not match any known bank card pattern
        }


        public static string HashBankCard(string bankCardNumber)
        {
            string firstSixNumbers = bankCardNumber.Substring(0, 6); // Extract the first 6 numbers

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(firstSixNumbers));

                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}

using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CMD.Appointment.Domain.Services
{
    /// <summary>
    /// Custom validation attribute to validate phone numbers based on country-specific rules.
    /// </summary>
    public class PhoneNumberValidatorAttribute : ValidationAttribute
    {
        private readonly string _defaultRegion;
        private static readonly Dictionary<string, (int MinLength, int MaxLength)> CountryCodeLengths = new()
        {
            { "91", (10, 10) } // India: 10 digits (excluding country code)
            // Add more country codes and their length ranges as needed
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneNumberValidatorAttribute"/> class.
        /// </summary>
        /// <param name="defaultRegion">The default region to use for phone number validation (e.g., "IN" for India).</param>
        public PhoneNumberValidatorAttribute(string defaultRegion = null)
        {
            _defaultRegion = defaultRegion;
        }

        /// <summary>
        /// Validates the phone number according to the specified country code rules.
        /// </summary>
        /// <param name="value">The value of the phone number to validate.</param>
        /// <param name="validationContext">The context of the validation.</param>
        /// <returns>Returns a <see cref="ValidationResult"/> indicating whether the phone number is valid or not.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var phoneNumberStr = value as string;

            if (string.IsNullOrEmpty(phoneNumberStr))
            {
                return new ValidationResult("Phone number is required.");
            }

            try
            {
                var phoneNumberUtil = PhoneNumberUtil.GetInstance();

                // Ensure phone number starts with "+"
                if (!phoneNumberStr.StartsWith("+"))
                {
                    phoneNumberStr = $"+{phoneNumberStr}";
                }

                // Check if phone number starts with the correct country code
                if (!phoneNumberStr.StartsWith($"+{_defaultRegion}"))
                {
                    return new ValidationResult($"Phone number must start with country code {_defaultRegion}.");
                }

                // Remove the country code from the phone number
                var phoneWithoutCountryCode = phoneNumberStr.Substring(_defaultRegion.Length + 1); // +1 for the "+" sign

                // Check if the country code is valid and get the expected length
                if (!CountryCodeLengths.TryGetValue(_defaultRegion, out var lengthRange))
                {
                    return new ValidationResult($"Country code {_defaultRegion} is not supported.");
                }

                // Check the length of the phone number (excluding the country code)
                if (phoneWithoutCountryCode.Length < lengthRange.MinLength || phoneWithoutCountryCode.Length > lengthRange.MaxLength)
                {
                    return new ValidationResult($"Phone number must be {lengthRange.MaxLength} digits long (excluding country code) for country code {_defaultRegion}.");
                }

                // Ensure the phone number contains only digits
                if (!phoneWithoutCountryCode.All(char.IsDigit))
                {
                    return new ValidationResult("Phone number must contain only digits.");
                }

                // Parse and validate the phone number using libphonenumber
                var phoneNumber = phoneNumberUtil.Parse(phoneNumberStr, _defaultRegion);
                bool isValid = phoneNumberUtil.IsValidNumber(phoneNumber);

                if (!isValid)
                {
                    return new ValidationResult($"The phone number with country code {_defaultRegion} is not valid.");
                }

                return ValidationResult.Success;
            }
            catch (NumberParseException)
            {
                return new ValidationResult("Invalid phone number format.");
            }
        }
    }
}

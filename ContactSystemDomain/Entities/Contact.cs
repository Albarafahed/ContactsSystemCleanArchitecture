using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsSystemDomain.Entities
{
      public sealed class Contact
    {

        public int ContactId { get; private set; }

        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Phone { get; private set; } = string.Empty;
        public string Address { get; private set; } = string.Empty;

        public DateTime DateOfBirth { get; private set; }

        public int CountryId { get; private set; }

        public string? ImagePath { get; private set; }

        public  Country Country { get; private set; } = null!;

        private Contact() { }

        // Create a new Contact
        public Contact(
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            DateTime dateOfBirth,
            int countryId,
            string? imagePath = null)
        {
            SetFirstName(firstName);
            SetLastName(lastName);
            SetEmail(email);
            SetPhone(phone);
            SetAddress(address);
            SetDateOfBirth(dateOfBirth);
            SetCountryId(countryId);
            SetImagePath(imagePath);
        }

        public void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException(
                    "First name is required.", nameof(firstName));

            if (firstName.Trim().Length > 50)
                throw new ArgumentException(
                    "First name cannot exceed 50 characters.", nameof(firstName));

            FirstName = firstName.Trim();
        }

        public void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException(
                    "Last name is required.", nameof(lastName));

            if (lastName.Trim().Length > 50)
                throw new ArgumentException(
                    "Last name cannot exceed 50 characters.", nameof(lastName));

            LastName = lastName.Trim();
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(
                    "Email is required.", nameof(email));

            if (email.Trim().Length > 50)
                throw new ArgumentException(
                    "Email cannot exceed 50 characters.", nameof(email));

            Email = email.Trim();
        }

        public void SetPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException(
                    "Phone is required.", nameof(phone));

            if (phone.Trim().Length > 20)
                throw new ArgumentException(
                    "Phone cannot exceed 20 characters.", nameof(phone));
            if (phone.Trim().Length < 9)
                throw new ArgumentException(
                    "Phone cannot exceed 9 characters.", nameof(phone));

            Phone = phone.Trim();
        }

        public void SetAddress(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException(
                    "Address is required.", nameof(address));

            if (address.Trim().Length > 200)
                throw new ArgumentException(
                    "Address cannot exceed 200 characters.", nameof(address));

            Address = address.Trim();
        }

        public void SetDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth.Date > DateTime.Today)
                throw new ArgumentException(
                    "Date of birth cannot be in the future.", nameof(dateOfBirth));

            DateOfBirth = dateOfBirth;
        }

        public void SetCountryId(int countryId)
        {
            if (countryId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(countryId), "Country ID must be greater than zero.");

            CountryId = countryId;
        }

        public void SetImagePath(string? imagePath)
        {
            if (imagePath?.Length > 500)
                throw new ArgumentException(
                    "Image path cannot exceed 500 characters.", nameof(imagePath));

            ImagePath = string.IsNullOrWhiteSpace(imagePath)
                ? null
                : imagePath.Trim();
        }
    }
}

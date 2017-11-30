using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Model;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Wilhelm.Backend.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IWContextFactory _wContextFactory;
        private readonly IConversionService _conversionService;
        private readonly HashAlgorithm _cryptoServiceProvider = new SHA256CryptoServiceProvider();
        private const int SaltValueSize = 1;
        private const string ValidationPattern = "^[a-zA-Z0-9_]*$";

        public AccountsService(IWContextFactory wContextFactory, IConversionService conversionService)
        {
            _wContextFactory = wContextFactory;
            _conversionService = conversionService;
        }

        public Validated<UserDto> CreateUser(string login, string password, string confirmPassword)
        {
            var validatedUser = CreateWUser(login, password, confirmPassword);
            var validatedUserDto = new Validated<UserDto>();
            validatedUserDto.Object = new UserDto();

            if (validatedUser.Object != null)
                _conversionService.ConvertToDto(validatedUserDto.Object, validatedUser.Object);
            validatedUserDto.ValidationViolations = validatedUser.ValidationViolations;

            return validatedUserDto;
        }
        public Validated<UserDto> VerifyUser(string login, string password)
        {
            var validatedUser = VerifyWUser(login, password);
            var validatedUserDto = new Validated<UserDto>();
            validatedUserDto.Object = new UserDto();

            if (validatedUser.Object != null)
                _conversionService.ConvertToDto(validatedUserDto.Object, validatedUser.Object);
            validatedUserDto.ValidationViolations = validatedUser.ValidationViolations;

            return validatedUserDto;
        }
        private Validated<WUser> CreateWUser(string login, string password, string confirmPassword)
        {
            var validatedUser = new Validated<WUser>();
            validatedUser.ValidationViolations = new List<string>();

            validatedUser.ValidationViolations.AddRange(ValidateLogin(login));
            validatedUser.ValidationViolations.AddRange(ValidatePasswords(password, confirmPassword));

            if (validatedUser.ValidationViolations.Count == 0)
                validatedUser.Object = CreateAccount(login, password);
            return validatedUser;
        }
        private Validated<WUser> VerifyWUser(string login, string password)
        {
            var validatedUser = new Validated<WUser>();
            validatedUser.ValidationViolations = new List<string>();

            using (var db = _wContextFactory.Create())
            {
                validatedUser.Object = db.Users.Where(x => x.Login == login).SingleOrDefault();
            }

            if (validatedUser.Object == null)
            {
                validatedUser.ValidationViolations.Add("User does not exists.");
            }
            else if (!VerifyHashedPassword(password, validatedUser.Object.Password))
            {
                validatedUser.Object = null;
                validatedUser.ValidationViolations.Add("Password is invalid");
            }

            return validatedUser;
        }

        private WUser CreateAccount(string login, string password)
        {
            var user = new WUser
            {
                Login = login,
                Password = HashPassword(password)
            };

            using (var db = _wContextFactory.Create())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }

            return user;
        }
        private List<string> ValidateLogin(string login)
        {
            var violations = new List<string>();
            if (string.IsNullOrEmpty(login))
            {
                violations.Add("Login cannot be empty.");
            }
            else if (!Regex.IsMatch(login, ValidationPattern))
            {
                violations.Add("Login contains invalid characters.");
            }
            else if (LoginExists(login))
            {
                violations.Add("Login already exists");
            }

            return violations;
        }
        private bool LoginExists(string login)
        {
            var found = false;
            using (var db = _wContextFactory.Create())
            {
                found = db.Users.Any(x => x.Login == login);
            }
            return found;
        }
        private List<string> ValidatePasswords(string password, string confirmPassword)
        {
            var violations = new List<string>();
            if (string.IsNullOrEmpty(password))
            {
                violations.Add("Password cannot be empty.");
            }
            else if (!Regex.IsMatch(password, ValidationPattern))
            {
                violations.Add("Password contains invalid characters.");
            }
            else if (password != confirmPassword)
            {
                violations.Add("Passwords have to be idenical");
            }

            return violations;
        }

        private string HashPassword(string clearData, string saltValue = null)
        {
            if (saltValue == null)
                saltValue = GenerateSaltValue();
            byte[] hashValue = _cryptoServiceProvider.ComputeHash(PreparePasswordToHash(clearData, saltValue));

            string hashedPassword = saltValue;
            foreach (byte hexdigit in hashValue)
                hashedPassword += hexdigit.ToString("X2", CultureInfo.InvariantCulture.NumberFormat);

            return hashedPassword;
        }
        private string GenerateSaltValue()
        {
            UnicodeEncoding utf16 = new UnicodeEncoding();

            var salt = new byte[SaltValueSize * UnicodeEncoding.CharSize];
            using (var random = new RNGCryptoServiceProvider())
                random.GetNonZeroBytes(salt);

            return utf16.GetString(salt);
        }
        private byte[] PreparePasswordToHash(string clearData, string saltValue)
        {
            UnicodeEncoding utf16 = new UnicodeEncoding();

            byte[] binarySaltValue = utf16.GetBytes(saltValue);
            byte[] binaryPassword = utf16.GetBytes(clearData);
            byte[] valueToHash = new byte[binarySaltValue.Length + binaryPassword.Length];

            binaryPassword.CopyTo(valueToHash, binarySaltValue.Length);

            return valueToHash;

        }
        private bool VerifyHashedPassword(string password, string profilePassword)
        {
            if (string.IsNullOrEmpty(profilePassword) || string.IsNullOrEmpty(password) || profilePassword.Length < SaltValueSize)
                return false;

            string saltValue = profilePassword.Substring(0, SaltValueSize);

            string hashedPassword = HashPassword(password, saltValue);
            if (profilePassword.Equals(hashedPassword, StringComparison.Ordinal))
                return true;

            return false;
        }
    }
}

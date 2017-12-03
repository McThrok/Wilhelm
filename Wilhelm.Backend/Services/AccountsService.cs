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
        private readonly IHashService _hashService;
        private const string ValidationPattern = "^[a-zA-Z0-9_]*$";

        public AccountsService(IWContextFactory wContextFactory, IConversionService conversionService, IHashService hashService)
        {
            _wContextFactory = wContextFactory;
            _conversionService = conversionService;
            _hashService = hashService;
        }

        public Validated<UserDto> CreateUserDto(string login, string password, string confirmPassword)
        {
            var validatedUser = CreateUser(login, password, confirmPassword);
            var validatedUserDto = new Validated<UserDto>();

            if (validatedUser.Object != null)
            {
                validatedUserDto.Object = new UserDto();
                _conversionService.ConvertToDto(validatedUserDto.Object, validatedUser.Object);
            }
            validatedUserDto.ValidationViolations = validatedUser.ValidationViolations;

            return validatedUserDto;
        }
        public Validated<UserDto> VerifyUserDto(string login, string password)
        {
            var validatedUser = VerifyUser(login, password);
            var validatedUserDto = new Validated<UserDto>();

            if (validatedUser.Object != null)
            {
                validatedUserDto.Object = new UserDto();
                _conversionService.ConvertToDto(validatedUserDto.Object, validatedUser.Object);
            }
            validatedUserDto.ValidationViolations = validatedUser.ValidationViolations;

            return validatedUserDto;
        }
        public Validated<WUser> CreateUser(string login, string password, string confirmPassword)
        {
            var validatedUser = new Validated<WUser>();
            validatedUser.ValidationViolations = new List<string>();

            validatedUser.ValidationViolations.AddRange(ValidateLogin(login));
            validatedUser.ValidationViolations.AddRange(ValidatePasswords(password, confirmPassword));

            if (validatedUser.ValidationViolations.Count == 0)
                validatedUser.Object = CreateAccount(login, password);
            return validatedUser;
        }
        public Validated<WUser> VerifyUser(string login, string password)
        {
            var validatedUser = new Validated<WUser>();
            validatedUser.ValidationViolations = new List<string>();

            using (var db = _wContextFactory.Create())
            {
                validatedUser.Object = db.Users.Where(x => x.Login == login).SingleOrDefault();
            }

            if (validatedUser.Object == null)
            {
                validatedUser.ValidationViolations.Add("User does not exist.");
            }
            else if (!_hashService.VerifyHashedPassword(password, validatedUser.Object.Password))
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
                Password = _hashService.HashPassword(password)
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

    }
}

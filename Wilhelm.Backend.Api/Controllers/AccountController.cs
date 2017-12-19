using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Services;
using Wilhelm.Shared.Dto;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Api.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountsService _accountsService;
        public AccountController()
        {
            _accountsService = new AccountsService(new WContextFactory(), new ConversionService(), new HashService());
        }

        public ValidatedDto<UserDto> GetNewUser(string login, string password, string confirmPassword)
        {
            return _accountsService.CreateUserDto(login, password, confirmPassword);
        }
        public ValidatedDto<UserDto> GetVerifiedUser(string login, string password)
        {
            return _accountsService.VerifyUserDto(login, password);

        }
    }
}

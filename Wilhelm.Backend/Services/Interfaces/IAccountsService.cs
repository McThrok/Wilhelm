using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.DataAccess;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IAccountsService
    {
        Validated<UserDto> CreateUserDto(string login, string password, string confirmPassword);
        Validated<UserDto> VerifyUserDto(string login, string password);
        Validated<WUser> CreateUser(string login, string password, string confirmPassword);
        Validated<WUser> VerifyUser(string login, string password);
    }
}

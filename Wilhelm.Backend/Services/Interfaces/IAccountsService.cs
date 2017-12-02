using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IAccountsService
    {
        Validated<UserDto> CreateUser(string login, string password, string confirmPassword);
        Validated<UserDto> VerifyUser(string login, string password);
    }
}

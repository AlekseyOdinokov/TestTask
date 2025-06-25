using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAuthenticationService
    {
        bool Register(string username, string password, string role);
        bool Login(string username, string password, out User user);
    }
}

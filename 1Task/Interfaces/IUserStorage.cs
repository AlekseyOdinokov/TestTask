using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IUserStorage
    {
        List<User> LoadUsers();
        void SaveUsers(List<User> users);
    }
}

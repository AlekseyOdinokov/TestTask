using Interfaces;
using Models;
using System;
using System.Linq;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserStorage _storage;

        public AuthenticationService(IUserStorage storage)
        {
            _storage = storage;
        }

        public bool Register(string username, string password, string role)
        {
            if (password.Length < 8)
            {
                Console.WriteLine("Пароль слишком короткий.");
                return false;
            }

            var users = _storage.LoadUsers();
            if (users.Any(u => u.Username == username))
            {
                Console.WriteLine("Пользователь уже существует.");
                return false;
            }

            var user = new User
            {
                Username = username,
                PasswordHash = PasswordUtils.Hash(password),
                Role = role
            };

            users.Add(user);
            _storage.SaveUsers(users);
            return true;
        }

        public bool Login(string username, string password, out User user)
        {
            var users = _storage.LoadUsers();
            var hash = PasswordUtils.Hash(password);
            user = users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);
            return user != null;
        }

        public List<User> GetAllUsers()
        {
            return _storage.LoadUsers();
        }
    }
}

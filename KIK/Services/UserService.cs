using KIK.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KIK.Services
{
    public class UserService : IUserService
    {
        private static ConcurrentBag<UserModel> _userStore;
        static UserService()
        {
            _userStore = new ConcurrentBag<UserModel>();
        }

        public Task<bool> RegisterUser(UserModel userModel)
        {
            _userStore.Add(userModel);
            return Task.FromResult(true);
        }


        public Task<UserModel> GetUserByEmail(string email)
        {
            return Task.FromResult(_userStore.FirstOrDefault(u => u.Email == email));
        }
        public Task UpdateUser(UserModel user)
        {
            _userStore = new ConcurrentBag<UserModel>(_userStore.Where(u => u.Email != user.Email))
            {
                user
            };
            return Task.CompletedTask;
        }

    }
}

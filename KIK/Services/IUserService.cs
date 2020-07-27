using System.Threading.Tasks;
using KIK;
using KIK.Model;

namespace KIK
{
    public interface IUserService
    {
        Task<bool> RegisterUser(UserModel userModel);
        Task<UserModel> GetUserByEmail(string email);
        Task UpdateUser(UserModel user);
    }
}
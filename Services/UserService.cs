using medium_app_back.Models;
using medium_app_back.Repositories;

namespace medium_app_back.Services
{
    public class UserService(UserRepository userRepository)
    {
        public async Task AddUserAsync(User user)
        {
            await userRepository.AddUserAsync(user);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            return user;
        }
    }
}

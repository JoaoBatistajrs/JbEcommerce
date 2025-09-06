using UsersAPI.Application.Model;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Domain.Interfaces.Service;

public interface IUserService
{
    Task<User> CreateUser(UserModel userModel);
    Task<User> CreateAdminUser(AdminCreateUserModel adminUserModel);
    Task<User> GetUserByEmail(string email);
}

using UsersApi.Application.Model;
using UsersApi.Domain.Entities;

namespace UsersApi.Domain.Interfaces.Service;

public interface IUserService
{
    Task<User> CreateUser(UserModel userModel);
    Task<User> CreateAdminUser(AdminCreateUserModel adminUserModel);
    Task<User> GetUserByEmail(string email);
}

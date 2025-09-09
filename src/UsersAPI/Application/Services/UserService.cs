using UsersApi.Application.Model;
using UsersApi.Domain.Entities;
using UsersApi.Domain.Enum;
using UsersApi.Domain.Interfaces.Repository;
using UsersApi.Domain.Interfaces.Service;

namespace UsersApi.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IPasswordService _passwordService;

    public UserService(IUserRepository repository, IPasswordService passwordService)
    {
        _repository = repository;
        _passwordService = passwordService;
    }

    public async Task<User> CreateUser(UserModel userModel)
    {
        var user = new User
        {
            Name = userModel.Name,
            Email = userModel.Email,
            Role = UserRole.User
        };

        user.Password = _passwordService.HashPassword(user, userModel.Password);

        return await _repository.CreateAsync(user);
    }

    public async Task<User> CreateAdminUser(AdminCreateUserModel adminUserModel)
    {
        var user = new User
        {
            Name = adminUserModel.Name,
            Email = adminUserModel.Email,
            Role = UserRole.Adm
        };

        user.Password = _passwordService.HashPassword(user, adminUserModel.Password);

        return await _repository.CreateAsync(user);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var result = await _repository.GetPagedAsync(u => u.Email == email, 1, 1)
            .ContinueWith(task => task.Result.Items.FirstOrDefault());

        return result ?? throw new KeyNotFoundException($"User with email {email} not found.");
    }
}

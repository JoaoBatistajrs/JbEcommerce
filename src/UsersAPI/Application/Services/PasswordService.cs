using Microsoft.AspNetCore.Identity;
using UsersAPI.Domain.Entities;
using UsersAPI.Domain.Interfaces.Service;

namespace UsersAPI.Application.Services;

public class PasswordService : IPasswordService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
        return result == PasswordVerificationResult.Success;
    }
}

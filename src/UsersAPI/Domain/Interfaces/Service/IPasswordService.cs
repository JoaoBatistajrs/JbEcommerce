using UsersAPI.Domain.Entities;

namespace UsersAPI.Domain.Interfaces.Service;

public interface IPasswordService
{
    string HashPassword(User user, string password);
    bool VerifyPassword(User user, string password);
}

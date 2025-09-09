using UsersApi.Domain.Entities;

namespace UsersApi.Domain.Interfaces.Service;

public interface IPasswordService
{
    string HashPassword(User user, string password);
    bool VerifyPassword(User user, string password);
}

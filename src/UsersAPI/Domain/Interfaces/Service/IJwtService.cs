using UsersApi.Domain.Entities;

namespace UsersApi.Domain.Interfaces.Service;

public interface IJwtService
{
    string GenerateToken(User user);
}

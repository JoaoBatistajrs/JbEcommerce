using UsersAPI.Domain.Entities;

namespace UsersAPI.Domain.Interfaces.Service;

public interface IJwtService
{
    string GenerateToken(User user);
}

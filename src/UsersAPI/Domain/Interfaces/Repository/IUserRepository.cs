using System.Linq.Expressions;
using UsersAPI.Domain.Entities;

namespace UsersAPI.Domain.Interfaces.Repository;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<PagedResult<User>> GetPagedAsync(Expression<Func<User, bool>>? filter, int pageNumber, int pageSize);
}

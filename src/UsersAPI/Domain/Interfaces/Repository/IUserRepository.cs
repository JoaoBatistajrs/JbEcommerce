using System.Linq.Expressions;
using UsersApi.Domain.Entities;

namespace UsersApi.Domain.Interfaces.Repository;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<PagedResult<User>> GetPagedAsync(Expression<Func<User, bool>>? filter, int pageNumber, int pageSize);
}

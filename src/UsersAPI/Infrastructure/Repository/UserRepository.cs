using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UsersAPI.Domain.Entities;
using UsersAPI.Domain.Interfaces.Repository;
using UsersAPI.Infrastructure.DataBase;

namespace UsersAPI.Infrastructure.Repository;

public class UserRepository : IUserRepository
{

    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Set<User>().FindAsync(id);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Set<User>().Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<PagedResult<User>> GetPagedAsync(Expression<Func<User, bool>>? filter, int pageNumber, int pageSize)
    {
        var query = _context.Set<User>().AsQueryable();

        if (filter is not null)
            query = query.Where(filter);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<User>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}

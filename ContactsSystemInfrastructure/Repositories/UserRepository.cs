
using ContactsSystemApplication.Interfaces;
using ContactsSystemDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Repositories;

public sealed class UserRepository (ContactsDbContext context) : IUserRepository
{
   
    public async Task<User?> GetByIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Include(u => u.ProfileContact)
            .FirstOrDefaultAsync(
                u => u.UserId == userId,
                cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Include(u => u.ProfileContact)
            .FirstOrDefaultAsync(
                u => u.ProfileContact.Email == email,
                cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AnyAsync(
                u => u.ProfileContact.Email == email,
                cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Include(u => u.ProfileContact)
            .OrderBy(u => u.UserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountActiveAdminsAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.Users
            .CountAsync(
                u => u.IsActive && u.Role == "Admin",
                cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}

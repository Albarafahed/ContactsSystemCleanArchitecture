
using ContactsSystemApplication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ContactsSystemInfrastructure.Persistence;

public sealed class UnitOfWork (ContactsDbContext context) : IUnitOfWork
{
  
    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await context.Database.BeginTransactionAsync(
                cancellationToken);

        try
        {
            var result = await operation(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}

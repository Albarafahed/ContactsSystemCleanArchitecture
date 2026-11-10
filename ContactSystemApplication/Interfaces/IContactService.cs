
using ContactsSystemApplication.DTO;
using ContactsSystemApplication.Requests;

namespace ContactsSystemApplication.Interfaces;

public interface IContactService
{
    Task<IReadOnlyList<ContactDto>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ContactDto> GetByIdAsync(
        int contactId,
        CancellationToken cancellationToken = default);

    Task<ContactDto> CreateAsync(
        SaveContactRequest request,
        CancellationToken cancellationToken = default);

    Task<ContactDto> UpdateAsync(
        int contactId,
        SaveContactRequest request,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(
        int contactId,
        CancellationToken cancellationToken = default);
}

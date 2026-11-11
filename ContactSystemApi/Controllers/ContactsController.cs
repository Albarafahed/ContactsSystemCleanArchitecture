
using ContactsSystemApplication.DTO;
using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsSystemApi.Controllers;

[Authorize(Roles = "Admin,User")]
[Route("api/Contacts")]
[ApiController]
public class ContactsController(IContactService contactService)
    : ControllerBase
{
    [HttpGet("All", Name = "GetAllContacts")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContactDto>>>
        GetAllContactsAsync()
    {
        var contacts = await contactService.GetAllAsync();
        if(contacts is null || !contacts.Any())
            return Ok(new List<ContactDto>());

        return Ok(contacts);
    }

    [HttpGet("{id:int}", Name = "GetContactById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>>
        GetContactByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest($"Invalid contact ID: {id}.");

        var contact = await contactService.GetByIdAsync(id);

        if (contact is null)
            return NotFound($"Contact with ID {id} was not found.");

        return Ok(contact);
    }

    [HttpPost("", Name = "AddContact")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactDto>>
        AddContactAsync(SaveContactRequest contactRequest)
    {
        var contact = await contactService.CreateAsync(contactRequest);

        return CreatedAtRoute(
            "GetContactById",
            new { id = contact.ContactId },
            contact);
    }

    [HttpPut("{id:int}", Name = "UpdateContact")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>>
        UpdateContactAsync(
            int id,
            SaveContactRequest contactRequest)
    {
        if (id <= 0)
            return BadRequest($"Invalid contact ID: {id}.");

        var contact = await contactService.UpdateAsync(
            id, contactRequest);

        if (contact is null)
            return NotFound($"Contact with ID {id} was not found.");

        return Ok(contact);
    }

    [HttpDelete("{id:int}", Name = "DeleteContact")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteContactAsync(int id)
    {
        if (id <= 0)
            return BadRequest($"Invalid contact ID: {id}.");

        bool deleted = await contactService.DeleteAsync(id);

        if (!deleted)
            return NotFound($"Contact with ID {id} was not found.");

        return Ok(new
        {
            Message = $"Contact with ID {id} deleted successfully."
        });
    }

    [HttpGet("Exists/{id:int}", Name = "ContactExists")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> ContactExistsAsync(int id)
    {
        if (id <= 0)
            return BadRequest($"Invalid contact ID: {id}.");

        var contact = await contactService.GetByIdAsync(id);

        return Ok(contact is not null);
    }
}

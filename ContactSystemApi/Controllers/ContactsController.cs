using System.Security.Claims;
using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Contacts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsSystemApi.Controllers;

[Authorize]
[Route("api/contacts")]
[ApiController]
public sealed class ContactsController(IContactService contactService) : ControllerBase
{
    private string? OwnerScope => User.IsInRole("Admin")
        ? null
        : User.FindFirstValue(ClaimTypes.NameIdentifier);

    [HttpGet("all", Name = "GetAllContacts")]
    [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContactDto>>> GetAllContactsAsync() =>
        Ok(await contactService.GetAllContactsAsync(OwnerScope));

    [HttpGet("{id:int}", Name = "GetContactById")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>> GetContactByIdAsync(int id)
    {
        if (id <= 0) return BadRequest("Contact ID must be greater than zero.");
        var contact = await contactService.GetContactByIdAsync(id, OwnerScope);
        return contact is null ? NotFound($"Contact with ID {id} was not found.") : Ok(contact);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactDto>> AddContactAsync([FromBody] SaveContactRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Forbid();
        var contact = await contactService.SaveAsync(0, request, userId);
        if (contact is null) return Problem(statusCode: 500, title: "The contact could not be created.");
        return CreatedAtRoute("GetContactById", new { id = contact.ContactId }, contact);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>> UpdateContactAsync(int id, [FromBody] SaveContactRequest request)
    {
        if (id <= 0) return BadRequest("Contact ID must be greater than zero.");
        var contact = await contactService.SaveAsync(id, request, OwnerScope);
        return contact is null ? NotFound($"Contact with ID {id} was not found.") : Ok(contact);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContactAsync(int id)
    {
        if (id <= 0) return BadRequest("Contact ID must be greater than zero.");
        var deleted = await contactService.DeleteContactAsync(id, OwnerScope);
        return deleted ? NoContent() : NotFound($"Contact with ID {id} was not found.");
    }

    [HttpGet("exists/{id:int}", Name = "ContactExists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> ContactExistsAsync(int id)
    {
        if (id <= 0) return BadRequest("Contact ID must be greater than zero.");
        return Ok(await contactService.ExistsAsync(id, OwnerScope));
    }
}

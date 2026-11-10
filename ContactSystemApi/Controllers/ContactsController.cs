using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Contacts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ContactsSystemApi.Controllers;

[Route("api/contacts")]
[ApiController]
public sealed class ContactsController(IContactService contactService) : ControllerBase
{
    [HttpGet("all", Name = "GetAllContacts")]
    [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ContactDto>>> GetAllContactsAsync()
    {
        var contacts = await contactService.GetAllContactsAsync();
        return Ok(contacts);
    }

    [HttpGet("{id:int}", Name = "GetContactById")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>> GetContactByIdAsync(int id)
    {
        if (id <= 0)
            return BadRequest("Contact ID must be greater than zero.");

        var contact = await contactService.GetContactByIdAsync(id);
        return contact is null
            ? NotFound($"Contact with ID {id} was not found.")
            : Ok(contact);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactDto>> AddContactAsync([FromBody] SaveContactRequest contactRequest)
    {
        var contact = await contactService.SaveAsync(0, contactRequest);
        if (contact is null)
            return Problem(statusCode: StatusCodes.Status500InternalServerError, title: "The contact could not be created.");

        return CreatedAtRoute("GetContactById", new { id = contact.ContactId }, contact);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactDto>> UpdateContactAsync(
        int id,
        [FromBody] SaveContactRequest contactRequest)
    {
        if (id <= 0)
            return BadRequest("Contact ID must be greater than zero.");

        var contact = await contactService.SaveAsync(id, contactRequest);
        return contact is null
            ? NotFound($"Contact with ID {id} was not found.")
            : Ok(contact);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContactAsync(int id)
    {
        if (id <= 0)
            return BadRequest("Contact ID must be greater than zero.");

        var deleted = await contactService.DeleteContactAsync(id);
        return deleted ? NoContent() : NotFound($"Contact with ID {id} was not found.");
    }

    [HttpGet("exists/{id:int}", Name = "ContactExists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<bool>> ContactExistsAsync(int id)
    {
        if (id <= 0)
            return BadRequest("Contact ID must be greater than zero.");

        return Ok(await contactService.ExistsAsync(id));
    }
}

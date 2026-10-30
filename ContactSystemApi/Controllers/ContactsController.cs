using ContactsSystemApplication.Contacts.Dtos;
using ContactsSystemApplication.Contacts.Interfaces;
using ContactsSystemApplication.Contacts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ContactsSystemApi.Controllers
{
    [Route("api/Contacts")]
    [ApiController]
    public class ContactsController(IContactService contactService) : ControllerBase
    {
        [HttpGet("All", Name = "GetAllContacts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetAllContactsAsync()
        {
            var contacts = await contactService.GetAllContactsAsync();
            if(contacts.Count==0) return NotFound("No contacts found.");
            return Ok(contacts);
        }

        [HttpGet("{id:int}", Name = "GetContactById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactDto>> GetContactByIdAsync(int id)
        {

            if(id <= 0) return BadRequest($"Invalid contact with id {id}.");
            var contact = await contactService.GetContactByIdAsync(id);
            if (contact is null) return NotFound($"Contact with id {id} not found.");
            return Ok(contact);
        }

        [HttpPost("", Name = "AddContact")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactDto>> AddContactAsync(SaveContactRequest contactRequest)
        {
            var contact = await contactService.SaveAsync(0, contactRequest);

            if (contact is null)
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Failed to create contact.");

            return CreatedAtRoute("GetContactById", new { id = contact.ContactId }, contact);
        }

        [HttpPut("{id:int}", Name = "UpdateContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ContactDto>> UpdateContactAsync(int id, SaveContactRequest contactRequest)
        {
            if (id <= 0) return BadRequest($"Invalid contact with id {id}.");
            var contact = await contactService.SaveAsync(id, contactRequest);
            if (contact is null)
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: $"Failed to update contact with id {id}.");
            return Ok(contact);
        }
        
        [HttpDelete("{id:int}", Name = "DeleteContact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteContactAsync(int id)
        {
            if (id <= 0) return BadRequest($"Invalid contact with id {id}.");
            var deleted = await contactService.DeleteContactAsync(id);
            if (!deleted)
              return NotFound($"Contact with id {id} not found.");

            return Ok($"Contact with id {id} deleted.");
        }

        [HttpGet("Exists/{id:int}", Name = "ContactExists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> ContactExistsAsync(int id)
        {
            if (id <= 0) return BadRequest($"Invalid contact with id {id}.");
            var exists = await contactService.ExistsAsync(id);
            if(!exists) return NotFound(exists);
            return Ok(exists);
        }
    }
}

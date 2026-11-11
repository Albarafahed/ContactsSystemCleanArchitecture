
using ContactsSystemApplication.Interfaces;
using ContactsSystemApplication.Requests;
using ContactsSystemApplication.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactsSystemApi.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/Users")]
[ApiController]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserManagementDto>>>
        GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await userService.GetAllAsync(cancellationToken);

        return Ok(users);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserManagementDto>>
        GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest("Invalid user ID.");

        var user = await userService.GetByIdAsync(
            id, cancellationToken);

        if (user is null)
            return NotFound($"User with ID {id} was not found.");

        return Ok(user);
    }

    [HttpPut("{id:int}/Role")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeRoleAsync(
        int id,
        UpdateUserRoleRequest request,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest("Invalid user ID.");

        await userService.ChangeRoleAsync(
            id, request, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int}/Activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest("Invalid user ID.");

        await userService.ActivateAsync(id, cancellationToken);

        return NoContent();
    }

    [HttpPut("{id:int}/Deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateAsync(
        int id,
        CancellationToken cancellationToken)
    {
        if (id <= 0)
            return BadRequest("Invalid user ID.");

        await userService.DeactivateAsync(id, cancellationToken);

        return NoContent();
    }
}

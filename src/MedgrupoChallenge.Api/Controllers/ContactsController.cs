using MedgrupoChallenge.Application.DTOs;
using MedgrupoChallenge.Application.Interfaces;
using MedgrupoChallenge.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedgrupoChallenge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
    {
        try
        {
            var contact = await _contactService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = contact.Id },
                contact);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message 
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var contacts = await _contactService.GetAllAsync();

        return Ok(contacts);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contact = await _contactService.GetByIdAsync(id);

        if (contact is null)
            return NotFound(new
            {
                message = "Contact not found."
            });

        return Ok(contact);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactRequest request)
    {
        try
        {
            var contact = await _contactService.UpdateAsync(id, request);

            if (contact is null)
                return NotFound(new
                {
                    message = "Contact not found."
                });

            return Ok(contact);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPatch("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var wasDeactivated = await _contactService.DeactivateAsync(id);

        if (!wasDeactivated)
            return NotFound(new
            {
                message = "Contact not found."
            });

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var wasDeleted = await _contactService.DeleteAsync(id);

        if (!wasDeleted)
            return NotFound(new
            {
                message = "Contact not found."
            });

        return NoContent();
    }
}
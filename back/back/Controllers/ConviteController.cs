using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Services;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConviteController : ControllerBase
{
    private readonly IConviteService _service;

    public ConviteController(IConviteService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetConvites() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetConvite(int id)
    {
        var convite = _service.GetById(id);
        return convite == null ? NotFound() : Ok(convite);
    }

    [HttpPost]
    public IActionResult PostConvite(Convite convite)
    {
        var criado = _service.Create(convite);
        return CreatedAtAction(nameof(GetConvite), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public IActionResult PutConvite(int id, Convite convite)
    {
        if (id != convite.Id) return BadRequest();
        if (!_service.Update(id, convite)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteConvite(int id)
    {
        if (!_service.Delete(id)) return NotFound();
        return NoContent();
    }
}

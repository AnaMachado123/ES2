using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Services;

namespace BackendTesteESII.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly IClienteService _service;

    public ClienteController(IClienteService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetClientes() => Ok(_service.GetAll());

    [HttpGet("{id}")]
    public IActionResult GetCliente(int id)
    {
        var cliente = _service.GetById(id);
        return cliente == null ? NotFound() : Ok(cliente);
    }

    [HttpPost]
    public IActionResult PostCliente(Cliente cliente)
    {
        var criado = _service.Create(cliente);
        return CreatedAtAction(nameof(GetCliente), new { id = criado.Id }, criado);
    }

    [HttpPut("{id}")]
    public IActionResult PutCliente(int id, Cliente cliente)
    {
        if (id != cliente.Id) return BadRequest();
        if (!_service.Update(id, cliente)) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCliente(int id)
    {
        if (!_service.Delete(id)) return NotFound();
        return NoContent();
    }
}

using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly GestaoServicosClientesContext _context;

    public ClienteController(GestaoServicosClientesContext context)
    {
        _context = context;
    }

    // GET: api/cliente
    [HttpGet]
    public ActionResult<IEnumerable<Cliente>> GetClientes()
    {
        return Ok(_context.Clientes.ToList());
    }

    // GET: api/cliente/5
    [HttpGet("{id}")]
    public ActionResult<Cliente> GetCliente(int id)
    {
        var cliente = _context.Clientes.Find(id);

        if (cliente == null)
            return NotFound();

        return Ok(cliente);
    }

    // POST: api/cliente
    [HttpPost]
    public ActionResult<Cliente> PostCliente(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
    }

    // PUT: api/cliente/5
    [HttpPut("{id}")]
    public IActionResult PutCliente(int id, Cliente cliente)
    {
        if (id != cliente.Id)
            return BadRequest();

        var clienteExistente = _context.Clientes.Find(id);
        if (clienteExistente == null)
            return NotFound();

        clienteExistente.Nome = cliente.Nome;
        clienteExistente.Email = cliente.Email;
        clienteExistente.Telefone = cliente.Telefone;
        _context.SaveChanges();

        return NoContent();
    }

    // DELETE: api/cliente/5
    [HttpDelete("{id}")]
    public IActionResult DeleteCliente(int id)
    {
        var cliente = _context.Clientes.Find(id);
        if (cliente == null)
            return NotFound();

        _context.Clientes.Remove(cliente);
        _context.SaveChanges();

        return NoContent();
    }
}

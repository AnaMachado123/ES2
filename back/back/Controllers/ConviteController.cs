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
    public IActionResult PostConvite([FromBody] ConviteCreateDTO dto)
    {
        if (dto == null || dto.UtilizadorId <= 0 || dto.ProjetoId <= 0)
            return BadRequest("Dados inválidos.");

        var convite = new Convite
        {
            UtilizadorId = dto.UtilizadorId,
            ProjetoId = dto.ProjetoId,
            Estado = "Pendente"
        };

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

    [HttpPut("{id}/estado")]
    public IActionResult AtualizarEstado(int id, [FromBody] string novoEstado)
    {
        var convite = _service.GetById(id);
        if (convite == null) return NotFound();

        convite.Estado = novoEstado;
        _service.Update(id, convite);
        return Ok(convite);
    }

    [HttpGet("utilizador/{id}")]
    public IActionResult GetPorUtilizador(int id)
    {
        var convitesDTO = _service.GetDTOsByUtilizador(id);
        return Ok(convitesDTO);
    }



    [HttpPut("{id}/recusar")]
    public IActionResult RecusarConvite(int id)
    {
        var sucesso = _service.RecusarConvite(id);
        if (!sucesso)
            return BadRequest("Não foi possível recusar o convite.");

        return Ok("Convite recusado com sucesso.");
    }
    
    [HttpPut("{id}/aceitar")]
    public IActionResult AceitarConvite(int id)
    {
        var sucesso = _service.AceitarConvite(id);
        if (!sucesso)
            return BadRequest("Não foi possível aceitar o convite.");

        return Ok("Convite aceite com sucesso.");
    }



    

}

using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models;
using BackendTesteESII.Services;

namespace BackendTesteESII.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var token = _loginService.Autenticar(request);

        if (token == null)
            return Unauthorized("Credenciais inválidas.");

        return Ok(new { token });
    }
}

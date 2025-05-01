using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Models.DTOs;

namespace BackendTesteESII.Services
{
    public interface ILoginService
    {
        string? Autenticar(LoginRequest request);
    }
}

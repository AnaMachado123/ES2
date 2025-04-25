using Microsoft.AspNetCore.Mvc;
using BackendTesteESII.Data;
using BackendTesteESII.Models;
using BackendTesteESII.Models.DTOs;


namespace BackendTesteESII.Services
{
    public interface IEmailService
    {
        void EnviarEmail(string para, string assunto, string corpo);

    }

    
}



using System.Net;
using System.Net.Mail;

namespace BackendTesteESII.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void EnviarEmail(string para, string assunto, string corpo)
        {
            var smtp = new SmtpClient(_config["Email:Smtp"], 587)
            {
                Credentials = new NetworkCredential(_config["Email:Username"], _config["Email:Password"]),
                EnableSsl = true
            };

            var mensagem = new MailMessage
            {
                From = new MailAddress(_config["Email:From"]),
                Subject = assunto,
                Body = corpo
            };

            mensagem.To.Add(para);

            smtp.Send(mensagem);
        }
    }
}

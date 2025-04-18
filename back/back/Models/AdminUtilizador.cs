namespace BackendTesteESII.Models
{
    public class AdminUtilizador : Utilizador
    {
        public override void ExibirInformacoes()
        {
            Console.WriteLine($"Admin: {Nome} ({Email}) - {HorasDia} horas/dia");
        }
    }
}

namespace BackendTesteESII.Models
{
    public class RegularUtilizador : Utilizador
    {
        public override void ExibirInformacoes()
        {
            Console.WriteLine($"Regular: {Nome} ({Email}) - {HorasDia} horas/dia");
        }
    public override bool PodeGerirUtilizadores()
        {
            return false;
        }

    }
}

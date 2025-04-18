namespace BackendTesteESII.Models
{
    public static class UtilizadorFactory
    {
        public static Utilizador CriarUtilizador(string tipo)
        {
            if (tipo.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                return new AdminUtilizador();
            }
            else if (tipo.Equals("regular", StringComparison.OrdinalIgnoreCase))
            {
                return new RegularUtilizador();
            }

            throw new ArgumentException("Tipo de utilizador desconhecido.");
        }
    }
}

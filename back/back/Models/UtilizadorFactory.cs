namespace BackendTesteESII.Models
{
    public static class UtilizadorFactory
    {
        public static Utilizador Criar(Utilizador baseU)

    {
            if (baseU.IsAdmin)
        {
                return new AdminUtilizador
            {
                Id = baseU.Id,
                Nome = baseU.Nome,
                Email = baseU.Email,
                Password = baseU.Password,
                HorasDia = baseU.HorasDia,
                IsAdmin = true
            };
        }

            return new RegularUtilizador
            {
                Id = baseU.Id,
                Nome = baseU.Nome,
                Email = baseU.Email,
                Password = baseU.Password,
                HorasDia = baseU.HorasDia,
                IsAdmin = false
            };
        }
    }
}

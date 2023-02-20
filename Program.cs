using Microsoft.Extensions.DependencyInjection;

namespace migracao_rebranding
{
    static class Program
    {
        static void Main()
        {
            IServiceCollection services = new ServiceCollection();
            Startup startup = new();
            startup.ConfigureServices(services);

            var buttons = new Buttons(startup.Configuration, "buttons.sql");
            if (!buttons.QueryButtons())
            {
                throw new System.Exception("Falha ao gerar script para a tabela buttons");
            }

            var quemSomos = new QuemSomos(startup.Configuration, "quem_somos.sql");
            if (!quemSomos.QueryQuemSomos())
            {
                throw new System.Exception("Falha ao gerar script para a tabela quem_somos");
            }

            // Depois de migrado tudo devemos remover o identificador na tabela buttons. Ex.: [id]Nome do botão
        }
    }
}

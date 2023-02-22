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

            var buttons = new Buttons(startup.Configuration, "buttons");
            if (!buttons.Execute())
            {
                throw new System.Exception("Falha ao gerar script para a tabela buttons");
            }

            var quemSomos = new QuemSomos(startup.Configuration, "quem_somos");
            if (!quemSomos.Execute())
            {
                throw new System.Exception("Falha ao gerar script para a tabela quem_somos");
            }

            var quemSomosCidade = new QuemSomosCidade(startup.Configuration, "quem_somos_cidade");
            if (!quemSomosCidade.Execute())
            {
                throw new System.Exception("Falha ao gerar script para a tabela quem_somos_cidade");
            }

            // Depois de migrado tudo devemos remover o identificador na tabela buttons. Ex.: [id]Nome do botão
        }
    }
}

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

            var nossaAtuacao = new NossaAtuacao(startup.Configuration, "nossa_atuacao");
            if (!nossaAtuacao.Execute())
            {
                throw new System.Exception("Falha ao gerar script para a tabela nossa_atuacao");
            }

            var nossaAtuacaoCidade = new NossaAtuacaoCidade(startup.Configuration, "nossa_atuacao_cidade");
            if (!nossaAtuacaoCidade.Execute())
            {
                throw new System.Exception("Falha ao gerar script para a tabela nossa_atuacao_cidade");
            }

            var openingPages = new OpeningPages(startup.Configuration, "opening_pages");
            if (!openingPages.Execute())
            {
                throw new System.Exception("Falha ao gerar script para a tabela opening_pages");
            }

            var homePages = new HomePages(startup.Configuration, "home_pages");
            if (!homePages.Execute())
            {
                throw new System.Exception("Falha ao gerar script para a tabela home_pages");
            }

            // Depois de migrado tudo devemos remover o identificador na tabela buttons. Ex.: [id]Nome do botão
        }
    }
}

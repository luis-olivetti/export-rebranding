using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace migracao_rebranding
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.AddSingleton<IConfigurationRoot>(Configuration);
        }
    }
}
using Autofac;
using Microsoft.Extensions.Configuration;
using TicTacToe.Infrastructure.Automapper;
using TicTacToe.Infrastructure.IoC.Modules;

namespace TicTacToe.Infrastructure.IoC
{
    public class ContainerModule : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public ContainerModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<RepositoryModule>();
            builder.RegisterModule<ServiceModule>();
            builder.RegisterModule<CommandModule>();

            builder.RegisterInstance(AutomapperConfig.Initialize()).SingleInstance();
        }
    }
}
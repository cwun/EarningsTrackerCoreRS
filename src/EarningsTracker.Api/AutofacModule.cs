using Autofac;
using EarningsTracker.Services.Common;
using EarningsTracker.Services.Dashboards;
using EarningsTracker.Services.Settings;
using EarningsTracker.Services.SqlServer;

namespace EarningsTracker.Api
{
    public class AutofacModule : Module
    {
        private string _connectionString;

        public AutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Register Connection class and expose IConnection 
            // by passing in the Database connection information
            builder.RegisterType<Connection>() // concrete type
                .As<IConnection>() // abstraction
                .WithParameter("connectionString", _connectionString)
                .InstancePerLifetimeScope();

            // Register Repository class and expose IRepository
            builder.RegisterType<Repository>()
                .As<IRepository>()
                .InstancePerLifetimeScope();

            // Register DashboardService class and expose IDashboardService
            builder.RegisterType<DashboardService>()
                .As<IDashboardService>()
                .InstancePerLifetimeScope();

            // Register SettingService class and expose ISettingService
            builder.RegisterType<SettingService>()
                .As<ISettingService>()
                .InstancePerLifetimeScope();
        }
    }
}

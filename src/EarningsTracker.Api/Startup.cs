using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace EarningsTracker.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime after the Startup constructor. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Add the CORS services
            services.AddCors();

            services.AddCors(o => o.AddPolicy("AllowAllOrigins", p =>
            {
                p.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
            }));

            // Add framework services.
            services.AddMvc();

            // Create the Autofac container builder.
            var builder = new ContainerBuilder();

            // Obtain database connection string once and resuse by Connection class
            var connectionString = Configuration.GetValue<string>("DBConnection:ConnectionString");

            // Add any Autofac modules or registrations.
            builder.RegisterModule(new AutofacModule(connectionString));

            // Populate the services.
            builder.Populate(services);

            // Build the container.
            this.ApplicationContainer = builder.Build();

            // Create and return the service provider.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime after ConfigureServices. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseCors("AllowAllOrigins");

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}
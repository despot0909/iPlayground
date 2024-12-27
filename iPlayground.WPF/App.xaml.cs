using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using iPlayground.Core.Interfaces.Services;
using iPlayground.Core.Services;
using iPlayground.Core.Interfaces.Repositories;
using iPlayground.Data.Repositories;
using iPlayground.Data.Context;
using System.Windows;
using System.IO;
using iPlayground.WPF.ViewModels;
using iPlayground.Core.Models;
using iPlayground.WPF.Services.Interfaces;
using iPlayground.WPF.Services;
using System;
using iPlayground.WPF.Views;

namespace iPlayground.WPF
{
    public partial class App : Application
    {
        private IServiceProvider serviceProvider;

        private IConfiguration configuration;

        public App()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        private void ConfigureServices(ServiceCollection services)
        {
            // Configuration
            services.AddSingleton(configuration);
            services.AddHttpClient();


            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IChildRepository, ChildRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            // Services
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IParentService, ParentService>();
            services.AddScoped<IFiscalService, FiscalService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddSingleton<IPrinterService, PrinterService>();
            services.AddSingleton<USBScannerService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<ISessionVoucherRepository, SessionVoucherRepository>();


            // Views
            services.AddSingleton<MainWindow>();
            services.AddTransient<ActiveSessionsView>();
            services.AddTransient<InActiveSessionsView>();

            // ViewModels
            services.AddTransient<NewChildViewModel>();
            services.AddTransient<ActiveSessionsViewModel>();
        }

        private void ConfigureServices2(ServiceCollection services)
        {
            // Configuration
            services.AddSingleton(configuration);

            // DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));  // Dodajte ovu liniju
            services.AddScoped<IChildRepository, ChildRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            // Services
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IParentService, ParentService>();

            // Navigation
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<MainViewModel>();

            // ViewModels
           
            services.AddTransient<NewChildViewModel>();
            services.AddTransient<ActiveSessionsViewModel>();

            // Main Window
            services.AddTransient<MainWindow>();

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create and migrate the database
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
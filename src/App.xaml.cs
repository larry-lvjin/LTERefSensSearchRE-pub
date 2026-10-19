using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using LteRefSensTester.Services;
using LteRefSensTester.ViewModels;

namespace LteRefSensTester
{
    /// <summary>
    /// Application entry point with dependency injection
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Configure dependency injection
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            // Create and show main window
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register Services as Singletons
            services.AddSingleton<ICmwService, CmwService>();
            services.AddSingleton<ITestService, TestService>();
            services.AddSingleton<ILogService, LogService>();

            // Register ViewModels
            services.AddSingleton<MainViewModel>();

            // Register Views
            services.AddSingleton<MainWindow>(sp =>
            {
                var window = new MainWindow();
                var viewModel = sp.GetRequiredService<MainViewModel>();
                window.DataContext = viewModel;
                return window;
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Cleanup resources
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }
}

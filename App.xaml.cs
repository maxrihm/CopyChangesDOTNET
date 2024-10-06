using CopyChanges.Interfaces;
using CopyChanges.LineHandlers;
using CopyChanges.Services;
using CopyChanges.ViewModels;
using CopyChanges.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CopyChanges
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IGitService, GitService>();
            services.AddSingleton<IJsonService, JsonService>();
            services.AddSingleton<IClipboardService, ClipboardService>();

            services.AddSingleton<MainViewModel>();

            services.AddTransient<MainWindow>();
        }
    }
}



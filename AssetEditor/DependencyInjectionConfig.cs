﻿using AnimationEditor;
using AnimMetaEditor;
using AssetEditor.Services;
using AssetEditor.ViewModels;
using AssetEditor.Views;
using AssetEditor.Views.Settings;
using Common;
using Common.ApplicationSettings;
using Common.GameInformation;
using CommonControls;
using CommonControls.Editors.AnimationFragment;
using CommonControls.Editors.AnimationPack;
using CommonControls.Resources;
using CommonControls.Services;
using KitbasherEditor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TextEditor;
using View3D;

namespace AssetEditor
{
    class DependencyInjectionConfig
    {
        public IServiceProvider ServiceProvider { get; private set; }
        
        public DependencyInjectionConfig()
        {
            Logging.Configure(Serilog.Events.LogEventLevel.Information);
            DirectoryHelper.EnsureCreated();
            ResourceController.Load();
            GameInformationFactory.Create();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();
            RegisterTools(ServiceProvider.GetService<ToolFactory>());
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ApplicationSettingsService>();
            services.AddSingleton<ToolFactory>();
            services.AddSingleton<FileTypes.PackFiles.Models.PackFileDataBase>();
            services.AddSingleton<SkeletonAnimationLookUpHelper>();

            services.AddTransient<GameInformationService>();
            services.AddTransient<MainWindow>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsWindow>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<MenuBarViewModel>();
            services.AddTransient<PackFileService>();
            services.AddTransient<SchemaManager>();

            TextEditor_DependencyInjectionContainer.Register(services);
            KitbasherEditor_DependencyInjectionContainer.Register(services);
            View3D_DependencyInjectionContainer.Register(services);
            AnimMetaEditor_DependencyInjectionContainer.Register(services);
            AnimationEditors_DependencyInjectionContainer.Register(services);
            AnimationFragment_DependencyInjectionContainer.Register(services);
            AnimationPack_DependencyInjectionContainer.Register(services);
        }

        void RegisterTools(IToolFactory factory)
        {
            TextEditor_DependencyInjectionContainer.RegisterTools(factory);
            KitbasherEditor_DependencyInjectionContainer.RegisterTools(factory);
            View3D_DependencyInjectionContainer.RegisterTools(factory);
            AnimMetaEditor_DependencyInjectionContainer.RegisterTools(factory);
            AnimationEditors_DependencyInjectionContainer.RegisterTools(factory);
            AnimationFragment_DependencyInjectionContainer.RegisterTools(factory);
            AnimationPack_DependencyInjectionContainer.RegisterTools(factory);
        }

        public void ShowMainWindow()
        {
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();
        }
    }

}

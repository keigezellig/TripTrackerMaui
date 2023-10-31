﻿using CommunityToolkit.Maui.Maps;
using MaterialColorUtilities.Maui;
using MauiApp1.Helpers;
using MauiIcons.Material;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;
using MauiApp1.Services;
using MauiApp1.Services.MessageProcessing;
using MauiApp1.ViewModels;
using MetroLog.MicrosoftExtensions;
using MetroLog.Operators;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseFluentMauiIcons()
                .UseMaterialColors()
                .UseMauiMaps()
                .UseMauiCommunityToolkitMaps("FIbihTK8UUhD61pp7uFp~nRhZ8n_l6z6hwY8AqYjGdQ~AoK4JSlpppbAnwR2-rx_hi_FQwlbpsOb0-V1zMLsdShfawdzF7Upl7IIjUPCpyQN")
                .UseMaterialMauiIcons();


            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<DataPage>();
            builder.Services.AddTransient<DataViewModel>();
            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<MapViewModel>();

            builder.Services.AddSingleton<IMessageQueueProvider, MqttProvider>();
            builder.Services.AddSingleton<TripStartedMessageProcessor>();
            builder.Services.AddSingleton<TripStoppedMessageProcessor>();
            builder.Services.AddSingleton<TripResumedMessageProcessor>();
            builder.Services.AddSingleton<TripPausedMessageProcessor>();
            builder.Services.AddSingleton<FuelStopMessageProcessor>();
            builder.Services.AddSingleton<VehicleMessageProcessor>();
            builder.Services.AddSingleton<GnssMessageProcessor>();
            builder.Services.AddSingleton<UnknownMessageProcessor>();
            builder.Services.AddSingleton<ExternalMessageProcessorFactory>();
            
            builder.Services.AddTransient<IDataService, MessageQueueDataService>();
            
            builder.Logging
            .AddConsoleLogger(
                options =>
                {
                    options.MinLevel = LogLevel.Debug;
                    options.MaxLevel = LogLevel.Critical;
                })
            .AddTraceLogger(
                options =>
                {
                    options.MinLevel = LogLevel.Debug;
                    options.MaxLevel = LogLevel.Critical;
                })
            .AddInMemoryLogger(
                options =>
                {
                    options.MaxLines = 1024;
                    options.MinLevel = LogLevel.Debug;
                    options.MaxLevel = LogLevel.Critical;
                });
            
            builder.Services.AddSingleton(LogOperatorRetriever.Instance);

            var app = builder.Build();
            //ServiceHelper.Initialize(app.Services);

            return app;
        }
    }
}
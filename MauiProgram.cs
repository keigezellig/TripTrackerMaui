using CommunityToolkit.Maui;
using MaterialColorUtilities.Maui;
using MauiApp1.Controls.MarkerMap;
using MauiIcons.Material;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;
using MauiApp1.Services;
using MauiApp1.Services.DataService;
using MauiApp1.Services.MessageProcessing;
using MauiApp1.Services.MessageProcessing.MessageProcessors;
using MauiApp1.Services.SettingsService;
using MauiApp1.ViewModels;
using MauiApp1.Views;
using MetroLog.MicrosoftExtensions;
using MetroLog.Operators;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace MauiApp1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp(true)
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseFluentMauiIcons()
                .UseMaterialColors()
                .UseMaterialMauiIcons();
            


            builder.Services.AddScoped<MainPage>();
            builder.Services.AddScoped<MainViewModel>();
            builder.Services.AddScoped<DataPage>();
            builder.Services.AddScoped<LiveDataPage>();
            builder.Services.AddScoped<LiveDataViewModel>();
            builder.Services.AddScoped<MapPage>();
            builder.Services.AddScoped<MapViewModel>();
            builder.Services.AddScoped<SettingsPage>();
            builder.Services.AddScoped<SettingsViewModel>();
            

            builder.Services.AddSingleton<IMessageQueueProvider, MqttProvider>();
            builder.Services.AddSingleton<TripStartedMessageProcessor>();
            builder.Services.AddSingleton<TripStoppedMessageProcessor>();
            builder.Services.AddSingleton<TripResumedMessageProcessor>();
            builder.Services.AddSingleton<TripPausedMessageProcessor>();
            builder.Services.AddSingleton<FuelStopMessageProcessor>();
            builder.Services.AddSingleton<VehicleMessageProcessor>();
            builder.Services.AddSingleton<GnssMessageProcessor>();
            builder.Services.AddSingleton<UnknownMessageProcessor>();
            builder.Services.AddSingleton<MessageProcessorFactory>();
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            
            builder.Services.AddSingleton<IDataService, MessageQueueDataService>();
            
            builder.Logging
            .AddConsoleLogger(
                options =>
                {
                    options.MinLevel = LogLevel.Trace;
                    options.MaxLevel = LogLevel.Critical;
                })
            .AddTraceLogger(
                options =>
                {
                    options.MinLevel = LogLevel.Trace;
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
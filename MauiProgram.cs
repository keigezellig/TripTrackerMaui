using CommunityToolkit.Maui;

using MaterialColorUtilities.Maui;

using MauiIcons.Fluent;
using MauiIcons.Material;

using MetroLog.MicrosoftExtensions;
using MetroLog.Operators;

using Microsoft.Extensions.Logging;

using SkiaSharp.Views.Maui.Controls.Hosting;

using TripTracker.Services.DataService;
using TripTracker.Services.MessageProcessing;
using TripTracker.Services.MessageProcessing.MessageProcessors;
using TripTracker.Services.MessageQueue;
using TripTracker.Services.SettingsService;
using TripTracker.ViewModels;
using TripTracker.Views;

namespace TripTracker
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

            return app;
        }
    }
}
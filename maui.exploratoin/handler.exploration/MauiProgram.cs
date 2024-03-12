using handler.exploration.Controls;
using handler.exploration.Handlers;
using handler.exploration.Interfaces;
using Microsoft.Extensions.Logging;

namespace handler.exploration
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
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(ImageEntry), typeof(ImageEntryHandler))
                            .AddHandler(typeof(GLabel), typeof(GLabelHandler));
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            HandlerOnly.Customize();
            HandlerAndVirtualView.Customize();

            return app;
        }
    }
}

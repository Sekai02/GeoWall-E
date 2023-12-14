using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using UraniumUI;

namespace AppInterface;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        try 
        {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseUraniumUI()
            .UseUraniumUIMaterial() // 👈 Don't forget these two lines.
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

            // Register the MainPage as transient to make sure it can resolve the IFolderPicker dependency.
            builder.Services.AddSingleton<MainPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

        return builder.Build();
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(ex.StackTrace);
            return null;
        }
    }
}

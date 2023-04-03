using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using MudBlazorExtensionLibrary.Services;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using MeddyExplorerApp.Services;

namespace MeddyExplorerApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiApp<App>().UseMauiCommunityToolkitCore()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

        builder.Services.AddSingleton<IFolderPicker>(FolderPicker.Default);
        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddMudServices();
		builder.Services.AddScoped<MBELLayoutService>();
		builder.Services.AddScoped<MeddyExplorerState>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

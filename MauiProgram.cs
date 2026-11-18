namespace TabBarColorRepro;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder().UseMauiApp<App>();
#if ANDROID
        builder.ConfigureImageSources(sources =>
        {
            sources.AddService<DelayedTabIconSource, DelayedTabIconService>();
        });
#endif
        return builder.Build();
    }
}

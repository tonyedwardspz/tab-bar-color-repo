using System.Reflection;

namespace TabBarColorRepro;

public sealed class HomePage : ReproPage
{
    public HomePage() : base("Home") { }
}

public sealed class ActivityPage : ReproPage
{
    public ActivityPage() : base("Activity") { }
}

public sealed class SettingsPage : ReproPage
{
    public SettingsPage() : base("Settings") { }
}

public class ReproPage : ContentPage
{
    public ReproPage(string title)
    {
        Title = title;
        var orange = (Color)Application.Current!.Resources["TabOrange"];
        var navigate = new Button { Text = "Push a detail page", AutomationId = "PushDetail" };
        navigate.Clicked += async (_, _) =>
        {
#if TABBED_PAGE
            await Navigation.PushAsync(new DetailPage());
#else
            await Shell.Current.GoToAsync("detail");
#endif
        };

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Padding = new Thickness(24),
                Spacing = 20,
                Children =
                {
                    new Label { Text = "TAB ICON TINT", FontSize = 12, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#64748B") },
                    new Label { Text = "Orange on the first frame?", FontSize = 28, FontAttributes = FontAttributes.Bold },
                    new Label { Text = "Inspect the selected Home icon before touching the tab bar. Then switch tabs and return to Home." },
                    new Label { Text = $"Timing experiment: Home icon load delayed {ReproTiming.IconLoadDelayMs} ms on Android. Wait for it to appear before navigating.", FontSize = 14 },
                    new Border
                    {
                        BackgroundColor = Colors.White,
                        Stroke = Color.FromArgb("#E2E8F0"),
                        Padding = 18,
                        Content = new HorizontalStackLayout
                        {
                            Spacing = 14,
                            Children =
                            {
                                new BoxView { Color = orange, WidthRequest = 36, HeightRequest = 36, VerticalOptions = LayoutOptions.Center },
                                new VerticalStackLayout
                                {
                                    Children =
                                    {
                                        new Label { Text = "Expected selected tint", FontAttributes = FontAttributes.Bold },
                                        new Label { Text = "Orange · #F58220", FontSize = 14 }
                                    }
                                }
                            }
                        }
                    },
                    new Label { Text = "1. Cold launch: check the selected icon.\n2. Tap Activity, then Home: compare the tint.\n3. Push a detail page and go back: compare again.\n4. Force-stop and relaunch to repeat.", LineHeight = 1.35 },
                    navigate,
                    new Label { Text = "Reported regression: selected icon starts blue on Android, then becomes orange after navigation. A passing run stays orange throughout.", FontSize = 14 },
                    new Label { Text = BuildInfo.Description, FontSize = 12, TextColor = Color.FromArgb("#64748B"), AutomationId = "BuildInfo" }
                }
            }
        };
    }
}

internal static class ReproTiming
{
    public static int IconLoadDelayMs { get; } = int.TryParse(
        typeof(App).Assembly.GetCustomAttributes<AssemblyMetadataAttribute>()
            .FirstOrDefault(attribute => attribute.Key == "IconLoadDelayMs")?.Value,
        out var delay) ? Math.Max(0, delay) : 0;
}

public sealed class DetailPage : ContentPage
{
    public DetailPage()
    {
        Title = "Detail";
        var back = new Button { Text = "Go back", AutomationId = "GoBack" };
        back.Clicked += async (_, _) =>
        {
#if TABBED_PAGE
            await Navigation.PopAsync();
#else
            await Shell.Current.GoToAsync("..");
#endif
        };
        Content = new VerticalStackLayout
        {
            Padding = 24,
            Spacing = 20,
            Children =
            {
                new Label { Text = "Return and inspect the tab icon", FontSize = 24, FontAttributes = FontAttributes.Bold },
                new Label { Text = "The app has not changed any tab colors. Navigation alone may cause the native tint to update." },
                back
            }
        };
    }
}

internal static class BuildInfo
{
#if TABBED_PAGE
    private const string Host = "TabbedPage";
#else
    private const string Host = "Shell";
#endif
    public static string Description =>
        $"{Host} · {DeviceInfo.Platform} {DeviceInfo.VersionString}\n" +
        $".NET {Environment.Version} · MAUI {typeof(Shell).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0]}";
}

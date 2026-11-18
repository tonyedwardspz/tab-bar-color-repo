using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Application = Microsoft.Maui.Controls.Application;

namespace TabBarColorRepro;

public sealed class ReproTabbedPage : Microsoft.Maui.Controls.TabbedPage
{
    public ReproTabbedPage()
    {
        // Set colors once, before native controls are created. No tint workaround.
        SelectedTabColor = (Color)Application.Current!.Resources["TabOrange"];
        UnselectedTabColor = (Color)Application.Current.Resources["TabUnselected"];
        BarBackgroundColor = (Color)Application.Current.Resources["TabBackground"];
        On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

        AddTab(new HomePage(), "tab_home.png");
        AddTab(new ActivityPage(), "tab_activity.png");
        AddTab(new SettingsPage(), "tab_settings.png");
    }

    private void AddTab(Page page, string icon)
    {
        Children.Add(new NavigationPage(page)
        {
            Title = page.Title,
            IconImageSource = icon,
            BarBackgroundColor = Colors.White,
            BarTextColor = Color.FromArgb("#182230")
        });
    }
}

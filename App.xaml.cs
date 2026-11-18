namespace TabBarColorRepro;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
#if TABBED_PAGE
        return new Window(new ReproTabbedPage());
#else
        return new Window(new AppShell());
#endif
    }
}

namespace TabBarColorRepro;

// A separate registration keeps the timing experiment isolated to these tab icons.
public sealed class DelayedTabIconSource : ImageSource, IFileImageSource
{
    public static readonly BindableProperty FileProperty = BindableProperty.Create(
        nameof(File), typeof(string), typeof(DelayedTabIconSource), string.Empty,
        propertyChanged: (source, _, _) => ((DelayedTabIconSource)source).OnSourceChanged());

    public string File
    {
        get => (string)GetValue(FileProperty);
        set => SetValue(FileProperty, value);
    }

    public override bool IsEmpty => string.IsNullOrEmpty(File);
}

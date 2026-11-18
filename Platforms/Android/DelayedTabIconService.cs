using Android.Content;
using Android.Graphics.Drawables;

namespace TabBarColorRepro;

// Delay only the Home asset: other icons can finish before Shell applies appearance.
// This changes loading timing, never native tint or Shell appearance properties.
public sealed class DelayedTabIconService : FileImageSourceService, IImageSourceService<DelayedTabIconSource>
{
    public override async Task<IImageSourceServiceResult<Drawable>?> GetDrawableAsync(
        IImageSource imageSource, Context context, CancellationToken cancellationToken = default)
    {
        if (imageSource is IFileImageSource file &&
            file.File == "tab_home.png" && ReproTiming.IconLoadDelayMs > 0)
        {
            Android.Util.Log.Info("TabTintRepro", $"Home icon load delayed {ReproTiming.IconLoadDelayMs} ms");
            await Task.Delay(ReproTiming.IconLoadDelayMs, cancellationToken);
        }

        var result = await base.GetDrawableAsync(imageSource, context, cancellationToken);
        Android.Util.Log.Info("TabTintRepro", $"Icon ready: {(imageSource as IFileImageSource)?.File}");
        return result;
    }
}

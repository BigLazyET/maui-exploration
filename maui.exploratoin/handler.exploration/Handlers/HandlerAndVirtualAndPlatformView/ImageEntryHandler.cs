using handler.exploration.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using PlatformView = handler.exploration.Platforms.Windows.MauiImageEntry;

namespace handler.exploration.Handlers
{
    internal partial class ImageEntryHandler
    {
        public static IPropertyMapper<ImageEntry, ImageEntryHandler> PropertyMapper = new PropertyMapper<ImageEntry, ImageEntryHandler>(ViewHandler.ViewMapper)
        {
            [nameof(ImageEntry.Text)] = MapText,
            [nameof(ImageEntry.ImageSource)] = MapImageSource
        };

        public static CommandMapper<ImageEntry, ImageEntryHandler> CommandMapper = new(ViewCommandMapper);

        public ImageEntryHandler() : base(PropertyMapper, CommandMapper) { }
    }
}

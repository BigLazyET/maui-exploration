using handler.exploration.Controls;
using handler.exploration.Extensions;
using handler.exploration.Platforms.Windows;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace handler.exploration.Handlers
{
    internal partial class ImageEntryHandler : ViewHandler<ImageEntry, MauiImageEntry>
    {
        protected override MauiImageEntry CreatePlatformView()
        {
            return new MauiImageEntry(VirtualView);
        }

        protected override void ConnectHandler(MauiImageEntry platformView)
        {
            base.ConnectHandler(platformView);

            UpdateText(platformView);
        }

        protected override void DisconnectHandler(MauiImageEntry platformView)
        {
            base.DisconnectHandler(platformView);
        }

        public static void MapImageSource(ImageEntryHandler imageEntryHandler, ImageEntry imageEntry)
        {
            imageEntryHandler.PlatformView.UpdateSource().FireAndForget();
        }

        public static void MapText(ImageEntryHandler imageEntryHandler, ImageEntry imageEntry)
        {
            if (imageEntryHandler.PlatformView.TextBox.Text != imageEntry.Text)
                imageEntryHandler.PlatformView.UpdateText();
        }

        private void UpdateText(MauiImageEntry mauiImageEntry)
        {
            var text = VirtualView?.Text;
            mauiImageEntry.TextBox.Text = text;
        }
    }
}

using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;

namespace handler.exploration.Handlers
{
    internal class EmailEntryHandler : EntryHandler
    {
        private Brush defaultBrush;

        protected override void ConnectHandler(TextBox platformView)
        {
            base.ConnectHandler(platformView);

            platformView.LostFocus += OnLostFocus;
        }

        public override void SetVirtualView(IView view)
        {
            base.SetVirtualView(view);
        }

        private void OnLostFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            PlatformView.BorderBrush = Brush.Red.ToBrush();
        }
    }
}

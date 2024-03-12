using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;

namespace handler.exploration.Interfaces
{
    internal static class HandlerHelper
    {
        public static IPropertyMapper<IGestureAwareControl, IElementHandler> GesturesMapper = new PropertyMapper<IGestureAwareControl, IElementHandler>
        {
            ["MouseEntered"] = MapPropertyChanged,
            ["MouseEnteredCommand"] = MapPropertyChanged,
            ["MouseMoved"] = MapPropertyChanged,
            ["MouseMovedCommand"] = MapPropertyChanged,
            ["MouseExited"] = MapPropertyChanged,
            ["MouseExitedCommand"] = MapPropertyChanged,
        };

        private static void MapPropertyChanged(IElementHandler handler, IGestureAwareControl element)
        {
            TaskHelpers.Debounce(handler, element, delegate (IElementHandler handler, IGestureAwareControl element)
            {
                ViewHandler viewHandler = handler as ViewHandler;
                if (viewHandler != null)
                {
                    handler.UpdateValue("ContainerView");
                }
                else
                {
                    viewHandler = null;
                }

                WinUIGestureHandler.OnElementPropertyChanged(element, viewHandler?.ContainerView ?? ((FrameworkElement)handler.PlatformView));
            }, TimeSpan.FromMilliseconds(100.0));
        }
    }
}

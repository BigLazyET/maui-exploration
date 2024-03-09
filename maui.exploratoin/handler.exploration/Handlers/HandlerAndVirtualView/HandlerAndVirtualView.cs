using handler.exploration.Controls;
using Microsoft.Maui.Platform;

namespace handler.exploration.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    internal class HandlerAndVirtualView
    {
        internal static void Customize()
        {
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("fooentry-background", (handler, view) =>
            {
                if(view is FooEntry)
                {
                    handler.PlatformView.UpdateBackground(Colors.Yellow.AsPaint());
                }
            });
        }
    }
}

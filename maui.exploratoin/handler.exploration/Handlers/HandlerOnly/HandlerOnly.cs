using Microsoft.Maui.Platform;

namespace handler.exploration.Handlers
{
    /// <summary>
    /// 对全局某一类型的handler进行自定义，此操作会影响全局以此handler实现的控件
    /// 所有示例仅针对windows，其他平台(android, ios and etc..)同理
    /// 1. 需要了解IPropertyMapper的三个扩展方法：AppendToMapping, PrependToMapping, ModifyMapping
    /// 2. 还有一种无需针对全局，只针对单个控件自定义的方式，他是通过HandlerChanged和HandlerChanging的方式实现
    /// 3. 顺便可以了解一下handler的生命周期和组件的渲染过程
    /// 以上共同点是仅针对handler进行自定义即可，无需自定义组件
    /// </summary>
    internal class HandlerOnly
    {
        internal static void Customize()
        {
            // 针对全局的Entry，将全局的Entry的背景色改成红色
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("red-background", (handler, view) =>
            {
                var platformView = handler.PlatformView;
                if (platformView != null && platformView is Microsoft.UI.Xaml.Controls.TextBox textBox && textBox != null)
                {
                    textBox.UpdateBackground(Colors.Red.AsPaint());
                }
            });

            // 针对全局的Entry，在获得焦点的同时全选Text
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("text-selection", (handler, view) =>
            {
                var platformView = handler.PlatformView;
                if (platformView != null && platformView is Microsoft.UI.Xaml.Controls.TextBox textBox && textBox != null)
                {
                    textBox.GotFocus += (sender, args)=>
                    {
                        textBox.SelectAll();
                    };
                }
            });
        }
    }
}

## MAUI自定义控件（一）

### 一、MAUI控件自定义架构

![Button-Handler](https://learn.microsoft.com/zh-cn/dotnet/maui/user-interface/handlers/media/overview/button-handler.png?view=net-maui-8.0)

#### 1. 概念
* 在MAUI中，我们使用的跨平台控件（存在于Microsoft.Maui.Controls命名空间下）是各原生平台控件的虚拟视图（VirtualView）
* 各原生平台的控件，又称为本机视图（PlatformView）
* **大部分**的跨平台控件都有其对应的接口（存在于Microsoft.Maui）表现形式：其目的用于通过接口将handler与其跨平台控件解耦
* VirtualView与PlatformView之间的**映射渲染**由XF的Renderer过渡到了MAUI的Handler，因此每一个跨平台控件都会有其对应的Handler

#### 2. 概念衍生
* {Controls}.Handler: 我们可以直接访问跨平台控件的Handler，这得益于跨平台控件的基类（Microsoft.Maui.Controls.View/Element）
* {IHandler}.PlatformView：每个Handler通过PlatformView公开跨平台控件对应的本机视图
* {IHandler}.VirtualView：每个Handler通过VirtualView公开跨平台控件对应的虚拟视图

[官方提供的Handler一览](https://learn.microsoft.com/zh-cn/dotnet/maui/user-interface/handlers/?view=net-maui-8.0#view-handlers)

### 二、映射器

.NET MAUI 处理程序的关键概念是映射器。 每个处理程序通常都提供属性映射器，有时提供命令映射器，用于将跨平台控件的 API 映射到本机视图的 API

#### 1. 属性映射器

属性映射器定义在跨平台控件中发生属性更改时要执行的操作。 它是 Dictionary，用于将跨平台控件的属性映射到其关联的操作。 然后，每个处理程序都提供操作的实现，用于操作本机视图 API。 这可确保在跨平台控件上设置属性时，基础本机视图会根据需要进行更新

#### 2. 命令映射器

命令映射器定义跨平台控件向本机视图发送命令时要执行的操作。 它们类似于属性映射器，但允许传递额外的数据。 命令映射器是 Dictionary，用于将跨平台控件的命令映射到其关联的操作。 然后，每个处理程序都提供操作的实现，用于操作本机视图 API

#### 3. 映射器的优势
* 将本机视图PlatformView与跨平台控件VirtualView分离
* 不需要本机视图PlatformView订阅和取消订阅跨平台控制事件
* 轻松进行自定义，无需子类化即可修改映射器。

#### 4. 生命周期
每个处理程序Handler都有两种生命周期方法：
* 当即将为跨平台控件创建新处理程序以及即将从跨平台控件中删除现有处理程序时，将引发 HandlerChanging
* 当创建跨平台控件并准备使用时，会引发 HandlerChanged，意味着该默认处理程序的所有属性可以使用

> 另请注意，所有控件都有一个您可以订阅的 OnHandlerChanging 和 OnHandlerChanged 事件

> 在跨平台控件上，HandlerChanging 事件是先于 HandlerChanged 事件引发的

### 三、Handler VS Renderer

|映射渲染|属性变更|注册联系|按需实现| 解耦 |
|:------|------|:------------------|----------------|---|
|Renderer| 通过OnElementPropertyChanged来响应跨平台控件属性的更改，会进行全属性遍历 | 通过ExportRendererAttribute来声明跨平台控件和Renderer之间的联系，这就导致在应用启动时需要去所有的dll中扫描这个Attribute，即使有的dll跟XF没有任何关系 | Renderer是各平台的视图，继承了各平台的View（安卓的Android.Views.View和iOS的UIKit.UIView），每个布局都需要一个视图，增加了视图层级 | 本机视图与跨平台控件耦合 |
|Handler| 通过PropertyMapper来实现这一过程，直接采用字典查找而非全属性遍历 | 在MauiProgram.CreateMauiApp()方法中，利用MauiAppBuilder的重载方法ConfigureMauiHandlers去配置跨平台控件和Handler的联系 | Handler利用CreatePlatformView()方法来创建并返回平台视图PlatformView，例如，如果你有一个 Button 控件，它在 Android 上可能会被映射到 Android.Widget.Button，在 iOS 上被映射到 UIButton。Handlers 直接与这些原生控件交互，而不需要通过继承它们来创建自定义视图 | 使用映射器让本机视图PlatformView与跨平台控件VirtualView解耦；同时跨平台控件也可以通过接口与Handler实现解耦 |

### 四、概念以Entry为例源码来解析

#### 1. 跨平台控件Entry
```csharp
namespace Microsoft.Maui.Controls
{
	public partial class Entry : InputView, ITextAlignmentElement, IEntryController, IElementConfiguration<Entry>, IEntry
	{
		public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(Entry), ReturnType.Default);

		public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(nameof(ReturnCommand), typeof(ICommand), typeof(Entry), default(ICommand));

        ...其他代码...
    }
}
```
> 注意到 Entry定义了一些可绑定的属性：ReturnType， ReturnCommand等，这点跟XF并无差异

#### 2. Common EntryHandler
注意它标记为partial，因为它需要返回各个平台的PlatformView，自然Handler具有各个平台的实现，其实现将在每个平台上使用附加分部类完成：
```csharp
<!--EntryHandler.cs-->
#if __IOS__ || MACCATALYST
using PlatformView = Microsoft.Maui.Platform.MauiTextField;
#elif MONOANDROID
using PlatformView = AndroidX.AppCompat.Widget.AppCompatEditText;
#elif WINDOWS
using PlatformView = Microsoft.UI.Xaml.Controls.TextBox;
#elif TIZEN
using PlatformView = Tizen.UIExtensions.NUI.Entry;
#elif (NETSTANDARD || !PLATFORM) || (NET6_0_OR_GREATER && !IOS && !ANDROID && !TIZEN)
using PlatformView = System.Object;
#endif
public partial class EntryHandler : IEntryHandler
{
	public static IPropertyMapper<IEntry, IEntryHandler> Mapper = new PropertyMapper<IEntry, IEntryHandler>(ViewHandler.ViewMapper)
	{
		[nameof(IEntry.ReturnType)] = MapReturnType,
        ..其他属性映射..
	};

	public static CommandMapper<IEntry, IEntryHandler> CommandMapper = new(ViewCommandMapper)
	{
#if ANDROID
		[nameof(IEntry.Focus)] = MapFocus
#endif
	};

    public EntryHandler() : this(Mapper)
    {
    }

    public EntryHandler(IPropertyMapper? mapper)
	    : base(mapper ?? Mapper, CommandMapper)
    {
    }

    public EntryHandler(IPropertyMapper? mapper, CommandMapper? commandMapper)
	    : base(mapper ?? Mapper, commandMapper ?? CommandMapper)
    {
    }

	IEntry IEntryHandler.VirtualView => VirtualView;

	PlatformView IEntryHandler.PlatformView => PlatformView;

    ..省略代码..
}
```
注意到最上面的命名空间导入，会利用预编译指令来导入各个平台的本机组件，这就是handler所返回的PlatformView：
* 安卓：AppCompatEditText
* iOS: MauiTextField
* Windows：TextBox

在 Common EntryHandler 中
* 请注意如何拥有属性映射器和命令映射器，并要求构造函数中传入属性/命令映射器
* 属性映射器将在Entry中定义的ReturnType属性映射到MapReturnType方法，作用就是调用各平台本机组件的本机API，该 API 执行该属性所期望的操作；并在属性发生更改时，调用MapReturnType方法

> 注意，Entry中定义了几种绑定属性，并不意味着属性映射器就有几种属性映射；同理命令映射器也是如此

#### 3. Android EntryHandler
安卓平台 EntryHandler的具体实现
```csharp
<!--EntryHandler.Android.cs-->
public partial class EntryHandler : ViewHandler<IEntry, AppCompatEditText>
{
    public static void MapReturnType(IEntryHandler handler, IEntry entry) =>
	handler.PlatformView?.UpdateReturnType(entry);
    ..省略代码..
}
```
在 Android EntryHandler 中
* 看到MapReturnType的具体实现，其最终确实是调用了本机视图PlatfromView的UpdateReturnType方法

```csharp
<!--ViewHandler.cs-->
public abstract partial class ViewHandler<TVirtualView, TPlatformView> : ViewHandler, IViewHandler
		where TVirtualView : class, IView
		where TPlatformView : PlatformView
{
    protected abstract TPlatformView CreatePlatformView();

    protected virtual void ConnectHandler(TPlatformView platformView){}

    protected virtual void DisconnectHandler(TPlatformView platformView){}

    public virtual void SetVirtualView(IView view) =>
	    base.SetVirtualView(view);

    public sealed override void SetVirtualView(IElement view) =>
	    SetVirtualView((IView)view);

    ..省略代码..
}
```
观察一下抽象基类ViewHandler<TVirtualView, TPlatformView>
* 其中 IEntry 是代表 VirtualView 的接口，MauiAppCompatEditText 代表本机 PlatformView，它是为 Android 创建的 AppCompatEditText 的本机实现。

另外还有最重要的几个方法：
* CreatePlatformView() - 应创建并返回实现跨平台控件的本机视图PlatformView
* SetVirtualView() - 设置VirtualView，这个方法很重要，对整个handler的创建和生命周期的理解很有帮助，其最终可以追溯到基类ElementHandle的SetVirtualView方法
* ConnectHandler() - 用于执行任何本机视图设置，例如初始化本机视图和执行事件订阅，每次新处理程序连接到控件时都会调用 ConnectHandler
* DisconnectHandler() - 用于执行任何本机视图清理，例如取消订阅事件和释放对象，每次处理程序因从控件中删除或分配了新处理程序而断开连接时都应调用

> .NET MAUI 有意不调用 DisconnectHandler 方法。 相反，你必须从应用的生命周期中的合适位置自行调用

> 查看一下ElementHandle的SetVirtualView方法源码，可以得出处理程序的调用堆栈如下所示：SetVirtualView → CreatePlatformView -> ConnectHandler，并在需要时 DisconnectHandler。这也是为什么 ConnectHandler 是所有类型初始化的最佳位置的原因

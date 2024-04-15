## MAUI自定义控件（三）

微软官方提供了从page，layout到view林林总总50多个控件（也许后来会更多），但当我们需要做一些特定场景或者效果，又或者一些酷炫的动画的时候，官方控件大都不符合我们的需求，这个时候就需要我们去自定义控件。

幸运的是，上一篇我们说过，微软的MAUI带来的新的自定义控件架构，其利用全新的映射渲染角色Handler来处理VirtualView和PlatformView之间的联系，相比Xamarin.Forms的Renderer更加的轻量和灵活。

那么显而易见，我们自定义控件的重点就在于Handler上

然而，在此之前我们先认识和回顾下一些自定义控件必备的知识。



### 一、配置多平台

自定义控件涉及到针对多个平台的自定义，我们在新建一个Common Handler之外，还应该实现在各个平台的Handler；这一部分的设置可以参考官方的文档：[配置多平台](https://learn.microsoft.com/zh-cn/dotnet/maui/platform-integration/configure-multi-targeting?view=net-maui-8.0)

总结下来就三种方式：
* 利用条件编译，编写各个平台的预处理指令（#if Android/iOS等），
* 新建类，其存在于Platforms/ {platform}文件夹中
* 新建类，其后缀为.{platform}.cs的文件

> platfom代指平台，可以是 Android, MaciOS, iOS, MacCatalyst, Window等

上面的2和3并不能开箱即用，还需要进行额外的项目配置来告知编译器如何选择编译文件

下面以Android为例，其代表如果当前项目不是为 Android 生成，则
* 不要包含和编译文件名以 .Android.cs 结尾的 C# 代码
* 不要包含和编译位于 Android 文件夹或子文件夹中的.cs结尾的C#代码
```csharp
<!-- Android -->
<ItemGroup Condition="$(TargetFramework.StartsWith('net8.0-android')) != true">
  <Compile Remove="**\*.Android.cs" />
  <None Include="**\*.Android.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />
  <Compile Remove="**\Android\**\*.cs" />
  <None Include="**\Android\**\*.cs" Exclude="$(DefaultItemExcludes);$(DefaultExcludesInProjectFolder)" />  
</ItemGroup>
```

### 二、可用于定制的方法

#### 1. 映射器的重载

我们知道Handler中最重要的是两个映射器，而下面三种重载方法则提供了对映射器的定制和修改的方式

* PrependToMapping ，它在应用 .NET MAUI 控件映射之前修改处理程序的映射器
* ModifyMapping ，修改现有映射。 （这里需要提供正确的key）
* AppendToMapping ，它在应用 .NET MAUI 控件映射后修改处理程序的映射器

> 三种方法都具有两个参数；一般来说第一个参数是一个字符串键，通常基于属性或事件名称；当您尝试修改现有映射（ModifyMapping），那么您需要提供MAUI在其映射中使用的键；
> 如果您只想添加个人定制（PrependToMapping和AppendToMapping），您可以添加任何喜欢的东西作为键key。

#### 2. Handler的生命周期

其定义在上一篇博客中已经给出，我们需要关注的是每个跨平台控件还具有在 HandlerChanging 事件引发时调用的可替代 OnHandlerChanging 方法，以及在 HandlerChanged 事件引发时调用的 OnHandlerChanged 方法

所以我们又多了两个自定义的入口，即控件本身的：
* HandlerChanged
* HandlerChanging

### 三、自定义控件的划分

我们有很多种场景需要自定义控件，并且自定义控件也有深度和轻度的区别；我无法列举所有的情况

在这里，我只是提出自己的一个认知，官方对此并没有任何总结，但我认为这可以帮助我们在自定义控件的时候做出一些选择，避免"杀鸡用牛刀"，又或者避免"无从下手"的情况。

从控件维度划分：
* 针对某一类控件统一进行自定义
* 针对某一个控件进行自定义
* 现有控件完全不符合需求，完全新建一个控件

从代码维度划分：
* 只修改控件的handler
* 自定义控件 + 控件handler
* 自定义控件 + 控件handler + 自定义原生控件

下面我会给出四五种示例，这些示例有官方的也有参考其他博客的，为了方便，所有的例子都是基于Windows这一平台（其他平台基本同理），我会详细解释这些划分，并在最后做出总结。

### 四、自定义 - 全局定制

#### 1. 全局Handler定制

假定我们有这么一个需求：需要将项目中所有的官方Entry控件的背景色设置成红色
（这不是一个好的需求，只是为示例服务）

我们不可能去找到项目中所有用到Entry的地方逐一修改，而根据官方的控件自定义架构知道每一个控件都有其Handler对应，而Handler是处理映射渲染的关键角色，那么从Handler入手才是正确的选择

根据官方给出的一览表，Entry对用的Handler是EntryHandler

```csharp
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
    }
}
```

很简单：
* 我们只需要修改EntryHandler的Mapper
* 利用AppendToMapping扩展去实现这一功能

> 注意，所有基于EntryHandler实现的控件（包含自定义控件）都会被影响

在这里我把这一类修改统一都放在一起，最后在CreateMauiApp的时候去调用
```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        ..省略代码..

        HandlerOnly.Customize();

        return builder.Build();
    }
}
```

> 思考：如果我们自定义全局的ViewHandler的背景色，结果会如何？

#### 2. 全局控件定制

这里的控件可以包含官方控件和自定义控件，我们这里仅以自定义控件为例：

这里用户自定义控件只考虑很简单的**子类化控件**，比如：
```csharp
internal class FooEntry : Entry {}
```
同样的，我们继续1的需求，只不过把背景颜色改成黄色
```csharp
internal class HandlerAndVirtualView
{
    internal static void Customize()
    {
        // 针对全局的FooEntry，将全局的FooEntry的背景色改成黄色
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("fooentry-background", (handler, view) =>
        {
            if(view is FooEntry)
            {
                handler.PlatformView.UpdateBackground(Colors.Yellow.AsPaint());
            }
        });
    }
}
```

### 五、自定义 - Handler生命周期定制

这个很简单，我们使用控件自带的HandlerChanged和HandlerChanging方法来单独定制某一个控件

比如我们在某个页面中单独定制上面自定义的控件FooEntry，让其在获取焦点Focus的时候选中其Text内容

```xaml
<!--页面-->
<controls:FooEntry Text="foo entry handler lifecycle" HandlerChanged="FooEntry_HandlerChanged" HandlerChanging="FooEntry_HandlerChanging"/>
```

```csharp
// code-behind
private void FooEntry_HandlerChanged(object sender, EventArgs e)
{
	var fooEntry = sender as FooEntry;
    (fooEntry!.Handler!.PlatformView as TextBox)!.GotFocus += OnGotFocus;
}

private void FooEntry_HandlerChanging(object sender, HandlerChangingEventArgs e)
{
    if(e.OldHandler != null)
    {
        (e.OldHandler.PlatformView as TextBox)!.GotFocus -= OnGotFocus;
    }
}

void OnGotFocus(object sender, RoutedEventArgs e)
{
    var textBox = sender as TextBox;
    textBox!.Select(0, textBox.Text.Length - 1);
}
```

上述的自定义不会影响其他使用FooEntry的地方，只针对当前的FooEntry

### 六、下一篇前瞻

#### 1. 阶段性总结

由于另外几种定制的内容较多，所以就开新的一篇继续，针对当前已经罗列的定制方法从几个方面做一下总结

| 自定义分类 | 复杂性 | 实用性 | 覆盖面 | New Handler | New Native Control |
|:--------|:--------|:--------|:--------|:--------|:--------|
| 全局Handler定制 | 低 | 低 | 大 | 否 | 否 |
| 全局控件定制 | 低 | 低 | 大 | 否 | 否 |
| Handler生命周期定制 | 低 | 中 | 小 | 否 | 否 |

#### 2. 下一篇的自定义分类

* 自定义控件1：子类化控件（无内容）+ 自定义handler
* 自定义控件2：子类化控件（新增绑定属性）+ 自定义handler(不新增属性Mapper)
* 自定义控件2：子类化控件（新增绑定属性）+ 自定义handler(新增属性Mapper)
* 自定义控件3：完全新建控件（绑定属性）+ 自定义handler + 自定义native control

### 七、项目代码

项目代码：[maui-exploration
](https://github.com/BigLazyET/maui-exploration)
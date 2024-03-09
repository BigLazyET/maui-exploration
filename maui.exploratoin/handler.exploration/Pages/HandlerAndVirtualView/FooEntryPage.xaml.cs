using handler.exploration.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace handler.exploration.Pages;

public partial class FooEntryPage : ContentPage
{
	public FooEntryPage()
	{
		InitializeComponent();
	}

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
}
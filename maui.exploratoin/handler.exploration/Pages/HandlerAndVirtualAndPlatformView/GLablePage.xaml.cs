using Microsoft.Maui.Handlers;

namespace handler.exploration.Pages;

public partial class GLablePage : ContentPage
{
	public GLablePage()
	{
		InitializeComponent();
	}

    private void GLabel_MouseEntered(object sender, Interfaces.MouseEventArgs e)
    {
        label.Text += "GLabel Mouse Entered, ";
    }

    private void GLabel_MouseMoved(object sender, Interfaces.MouseEventArgs e)
    {
        label.Text += "GLabel Mouse Moved, ";
    }

    private void GLabel_MouseExited(object sender, Interfaces.MouseEventArgs e)
    {
        label.Text += "GLabel Mouse Exited, ";
    }
}
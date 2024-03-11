using handler.exploration.Controls;
using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Xaml.Controls;
using ColumnDefinition = Microsoft.UI.Xaml.Controls.ColumnDefinition;
using GridLength = Microsoft.UI.Xaml.GridLength;
using GridUnitType = Microsoft.UI.Xaml.GridUnitType;
using Grid = Microsoft.UI.Xaml.Controls.Grid;
using Microsoft.Maui.Controls.Compatibility.Platform.UWP;

namespace handler.exploration.Platforms.Windows
{
    internal class MauiImageEntry : Grid
    {
        private TextBox _textBox;
        private Microsoft.UI.Xaml.Controls.Image _image;

        private ImageEntry _imageEntry;

        public Microsoft.UI.Xaml.Controls.Image Image => _image;
        public TextBox TextBox => _textBox;

        public MauiImageEntry(ImageEntry imageEntry)
        {
            Width = 300;
            Height = 300;
            Background = Brush.Pink.ToBrush();
            _textBox = new TextBox { Background = Brush.Red.ToBrush(), Width = 200, Height = 50 };
            _image = new Microsoft.UI.Xaml.Controls.Image();
            _imageEntry = imageEntry;

            this.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Star) });
            this.ColumnDefinitions.Add( new ColumnDefinition { Width = new GridLength (50, GridUnitType.Star) });

            this.Children.Add( _textBox );
            this.Children.Add(_image);

            SetColumn(_textBox, 1);
            SetColumn(_image, 0);
        }

        public async Task UpdateSource()
        {
            _image.Source = await new FileImageSourceHandler().LoadImageAsync(_imageEntry.ImageSource);
        }

        public void UpdateText()
        {
            _textBox.Text = _imageEntry.Text;
        }
    }
}

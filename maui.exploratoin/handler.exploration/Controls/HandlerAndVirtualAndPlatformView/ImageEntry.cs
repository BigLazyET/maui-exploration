namespace handler.exploration.Controls
{
    internal class ImageEntry : View
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
               nameof(ImageSource),
               typeof(ImageSource),
               typeof(ImageEntry),
               default(ImageSource));

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
               nameof(Text),
               typeof(string),
               typeof(ImageEntry),
               default(string));



        /// <summary>
        /// An ImageSource that you want to add to your ViewPort
        /// </summary>
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}

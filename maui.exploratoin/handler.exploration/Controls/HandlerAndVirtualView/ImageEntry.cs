using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace handler.exploration.Controls
{
    internal class ImageEntry : Entry
    {
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
               nameof(Image),
               typeof(ImageSource),
               typeof(ImageEntry),
               default(ImageSource));

        public static readonly BindableProperty ImageHeightProperty = BindableProperty.Create(
               nameof(ImageHeight),
               typeof(int),
               typeof(ImageEntry),
               25);

        public static readonly BindableProperty ImageWidthProperty = BindableProperty.Create(
               nameof(ImageWidth),
               typeof(int),
               typeof(ImageEntry),
               25);

        public static readonly BindableProperty ImagePaddingProperty = BindableProperty.Create(
               nameof(ImagePadding),
               typeof(int),
               typeof(ImageEntry),
               5);

        public static readonly BindableProperty ImageCommandProperty = BindableProperty.Create(
              nameof(ImagePadding),
              typeof(ICommand),
              typeof(ImageEntry),
              default(ICommand));

        public static readonly BindableProperty ImageCommandParameterProperty = BindableProperty.Create(
              nameof(ImageCommandParameter),
              typeof(object),
              typeof(ImageEntry),
              default(object));

        /// <summary>
        /// Command parameter for your Image tap command
        /// </summary>
        public object ImageCommandParameter
        {
            get => GetValue(ImageCommandParameterProperty);
            set => SetValue(ImageCommandParameterProperty, value);
        }

        /// <summary>
        /// A command that you can use to bind with your Image that you added to your Entry's ViewPort
        /// </summary>
        public ICommand ImageCommand
        {
            get => (ICommand)GetValue(ImageCommandProperty);
            set => SetValue(ImageCommandProperty, value);
        }

        /// <summary>
        /// Padding of the Image that you added to the ViewPort
        /// </summary>
        public int ImagePadding
        {
            get => (int)GetValue(ImagePaddingProperty);
            set => SetValue(ImagePaddingProperty, value);
        }

        /// <summary>
        /// Width of the Image in your ViewPort
        /// </summary>
        public int ImageWidth
        {
            get => (int)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        /// <summary>
        /// Height of the Image in your ViewPort
        /// </summary>
        public int ImageHeight
        {
            get => (int)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        /// <summary>
        /// An ImageSource that you want to add to your ViewPort
        /// </summary>
        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }
    }
}

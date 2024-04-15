namespace handler.exploration.Controls
{
    internal class EmailEntry : Entry
    {
        public static readonly BindableProperty IsEmailProperty = BindableProperty.Create(
               nameof(IsEmail),
               typeof(bool),
               typeof(EmailEntry),
               false);

        public bool IsEmail
        {
            get => (bool)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
    }
}

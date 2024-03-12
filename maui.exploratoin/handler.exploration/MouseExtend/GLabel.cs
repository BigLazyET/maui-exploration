using System.Windows.Input;

namespace handler.exploration.Interfaces
{
    internal class GLabel : Microsoft.Maui.Controls.Label, IGestureAwareControl, IElement
    {
        private GestureHandler gestureHandler;

        //
        // 摘要:
        //     The BindableProperty for the MouseEnteredCommand.
        public static readonly BindableProperty MouseEnteredCommandProperty = GestureHandler.MouseEnteredCommandProperty;

        //
        // 摘要:
        //     The BindableProperty for the MouseEnteredCommandParameter.
        public static readonly BindableProperty MouseEnteredCommandParameterProperty = GestureHandler.MouseEnteredCommandParameterProperty;

        //
        // 摘要:
        //     The BindableProperty for the MouseMovedCommand.
        public static readonly BindableProperty MouseMovedCommandProperty = GestureHandler.MouseMovedCommandProperty;

        //
        // 摘要:
        //     The BindableProperty for the MouseMovedCommandParameter.
        public static readonly BindableProperty MouseMovedCommandParameterProperty = GestureHandler.MouseMovedCommandParameterProperty;

        //
        // 摘要:
        //     The BindableProperty for the MouseExitedCommand.
        public static readonly BindableProperty MouseExitedCommandProperty = GestureHandler.MouseExitedCommandProperty;

        //
        // 摘要:
        //     The BindableProperty for the MouseExitedCommandParameter.
        public static readonly BindableProperty MouseExitedCommandParameterProperty = GestureHandler.MouseExitedCommandParameterProperty;

        
        public GestureHandler GestureHandler => gestureHandler ?? (gestureHandler = new GestureHandler(this));

        //
        // 摘要:
        //     Gets or sets the command which is executed when the mouse pointer entered. This
        //     is a bindable property.
        public ICommand MouseEnteredCommand
        {
            get
            {
                return (ICommand)GetValue(GestureHandler.MouseEnteredCommandProperty);
            }
            set
            {
                SetValue(GestureHandler.MouseEnteredCommandProperty, value);
            }
        }

        //
        // 摘要:
        //     Gets or sets the parameter to pass to the MouseEnteredCommand. This is a bindable
        //     property.
        public object MouseEnteredCommandParameter
        {
            get
            {
                return GetValue(GestureHandler.MouseEnteredCommandParameterProperty);
            }
            set
            {
                SetValue(GestureHandler.MouseEnteredCommandParameterProperty, value);
            }
        }

        //
        // 摘要:
        //     Gets or sets the command which is executed when the mouse pointer moved. This
        //     is a bindable property.
        public ICommand MouseMovedCommand
        {
            get
            {
                return (ICommand)GetValue(GestureHandler.MouseMovedCommandProperty);
            }
            set
            {
                SetValue(GestureHandler.MouseMovedCommandProperty, value);
            }
        }

        //
        // 摘要:
        //     Gets or sets the parameter to pass to the MouseMovedCommand. This is a bindable
        //     property.
        public object MouseMovedCommandParameter
        {
            get
            {
                return GetValue(GestureHandler.MouseMovedCommandParameterProperty);
            }
            set
            {
                SetValue(GestureHandler.MouseMovedCommandParameterProperty, value);
            }
        }

        //
        // 摘要:
        //     Gets or sets the command which is executed when the mouse pointer exited. This
        //     is a bindable property.
        public ICommand MouseExitedCommand
        {
            get
            {
                return (ICommand)GetValue(GestureHandler.MouseExitedCommandProperty);
            }
            set
            {
                SetValue(GestureHandler.MouseExitedCommandProperty, value);
            }
        }

        //
        // 摘要:
        //     Gets or sets the parameter to pass to the MouseExitedCommand. This is a bindable
        //     property.
        public object MouseExitedCommandParameter
        {
            get
            {
                return GetValue(GestureHandler.MouseExitedCommandParameterProperty);
            }
            set
            {
                SetValue(GestureHandler.MouseExitedCommandParameterProperty, value);
            }
        }

        //
        // 摘要:
        //     The event which is raised when the mouse pointer entered the area over the view.
        public event EventHandler<MouseEventArgs> MouseEntered
        {
            add
            {
                GestureHandler.MouseEntered += value;
                OnPropertyChanged("MouseEntered");
            }
            remove
            {
                GestureHandler.MouseEntered -= value;
                OnPropertyChanged("MouseEntered");
            }
        }

        //
        // 摘要:
        //     The event which is raised when the mouse pointer moved over the view.
        public event EventHandler<MouseEventArgs> MouseMoved
        {
            add
            {
                GestureHandler.MouseMoved += value;
                OnPropertyChanged("MouseMoved");
            }
            remove
            {
                GestureHandler.MouseMoved -= value;
                OnPropertyChanged("MouseMoved");
            }
        }

        //
        // 摘要:
        //     The event which is raised when the mouse pointer was moved away from the view.
        public event EventHandler<MouseEventArgs> MouseExited
        {
            add
            {
                GestureHandler.MouseExited += value;
                OnPropertyChanged("MouseExited");
            }
            remove
            {
                GestureHandler.MouseExited -= value;
                OnPropertyChanged("MouseExited");
            }
        }
    }
}

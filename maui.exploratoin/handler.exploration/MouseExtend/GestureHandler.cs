using System.Windows.Input;
using Windows.Devices.Input;

namespace handler.exploration.Interfaces
{
    public class GestureHandler : IGestureListener
    {
        public static readonly string[] AllProperties = new string[9]
        {
            "MouseEntered",
            "MouseEnteredCommand",
            "MouseEnteredCommandParameter",
            "MouseMoved",
            "MouseMovedCommand",
            "MouseMovedCommandParameter",
            "MouseExited",
            "MouseExitedCommand",
            "MouseExitedCommandParameter"
        };

        public static readonly BindableProperty MouseEnteredCommandProperty = BindableProperty.Create("MouseEnteredCommand", typeof(ICommand), typeof(GestureHandler));

        public static readonly BindableProperty MouseEnteredCommandParameterProperty = BindableProperty.Create("MouseEnteredCommandParameter", typeof(object), typeof(GestureHandler));

        public static readonly BindableProperty MouseMovedCommandProperty = BindableProperty.Create("MouseMovedCommand", typeof(ICommand), typeof(GestureHandler));

        public static readonly BindableProperty MouseMovedCommandParameterProperty = BindableProperty.Create("MouseMovedCommandParameter", typeof(object), typeof(GestureHandler));

        public static readonly BindableProperty MouseExitedCommandProperty = BindableProperty.Create("MouseExitedCommand", typeof(ICommand), typeof(GestureHandler));

        public static readonly BindableProperty MouseExitedCommandParameterProperty = BindableProperty.Create("MouseExitedCommandParameter", typeof(object), typeof(GestureHandler));

        public event EventHandler<MouseEventArgs> MouseEntered;

        public event EventHandler<MouseEventArgs> MouseMoved;

        public event EventHandler<MouseEventArgs> MouseExited;

        private IGestureAwareControl Element;

        public GestureHandler(IGestureAwareControl element)
        {
            Element = element;
        }

        public bool HandlesMouseEntered
        {
            get
            {
                if (this.MouseEntered == null)
                {
                    return Element.MouseEnteredCommand != null;
                }

                return true;
            }
        }

        public bool HandlesMouseMoved
        {
            get
            {
                if (this.MouseMoved == null)
                {
                    return Element.MouseMovedCommand != null;
                }

                return true;
            }
        }

        public bool HandlesMouseExited
        {
            get
            {
                if (this.MouseExited == null)
                {
                    return Element.MouseExitedCommand != null;
                }

                return true;
            }
        }

        public bool OnMouseEntered(MouseEventArgs args)
        {
            bool result = false;
            if (HandlesMouseEntered)
            {
                //if (!Settings.IsLicenseValid)
                //{
                //    args = new MouseEventArgs();
                //}

                RaiseEvent(this.MouseEntered, args);
                ICommand command = null;
                object parameter = null;
                try
                {
                    command = Element.MouseEnteredCommand;
                    parameter = Element.MouseEnteredCommandParameter;
                }
                catch (Exception)
                {
                }

                ExecuteCommand(command, parameter, args);
                result = args.Handled;
            }

            return result;
        }

        public bool OnMouseMoved(MouseEventArgs args)
        {
            bool result = false;
            if (HandlesMouseMoved)
            {
                //if (!Settings.IsLicenseValid)
                //{
                //    args = new MouseEventArgs();
                //}

                RaiseEvent(this.MouseMoved, args);
                ICommand command = null;
                object parameter = null;
                try
                {
                    command = Element.MouseMovedCommand;
                    parameter = Element.MouseMovedCommandParameter;
                }
                catch (Exception)
                {
                }

                ExecuteCommand(command, parameter, args);
                result = args.Handled;
            }

            return result;
        }

        public bool OnMouseExited(MouseEventArgs args)
        {
            bool result = false;
            if (HandlesMouseExited)
            {
                //if (!Settings.IsLicenseValid)
                //{
                //    args = new MouseEventArgs();
                //}

                RaiseEvent(this.MouseExited, args);
                ICommand command = null;
                object parameter = null;
                try
                {
                    command = Element.MouseExitedCommand;
                    parameter = Element.MouseExitedCommandParameter;
                }
                catch (Exception)
                {
                }

                ExecuteCommand(command, parameter, args);
                result = args.Handled;
            }

            return result;
        }

        private void RaiseEvent<T>(EventHandler<T> handler, T args) where T : BaseGestureEventArgs
        {
            args.Sender = Element;
            handler?.Invoke(Element, args);
        }

        private void ExecuteCommand(ICommand command, object parameter, BaseGestureEventArgs args)
        {
            if (command != null)
            {
                args.Sender = Element;
                parameter = parameter ?? args;
                if (command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
        }
    }
}

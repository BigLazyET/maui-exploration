using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Input;

namespace handler.exploration.Interfaces
{
    public interface IGestureAwareControl : IElement
    {
        //
        // 摘要:
        //     The object which handles all the gestures.
        GestureHandler GestureHandler { get; }

        //
        // 摘要:
        //     Gets or sets the command which is executed when the mouse entered the elements
        //     area. This is a bindable property.
        ICommand MouseEnteredCommand { get; set; }

        //
        // 摘要:
        //     Gets or sets the parameter to pass to the MouseEnteredCommand. This is a bindable
        //     property.
        object MouseEnteredCommandParameter { get; set; }

        //
        // 摘要:
        //     Gets or sets the command which is executed when the mouse moved over the elements
        //     area. This is a bindable property.
        ICommand MouseMovedCommand { get; set; }

        //
        // 摘要:
        //     Gets or sets the parameter to pass to the MouseMovedCommand. This is a bindable
        //     property.
        object MouseMovedCommandParameter { get; set; }

        //
        // 摘要:
        //     Gets or sets the command which is executed when the mouse moved off the elements
        //     area. This is a bindable property.
        ICommand MouseExitedCommand { get; set; }

        //
        // 摘要:
        //     Gets or sets the parameter to pass to the MouseExitedCommand. This is a bindable
        //     property.
        object MouseExitedCommandParameter { get; set; }

        //
        // 摘要:
        //     The event which is raised when the mouse pointer enters the area of an element.
        event EventHandler<MouseEventArgs> MouseEntered;

        //
        // 摘要:
        //     The event which is raised when the mouse is moved around over an element.
        event EventHandler<MouseEventArgs> MouseMoved;

        //
        // 摘要:
        //     The event which is raised when the mouse is moved away from an element.
        event EventHandler<MouseEventArgs> MouseExited;
    }
}

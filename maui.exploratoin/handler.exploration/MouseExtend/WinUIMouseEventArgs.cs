using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace handler.exploration.Interfaces
{
    public class WinUIMouseEventArgs : MouseEventArgs
    {
        public WinUIMouseEventArgs(FrameworkElement view, PointerPoint currentPointer, MouseEventArgs previous, ulong prevTimestamp)
        {
            ViewPosition = WinUIEventArgsHelper.GetViewPosition(view);
            Touches = WinUIEventArgsHelper.GetTouches(currentPointer, view);
            base.Sources = WinUIEventArgsHelper.GetSources(currentPointer);
            CalculateDistances(previous);
            Velocity = GetVelocity(currentPointer, previous, prevTimestamp);
        }

        private Point GetVelocity(PointerPoint currentPointer, BaseGestureEventArgs prevArgs, ulong prevTimestamp)
        {
            if (prevArgs == null)
            {
                return Point.Zero;
            }

            ulong timestamp = currentPointer.Timestamp;
            ulong num = ((timestamp > prevTimestamp) ? (timestamp - prevTimestamp) : (prevTimestamp - timestamp));
            if (num != 0)
            {
                return new Point(DeltaDistance.X * 1000.0 / (double)num, DeltaDistance.Y * 1000.0 / (double)num);
            }

            return (prevArgs as PanEventArgs)?.Velocity ?? Point.Zero;
        }
    }
}

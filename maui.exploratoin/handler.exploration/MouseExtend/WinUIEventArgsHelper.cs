using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Input;

namespace handler.exploration.Interfaces
{
    public static class WinUIEventArgsHelper
    {
        public static Microsoft.Maui.Graphics.Rect GetViewPosition(FrameworkElement view)
        {
            Windows.Foundation.Point point = view.TransformToVisual(view.GetRoot()).TransformPoint(new Windows.Foundation.Point(0f, 0f));
            return new Microsoft.Maui.Graphics.Rect(point.X, point.Y, view.ActualWidth, view.ActualHeight);
        }

        public static Microsoft.Maui.Graphics.Point[] GetTouches(List<PointerPoint> currentPointers, FrameworkElement view, BaseGestureEventArgs previous = null, int minimumTouches = 1)
        {
            if (currentPointers.Count < minimumTouches && previous != null)
            {
                return previous.Touches;
            }

            GeneralTransform transform = view.GetRoot().TransformToVisual(view);
            return currentPointers.Select((PointerPoint p) => ToXFPoint(transform.TransformPoint(p.Position))).ToArray();
        }

        public static TouchSource[] GetSources(List<PointerPoint> currentPointers, BaseGestureEventArgs previous = null, int minimumTouches = 1)
        {
            if (currentPointers.Count < minimumTouches && previous != null)
            {
                return previous.Sources;
            }

            return currentPointers.Select((PointerPoint p) => ToTouchSource(p)).ToArray();
        }

        public static Microsoft.Maui.Graphics.Point[] GetTouches(PointerPoint point, FrameworkElement view)
        {
            GeneralTransform generalTransform = view.GetRoot().TransformToVisual(view);
            return new Microsoft.Maui.Graphics.Point[1] { ToXFPoint(generalTransform.TransformPoint(point.Position)) };
        }

        public static TouchSource[] GetSources(PointerPoint point)
        {
            return new TouchSource[1] { ToTouchSource(point) };
        }

        public static Microsoft.Maui.Graphics.Point ToXFPoint(Windows.Foundation.Point winPoint)
        {
            return new Microsoft.Maui.Graphics.Point(winPoint.X, winPoint.Y);
        }

        private static TouchSource ToTouchSource(PointerPoint point)
        {
            switch (point.PointerDeviceType)
            {
                case PointerDeviceType.Touch:
                    return TouchSource.Touchscreen;
                case PointerDeviceType.Mouse:
                    {
                        TouchSource touchSource = ((point.Properties.IsLeftButtonPressed || point.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased) ? TouchSource.LeftMouseButton : TouchSource.Unknown) | ((point.Properties.IsRightButtonPressed || point.Properties.PointerUpdateKind == PointerUpdateKind.RightButtonReleased) ? TouchSource.RightMouseButton : TouchSource.Unknown) | ((point.Properties.IsMiddleButtonPressed || point.Properties.PointerUpdateKind == PointerUpdateKind.MiddleButtonReleased) ? TouchSource.MiddleMouseButton : TouchSource.Unknown) | ((point.Properties.IsXButton1Pressed || point.Properties.PointerUpdateKind == PointerUpdateKind.XButton1Released) ? TouchSource.XButton1 : TouchSource.Unknown) | ((point.Properties.IsXButton2Pressed || point.Properties.PointerUpdateKind == PointerUpdateKind.XButton2Released) ? TouchSource.XButton2 : TouchSource.Unknown);
                        if (touchSource == TouchSource.Unknown)
                        {
                            return TouchSource.MousePointer;
                        }

                        return touchSource;
                    }
                case PointerDeviceType.Pen:
                    return TouchSource.Pen;
                default:
                    return TouchSource.Unknown;
            }
        }
    }
}

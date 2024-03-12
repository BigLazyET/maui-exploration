using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;

namespace handler.exploration.Interfaces
{
    public class WinUIGestureHandler : IDisposable
    {
        private static Dictionary<IGestureAwareControl, WinUIGestureHandler> allGestureHandlers = new Dictionary<IGestureAwareControl, WinUIGestureHandler>();

        private IGestureAwareControl element;

        private FrameworkElement view;

        private IGestureListener listener;

        private FrameworkElement root;

        private Microsoft.Maui.Controls.Page parentPage;

        private MultiPage<Microsoft.Maui.Controls.Page> multiPage;

        private bool isVisible = true;

        private PointerEventHandler _pointerMovedHandler;

        private PointerEventHandler _pointerExitedHandler;

        private PointerPoint currentPoint;

        private bool isHover;

        private ulong pointerExitedTimestamp;

        private MouseEventArgs lastMouseArgs;

        private ulong prevMouseTimestamp;

        private bool IsVisible
        {
            get
            {
                if (!isVisible)
                {
                    return false;
                }

                DependencyObject parent = view;
                while (parent != null)
                {
                    if (parent is UIElement uIElement && uIElement.Visibility != 0)
                    {
                        return false;
                    }

                    parent = VisualTreeHelper.GetParent(parent);
                }

                return true;
            }
        }

        public static void AddGestureRecognizers(IGestureAwareControl element, FrameworkElement view)
        {
            RemoveInstance(element);
            WinUIGestureHandler winUIGestureHandler = new WinUIGestureHandler(element, view);
            allGestureHandlers[element] = winUIGestureHandler;
        }

        public static void RemoveInstance(IGestureAwareControl element)
        {
            if (element != null && allGestureHandlers.Remove(element, out var value))
            {
                value.Dispose();
            }
        }

        public static void OnElementChanged(IGestureAwareControl oldElement, IGestureAwareControl newElement, FrameworkElement view)
        {
            if (oldElement != null)
            {
                RemoveInstance(oldElement);
            }

            if (newElement != null)
            {
                AddGestureRecognizers(newElement, view);
            }
        }

        public static void OnElementPropertyChanged(IGestureAwareControl element, FrameworkElement view)
        {
            if (element != null)
            {
                AddGestureRecognizers(element, view);
            }
        }

        private WinUIGestureHandler(IGestureAwareControl element, FrameworkElement view, IGestureListener listener = null)
        {
            this.element = element;
            this.view = view;
            this.listener = listener ?? new GestureThrottler(new GestureFilter(element, element.GestureHandler));

            parentPage = element.FindParent<Microsoft.Maui.Controls.Page>();
            if (parentPage != null)
            {
                parentPage.Appearing += Appearing;
                parentPage.Disappearing += Disappearing;
            }

            multiPage = element.FindParent<MultiPage<Microsoft.Maui.Controls.Page>>();
            if (multiPage != null)
            {
                multiPage.CurrentPageChanged += MultiPage_CurrentPageChanged;
            }

            if (view.IsLoaded)
            {
                View_Loaded(this, null);
            }
            else
            {
                view.Loaded += View_Loaded;
            }

            if (element is VisualElement visualElement)
            {
                visualElement.HandlerChanging += Element_HandlerChanging;
            }
        }

        private void View_Loaded(object sender, RoutedEventArgs e)
        {
            view.Loaded -= View_Loaded;
            _pointerMovedHandler = Window_PointerMoved;
            _pointerExitedHandler = Window_PointerExited;
            root = view.GetRoot();
            root.AddHandler(UIElement.PointerEnteredEvent, _pointerMovedHandler, handledEventsToo: true);
            root.AddHandler(UIElement.PointerMovedEvent, _pointerMovedHandler, handledEventsToo: true);
            root.AddHandler(UIElement.PointerExitedEvent, _pointerExitedHandler, handledEventsToo: true);
        }

        private void Element_HandlerChanging(object sender, HandlerChangingEventArgs e)
        {
            if (e.NewHandler == null)
            {
                RemoveInstance(element);
            }
        }

        private void Appearing(object sender, System.EventArgs e)
        {
            isVisible = true;
        }

        private void Disappearing(object sender, System.EventArgs e)
        {
            isVisible = false;
        }

        private void MultiPage_CurrentPageChanged(object sender, System.EventArgs e)
        {
            isVisible = element.HasParent(multiPage.CurrentPage);
        }

        private void Window_PointerMoved(object sender, PointerRoutedEventArgs ev)
        {
            if (!IsVisible)
            {
                return;
            }

            currentPoint = ev.GetCurrentPoint(null);
            if (currentPoint.Timestamp == pointerExitedTimestamp)
            {
                return;
            }

            bool flag = IsOverView(currentPoint.Position);
            if (flag != isHover)
            {
                if (ev.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    if (flag)
                    {
                        OnMouseEntered();
                    }
                    else
                    {
                        OnMouseExited();
                    }
                }

                isHover = flag;
            }

            if (isHover && ev.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                OnMouseMoved();
            }
        }

        private void Window_PointerExited(object sender, PointerRoutedEventArgs ev)
        {
            currentPoint = ev.GetCurrentPoint(null);
            pointerExitedTimestamp = currentPoint.Timestamp;
            if (isHover)
            {
                if (ev.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                {
                    OnMouseExited();
                }

                isHover = false;
            }
        }

        private bool OnMouseEntered()
        {
            bool result = false;
            WinUIMouseEventArgs args = new WinUIMouseEventArgs(view, currentPoint, null, currentPoint.Timestamp);
            if (element.GestureHandler.HandlesMouseEntered)
            {
                result = listener.OnMouseEntered(args);
            }

            lastMouseArgs = args;
            prevMouseTimestamp = currentPoint.Timestamp;
            return result;
        }

        private bool OnMouseMoved()
        {
            bool result = false;
            WinUIMouseEventArgs args = new WinUIMouseEventArgs(view, currentPoint, lastMouseArgs, prevMouseTimestamp);
            if (element.GestureHandler.HandlesMouseMoved)
            {
                result = listener.OnMouseMoved(args);
            }

            lastMouseArgs = args;
            prevMouseTimestamp = currentPoint.Timestamp;
            return result;
        }

        private bool OnMouseExited()
        {
            bool result = false;
            WinUIMouseEventArgs args = new WinUIMouseEventArgs(view, currentPoint, lastMouseArgs, prevMouseTimestamp);
            if (element.GestureHandler.HandlesMouseExited)
            {
                result = listener.OnMouseExited(args);
            }

            lastMouseArgs = null;
            prevMouseTimestamp = 0uL;
            return result;
        }

        private bool IsOverView(Windows.Foundation.Point point)
        {
            UIElement[] array = null;
            try
            {
                array = VisualTreeHelper.FindElementsInHostCoordinates(point, view, includeAllElements: true)?.ToArray();
            }
            catch
            {
            }

            if (array != null)
            {
                return array.Length != 0;
            }

            return false;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (element is Cell cell)
                {
                    cell.Appearing -= Appearing;
                    cell.Disappearing -= Disappearing;
                }

                if (parentPage != null)
                {
                    parentPage.Appearing -= Appearing;
                    parentPage.Disappearing -= Disappearing;
                    parentPage = null;
                }

                if (multiPage != null)
                {
                    multiPage.CurrentPageChanged -= MultiPage_CurrentPageChanged;
                    multiPage = null;
                }

                if (root != null)
                {
                    root.RemoveHandler(UIElement.PointerEnteredEvent, _pointerMovedHandler);
                    root.RemoveHandler(UIElement.PointerMovedEvent, _pointerMovedHandler);
                    root.RemoveHandler(UIElement.PointerCaptureLostEvent, _pointerExitedHandler);
                    root.RemoveHandler(UIElement.PointerCanceledEvent, _pointerExitedHandler);
                    root.RemoveHandler(UIElement.PointerExitedEvent, _pointerExitedHandler);
                    root = null;
                    _pointerMovedHandler = null;
                    _pointerExitedHandler = null;
                }

                if (element is VisualElement visualElement)
                {
                    visualElement.HandlerChanging -= Element_HandlerChanging;
                }

                element = null;
                view = null;
                listener = null;
            }
        }
    }
}

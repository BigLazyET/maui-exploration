namespace handler.exploration.Interfaces
{
    public class GestureThrottler : IGestureListener
    {
        private IGestureListener listener;

        private MouseEventArgs lastMouseArgs;

        public GestureThrottler(IGestureListener nextListener)
        {
            listener = nextListener;
        }

        public bool OnMouseEntered(MouseEventArgs args)
        {
            lastMouseArgs = args;
            return listener.OnMouseEntered(args);
        }

        public bool OnMouseMoved(MouseEventArgs args)
        {
            bool result = false;
            if (lastMouseArgs != null)
            {
                MouseEventArgs mouseEventArgs = args.Diff(lastMouseArgs);
                args = ((!(Math.Abs(mouseEventArgs.DeltaDistance.X) > 2.0) && !(Math.Abs(mouseEventArgs.DeltaDistance.Y) > 2.0)) ? null : mouseEventArgs);
            }

            if (args != null)
            {
                result = listener.OnMouseMoved(args);
                lastMouseArgs = args;
            }

            return result;
        }

        public bool OnMouseExited(MouseEventArgs args)
        {
            if (lastMouseArgs != null)
            {
                args = args.Diff(lastMouseArgs);
                lastMouseArgs = null;
            }

            return listener.OnMouseExited(args);
        }
    }
}

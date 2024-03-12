namespace handler.exploration.Interfaces
{
    /// <summary>
    /// A simple filter so that events are only raised if Element.InputTransparent == false
    /// </summary>
    internal class GestureFilter : IGestureListener
    {
        private VisualElement element;

        private IGestureListener listener;

        private bool RaiseEvent
        {
            get
            {
                if (element != null)
                {
                    return !element.InputTransparent;
                }

                return true;
            }
        }

        public GestureFilter(IGestureAwareControl element, IGestureListener nextListener)
        {
            this.element = element as VisualElement;
            listener = nextListener;
        }

        public bool OnMouseEntered(MouseEventArgs args)
        {
            if (!RaiseEvent)
            {
                return false;
            }

            return listener.OnMouseEntered(args);
        }

        public bool OnMouseMoved(MouseEventArgs args)
        {
            if (!RaiseEvent)
            {
                return false;
            }

            return listener.OnMouseMoved(args);
        }

        public bool OnMouseExited(MouseEventArgs args)
        {
            if (!RaiseEvent)
            {
                return false;
            }

            return listener.OnMouseExited(args);
        }
    }
}

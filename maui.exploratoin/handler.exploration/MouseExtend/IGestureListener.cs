namespace handler.exploration.Interfaces
{
    public interface IGestureListener
    {
        bool OnMouseEntered(MouseEventArgs args);

        bool OnMouseMoved(MouseEventArgs args);

        bool OnMouseExited(MouseEventArgs args);
    }
}

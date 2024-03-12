namespace handler.exploration.Interfaces
{
    [Flags]
    public enum TouchSource
    {
        Unknown = 0,
        Touchscreen = 1,
        LeftMouseButton = 2,
        RightMouseButton = 4,
        MiddleMouseButton = 8,
        Touchpad = 0x10,
        XButton1 = 0x20,
        XButton2 = 0x40,
        Pen = 0x80,
        MousePointer = 0x100
    }
}

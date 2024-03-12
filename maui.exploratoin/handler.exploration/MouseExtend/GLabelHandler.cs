namespace handler.exploration.Interfaces
{
    internal class GLabelHandler : Microsoft.Maui.Handlers.LabelHandler
    {
        public new static PropertyMapper<IGestureAwareControl, IElementHandler> Mapper = new PropertyMapper<IGestureAwareControl, IElementHandler>(HandlerHelper.GesturesMapper, Microsoft.Maui.Handlers.LabelHandler.Mapper);

        public GLabelHandler()
            : base(Mapper)
        {
        }

        public GLabelHandler(IPropertyMapper mapper = null)
            : base(mapper ?? Mapper)
        {
        }
    }
}

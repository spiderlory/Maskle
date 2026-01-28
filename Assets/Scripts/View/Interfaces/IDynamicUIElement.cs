namespace View.Interfaces
{
    public interface IDynamicUIElement : IUIElement
    {
        public void Activate();
        public void Deactivate();
    }
}
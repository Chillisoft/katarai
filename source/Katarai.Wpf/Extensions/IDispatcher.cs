namespace Katarai.Wpf.Extensions
{
    public interface IDispatcher
    {
        void Invoke(System.Action action);
    }
}
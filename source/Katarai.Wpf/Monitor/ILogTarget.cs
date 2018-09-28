using System;

namespace Katarai.Wpf.Monitor
{
    public interface ILogTarget : IDisposable
    {
        void LogInfo(string message);
        void LogError(string message);
    }
}
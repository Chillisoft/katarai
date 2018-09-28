using System.Diagnostics;

namespace Katarai.Wpf.Utils
{
    public interface ITraceLoggerHelper
    {
        void LogToUi(string message);
    }

    public class TraceLoggerHelper : ITraceLoggerHelper
    {
        public void LogToUi(string message)
        {
            var enumerator = Trace.Listeners.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var myTraceLogger = enumerator.Current as ListBoxTraceLogger;
                if (myTraceLogger != null)
                {
                    myTraceLogger.WriteLine(message);
                }
            }
        }
    }
}

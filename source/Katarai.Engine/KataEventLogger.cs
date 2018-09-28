using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public interface IKataEventLogger
    {
        void LogEvent(IKataEvent kataEvent);
    }

    public interface IKataEvent
    {
        string Message { get; }
    }

    public class KataEventLogger : IKataEventLogger
    {
        public KataEventLogger()
        {
            
        }

        public void LogEvent(IKataEvent kataEvent)
        {
            
        }
    }
}

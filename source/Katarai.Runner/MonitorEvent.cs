using System;

namespace Katarai.Runner
{
    public class MonitorEvent
    {
        public string Description { get; set; }
        public DateTime Logged { get; set; }
        public KataraiSettings Settings { get; set; }
    }

    public class CommandEvent
    {
        public string Description { get; set; }
        public DateTime Logged { get; set; }
    }
}
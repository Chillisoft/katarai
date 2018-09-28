using System;
using System.Collections.Generic;

namespace Katarai.Runner
{
    [Serializable]
    public class KataraiSettings
    {
        public string KataPath { get; set; }

        public string KataHash { get; set; }

        public string PlayerPath { get; set; }

        public string PlayerHash { get; set; }

        public bool ShowHint { get; set; }

        public bool IsAlwaysOnTop { get; set; }
        public int NotificationVisibility { get; set; }
        public List<KataReminder> Reminders { get; set; }
    }

    [Serializable]
    public class KataReminder
    {
        public bool ShowReminder { get; set; }
        public string Day { get; set; }
        public DateTime Time { get; set; }
    }
}

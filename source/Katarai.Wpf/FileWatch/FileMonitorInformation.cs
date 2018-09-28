namespace Katarai.Wpf.FileWatch
{
    public class FileMonitorInformation
    {
        public string OldKataHash { get; set; }

        public string KataHash { get; set; }

        public string OldPlayerHash { get; set; }

        public string PlayerHash { get; set; }

        public bool AreThereChanges()
        {
            if (string.IsNullOrEmpty(OldKataHash) || string.IsNullOrEmpty(OldPlayerHash))
            {
                return true;
            }

            return (OldKataHash != KataHash || OldPlayerHash != PlayerHash);
        }
    }
}

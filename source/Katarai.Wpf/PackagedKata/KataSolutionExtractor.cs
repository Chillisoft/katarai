using Ionic.Zip;

namespace Katarai.Wpf.PackagedKata
{
    public interface IKataSolutionExtractor
    {
        bool ExtractKataTo(string outputLocation, KataName kataName);
    }

    //TODO mark 09 Feb 2015: Review - This has not tests! We need to get this tested
    public class KataSolutionExtractor : IKataSolutionExtractor
    {
        private readonly string _kataPackageExtension;
        private readonly string _kataraiWpfPackagedkataPackages;

        public KataSolutionExtractor()
        {
            _kataPackageExtension = ".zip";
            _kataraiWpfPackagedkataPackages = "Katarai.Wpf.PackagedKata.Packages.";
        }

        public bool ExtractKataTo(string outputLocation, KataName kataName)
        {
            
            var fileName = kataName + _kataPackageExtension;
           
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(_kataraiWpfPackagedkataPackages + fileName))
            {
                if (stream == null) return false;

                using (var zip = ZipFile.Read(stream))
                {
                    zip.ExtractAll(outputLocation);
                }
            }

            return true;
        }
    }
}

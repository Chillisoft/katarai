using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Katarai.Runner;
using Katarai.Wpf.AnalysisContainers.Delegates;
using Katarai.Wpf.Settings;

namespace Katarai.Wpf.AnalysisContainers
{
    public class SandboxedAnalysisContainer : MarshalByRefObject, IAnalysisContainer
    {
        public string ExecuteShadowLocation { get; private set; }

        private const string ShadowDirectory = "_shadow";
        private readonly string _shadowLocation = Path.Combine(Path.GetTempPath(), ShadowDirectory);
        


        public Result Execute(KataraiSettings settings)
        {
            AppDomain domain = null;

            try
            {
                var domainId = Guid.NewGuid();
                ExecuteShadowLocation = GenerateShadowPath(domainId);
                domain = AppDomain.CreateDomain("New App Domain: " + domainId);

                var sandboxDelegate = (SandboxDelegate) domain.CreateInstanceAndUnwrap(
                    typeof (SandboxDelegate).Assembly.FullName,
                    typeof (SandboxDelegate).FullName);

                var result = sandboxDelegate.ExecuteInDelegate(settings, ExecuteShadowLocation);

                return result;
            }
            finally
            {
                if (domain != null)
                {
                    AppDomain.Unload(domain);
                }
                CleanupShadowStaging(ExecuteShadowLocation);
            }
        }

        private static void CleanupShadowStaging(string shadowLoacation)
        {
            if (!string.IsNullOrEmpty(shadowLoacation) && Directory.Exists(shadowLoacation))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                Directory.Delete(shadowLoacation, true);
            }
        }

        private string GenerateShadowPath(Guid domainId)
        {
            if (!Directory.Exists(_shadowLocation))
            {
                Directory.CreateDirectory(_shadowLocation);
            }

            var shadowDirectory = Path.Combine(_shadowLocation, domainId.ToString());
            if (!Directory.Exists(shadowDirectory))
            {
                Directory.CreateDirectory(shadowDirectory);
            }
            return shadowDirectory;
        }
    }
}

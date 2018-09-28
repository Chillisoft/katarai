using System.Collections.Specialized;
using System.Configuration;
using System.Security.Cryptography;
using Katarai.Utils;
using Katarai.Wpf.Properties;

namespace Katarai.Wpf
{
    public interface ISplunkSettings
    {
        string Server { get; }
        int Port { get; }
        string Username { get; }
        string Password { get; }
    }

    public class SplunkSettings : ISplunkSettings
    {
        public const string DEFAULT_SERVER = "localhost";

        public string Server { get; private set; }
        public int Port { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }

        public SplunkSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            Server = appSettings["server"] ?? DEFAULT_SERVER;
            Port = TryGetConfiguredSplunkPortFrom(appSettings);
            Username = appSettings["username"] ?? "admin";
            Password = TryGetPasswordFrom(appSettings);
        }

        private string TryGetPasswordFrom(NameValueCollection appSettings)
        {
            var password = appSettings["password"];
            if (string.IsNullOrEmpty(password)) return "admin";
            var rsa = RSA.Create();
            //rsa.FromXmlString(Resources.PrivateKey);  TODO: this would need a new private key if Splunk is posted to
            var rsaPasswordCrypter = new RSAPasswordCrypter(rsa);
            password = rsaPasswordCrypter.DecryptString(password);
            return password;
        }

        private static int TryGetConfiguredSplunkPortFrom(NameValueCollection appSettings)
        {
            int port;
            if (!int.TryParse(appSettings["port"], out port))
                port = 80;
            return port;
        }
    }
}
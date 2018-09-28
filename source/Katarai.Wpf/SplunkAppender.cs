using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Katarai.Utils;
using Katarai.Wpf.Properties;
using Splunk.Client;

namespace Katarai.Wpf
{
    public interface ISplunkAppender
    {
        void Log(string jsonObject, string indexName);
    }

    public class SplunkAppender : ISplunkAppender
    {
        private ISplunkSettings _settings;

        static SplunkAppender()
        {
            //// TODO: Use WebRequestHandler.ServerCertificateValidationCallback instead
            //// 1. Instantiate a WebRequestHandler
            //// 2. Set its ServerCertificateValidationCallback
            //// 3. Instantiate a Splunk.Client.Context with the WebRequestHandler
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
        }

        public SplunkAppender(ISplunkSettings settings)
        {
            _settings = settings;
        }

        public void Log(string jsonObject, string indexName)
        {
            // Disabled due to no Splunk endpoint in place
//            Task.Factory.StartNew(() =>
//            {
//                using (var service = new Service(Scheme.Https, _settings.Server, _settings.Port, new Namespace("nobody", "search")))
//                {
//                    Run(service, jsonObject, indexName).Wait();
//                }
//                
//            });
        }

        private async Task Run(Service service, string jsonObject, string indexName)
        {
            try
            {
                await service.LogOnAsync(_settings.Username, _settings.Password);

                Index index = await service.Indexes.GetOrNullAsync(indexName) ??
                              await service.Indexes.CreateAsync(indexName);

                await index.EnableAsync();

                ITransmitter transmitter = service.Transmitter;

                await transmitter.SendAsync(jsonObject, indexName);

            }
            catch (Exception exception)
            {
                Console.Write(exception.Message);
            }
        }

    }
}
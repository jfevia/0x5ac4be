using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using FountainBlue.Host.Console.Properties;
using FountainBlue.Service;
using FountainBlue.Service.Core;
using log4net;

namespace FountainBlue.Host.Console
{
    internal class Program
    {
        private static ILog _log;

        private static void Main(string[] _)
        {
            try
            {
                _log = LogManager.GetLogger(typeof(Program));

                LogStatus("Starting...");

                var serviceSettings = new ServiceSettings
                {
                    ScriptsDirectory = Settings.Default.ScriptsDirectory
                };

                var baseAddress = new Uri(Settings.Default.ServiceAddress);
                var service = new DataService(serviceSettings);
                var serviceHost = new ServiceHost(service, baseAddress);
                serviceHost.AddServiceEndpoint(typeof(IDataService), new WSDualHttpBinding(WSDualHttpSecurityMode.None), Settings.Default.ServiceName);

                var serviceMetadataBehavior = new ServiceMetadataBehavior
                {
                    HttpGetEnabled = true
                };

                serviceHost.Description.Behaviors.Add(serviceMetadataBehavior);
                serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), $"{baseAddress}mex");

                serviceHost.Open();

                LogStatus("Service running...");
                LogStatus("Press 'Enter' to exit this console...");
                System.Console.ReadLine();

                serviceHost.Close();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        /// <summary>
        ///     Logs the status.
        /// </summary>
        /// <param name="message">The message.</param>
        private static void LogStatus(string message)
        {
            System.Console.WriteLine($"{DateTime.Now} >> {message}");
        }
    }
}
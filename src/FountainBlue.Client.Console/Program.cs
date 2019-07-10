using System;
using System.ServiceModel;
using FountainBlue.Client.Console.Properties;
using FountainBlue.Service.Provider;
using log4net;

namespace FountainBlue.Client.Console
{
    internal class Program
    {
        private static ContractCallback _contractCallback;
        private static InstanceContext _instanceContext;
        private static WSDualHttpBinding _dualHttpBinding;
        private static EndpointAddress _endpointAddress;
        private static DataServiceClient _serviceClient;
        private static ILog _log;

        private static void Main(string[] _)
        {
            try
            {
                _log = LogManager.GetLogger(typeof(Program));

                LogStatus("Starting...");

                _contractCallback = new ContractCallback();
                _contractCallback.ExecutingScript += OnExecutingScript;
                _instanceContext = new InstanceContext(_contractCallback);
                _dualHttpBinding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
                _endpointAddress = new EndpointAddress(Settings.Default.ServiceAddress);
                _serviceClient = new DataServiceClient(_instanceContext, _dualHttpBinding, _endpointAddress);

                _serviceClient.Connect();

                LogStatus("Client running...");
                LogStatus("Press 'Enter' to exit this console...");
                System.Console.ReadLine();

                _serviceClient.Disconnect();
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        /// <summary>
        ///     Called when [executing script].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ExecuteScriptsEventArgs" /> instance containing the event data.</param>
        private static async void OnExecutingScript(object sender, ExecuteScriptsEventArgs e)
        {
            foreach (var script in e.Scripts)
            {
                try
                {
                    LogStatus($"Executing script {script.Name}...", true);

                    var scriptService = new ScriptService();
                    await scriptService.ExecuteAsync(script.Content);

                    LogStatus("Finished executing script", true);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);

                    // Using string interpolation or ex.ToString() will give us all necessary info
                    LogStatus($"Fail script execution: {ex}", true);
                }
            }
        }

        /// <summary>
        ///     Logs the status.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="broadcast">if set to <c>true</c> [broadcast].</param>
        private static void LogStatus(string message, bool broadcast = false)
        {
            System.Console.WriteLine($"{DateTime.Now} >> {message}");
            if (broadcast)
                _serviceClient.ReportStatus(message);
        }
    }
}
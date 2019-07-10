using System.ServiceModel;
using FountainBlue.Service.Core;

namespace FountainBlue.Service.Provider
{
    public class DataServiceClient : DuplexClientBase<IDataService>, IDataService
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DataServiceClient" /> class.
        /// </summary>
        /// <param name="callbackInstance">The callback instance.</param>
        /// <param name="binding">The binding.</param>
        /// <param name="endpointAddress">The endpoint address.</param>
        public DataServiceClient(InstanceContext callbackInstance, WSDualHttpBinding binding, EndpointAddress endpointAddress)
            : base(callbackInstance, binding, endpointAddress)
        {
        }

        /// <summary>
        ///     Connects this instance.
        /// </summary>
        public void Connect()
        {
            Channel.Connect();
        }

        /// <summary>
        ///     Loads the scripts.
        /// </summary>
        public void LoadScripts()
        {
            Channel.LoadScripts();
        }

        /// <summary>
        ///     Executes the script with the specified identifiers.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="ids">The identifiers.</param>
        public void ExecuteScript(ClientEndpoint client, params int[] ids)
        {
            Channel.ExecuteScript(client, ids);
        }

        /// <summary>
        ///     Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            Channel.Disconnect();
        }

        /// <summary>
        ///     Loads the clients.
        /// </summary>
        public void LoadClients()
        {
            Channel.LoadClients();
        }

        /// <summary>
        ///     Reports the status.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ReportStatus(string message)
        {
            Channel.ReportStatus(message);
        }
    }
}
using System.ServiceModel;

namespace FountainBlue.Service.Core
{
    [ServiceContract(CallbackContract = typeof(IContractCallback))]
    public interface IDataService
    {
        /// <summary>
        ///     Connects this instance.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Connect();

        /// <summary>
        ///     Loads the scripts.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void LoadScripts();

        /// <summary>
        ///     Loads the clients.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void LoadClients();

        /// <summary>
        ///     Reports the status.
        /// </summary>
        /// <param name="message">The message.</param>
        [OperationContract(IsOneWay = true)]
        void ReportStatus(string message);

        /// <summary>
        ///     Executes the script with the specified identifiers.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="ids">The identifiers.</param>
        [OperationContract(IsOneWay = true)]
        void ExecuteScript(ClientEndpoint client, params int[] ids);

        /// <summary>
        ///     Disconnects this instance.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void Disconnect();
    }
}
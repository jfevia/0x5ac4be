using System.Collections.Generic;
using System.ServiceModel;

namespace FountainBlue.Service.Core
{
    public interface IContractCallback
    {
        /// <summary>
        ///     Loads the scripts.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        [OperationContract(IsOneWay = true)]
        void LoadScripts(IEnumerable<ScriptItem> scripts);

        /// <summary>
        ///     Loads the clients.
        /// </summary>
        /// <param name="clients">The clients.</param>
        [OperationContract(IsOneWay = true)]
        void LoadClients(IEnumerable<ClientEndpoint> clients);

        /// <summary>
        ///     Executes the scripts.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        [OperationContract(IsOneWay = true)]
        void ExecuteScript(IEnumerable<ScriptFile> scripts);

        /// <summary>
        ///     Reports the status.
        /// </summary>
        /// <param name="status">The status.</param>
        [OperationContract(IsOneWay = true)]
        void ReportStatus(Status status);
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading;
using FountainBlue.Service.Core;

namespace FountainBlue.Service.Provider
{
    [CallbackBehavior(UseSynchronizationContext = false)]
    public class ContractCallback : IContractCallback
    {
        private readonly SynchronizationContext _syncContext = AsyncOperationManager.SynchronizationContext;

        /// <summary>
        ///     Loads the scripts.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        public void LoadScripts(IEnumerable<ScriptItem> scripts)
        {
            _syncContext.Post(OnLoadScripts, scripts);
        }

        /// <summary>
        ///     Loads the clients.
        /// </summary>
        /// <param name="clients">The clients.</param>
        public void LoadClients(IEnumerable<ClientEndpoint> clients)
        {
            _syncContext.Post(OnLoadClients, clients);
        }

        /// <summary>
        ///     Executes the scripts.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        public void ExecuteScript(IEnumerable<ScriptFile> scripts)
        {
            _syncContext.Post(OnExecuteScripts, scripts);
        }

        /// <summary>
        ///     Reports the status.
        /// </summary>
        /// <param name="status">The status.</param>
        public void ReportStatus(Status status)
        {
            _syncContext.Post(OnReportStatus, status);
        }

        /// <summary>
        ///     Called when [report status].
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="InvalidOperationException">Could not convert object state to Status</exception>
        private void OnReportStatus(object state)
        {
            if (!(state is Status status))
                throw new InvalidOperationException("Could not convert object state to Status");

            ReportingStatus?.Invoke(this, new StatusEventArgs(status));
        }

        /// <summary>
        ///     Called when [load scripts].
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="System.InvalidOperationException">Could not convert object state to IEnumerable{ScriptFile}</exception>
        private void OnLoadScripts(object state)
        {
            if (!(state is IEnumerable<ScriptItem> scripts))
                throw new InvalidOperationException("Could not convert object state to IEnumerable<ScriptFile>");

            LoadingScripts?.Invoke(this, new ScriptItemsEventArgs(scripts));
        }

        /// <summary>
        ///     Called when [execute script].
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="System.InvalidOperationException">Could not convert object state to IEnumerable{ScriptFile}</exception>
        private void OnExecuteScripts(object state)
        {
            if (!(state is IEnumerable<ScriptFile> scripts))
                throw new InvalidOperationException("Could not convert object state to IEnumerable<ScriptFile>");

            ExecutingScript?.Invoke(this, new ExecuteScriptsEventArgs(scripts));
        }

        /// <summary>
        ///     Called when [load clients].
        /// </summary>
        /// <param name="state">The state.</param>
        /// <exception cref="System.InvalidOperationException">Could not convert object state to IEnumerable{ClientEndpoint}</exception>
        private void OnLoadClients(object state)
        {
            if (!(state is IEnumerable<ClientEndpoint> clients))
                throw new InvalidOperationException("Could not convert object state to IEnumerable<ClientEndpoint>");

            LoadingClients?.Invoke(this, new ClientsEventArgs(clients));
        }

        /// <summary>
        ///     Occurs when [loading scripts].
        /// </summary>
        public event EventHandler<ScriptItemsEventArgs> LoadingScripts;

        /// <summary>
        ///     Occurs when [executing script].
        /// </summary>
        public event EventHandler<ExecuteScriptsEventArgs> ExecutingScript;

        /// <summary>
        ///     Occurs when [loading clients].
        /// </summary>
        public event EventHandler<ClientsEventArgs> LoadingClients;

        /// <summary>
        ///     Occurs when [reporting status].
        /// </summary>
        public event EventHandler<StatusEventArgs> ReportingStatus;
    }
}
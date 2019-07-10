using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using FountainBlue.Service.Core;

namespace FountainBlue.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DataService : IDataService
    {
        private readonly ConnectivityCache _connectivityCache = new ConnectivityCache();
        private readonly object _syncRoot = new object();

        /// <summary>
        ///     Initializes a new instance of the <see cref="DataService" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public DataService(ServiceSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        ///     Gets the settings.
        /// </summary>
        /// <value>
        ///     The settings.
        /// </value>
        public ServiceSettings Settings { get; }

        /// <summary>
        ///     Connects this instance.
        /// </summary>
        public void Connect()
        {
            var callbackChannel = OperationContext.Current.GetCallbackChannel<IContractCallback>();
            lock (_syncRoot)
            {
                if (_connectivityCache.Contains(callbackChannel))
                    return;

                var clientEndpoint = GetClientEndpoint();
                _connectivityCache.Add(clientEndpoint, callbackChannel);

                LoadClients();
                LoadScripts();
            }
        }

        /// <summary>
        ///     Loads the scripts.
        /// </summary>
        public void LoadScripts()
        {
            var callbackChannel = OperationContext.Current.GetCallbackChannel<IContractCallback>();
            lock (_syncRoot)
            {
                if (!_connectivityCache.Contains(callbackChannel))
                    return;

                var items = GetScripts().ToList();
                callbackChannel.LoadScripts(items);
            }
        }

        /// <summary>
        ///     Loads the clients.
        /// </summary>
        public void LoadClients()
        {
            lock (_syncRoot)
            {
                foreach (var callbackChannel in _connectivityCache.GetCallbackChannels())
                    callbackChannel.LoadClients(_connectivityCache.GetClients());
            }
        }

        /// <summary>
        ///     Reports the status.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ReportStatus(string message)
        {
            lock (_syncRoot)
            {
                var sourceCallbackChannel = OperationContext.Current.GetCallbackChannel<IContractCallback>();
                var client = _connectivityCache.GetClient(sourceCallbackChannel);
                var status = new Status
                {
                    Client = client,
                    Message = message
                };

                foreach (var callbackChannel in _connectivityCache.GetCallbackChannels())
                    callbackChannel.ReportStatus(status);
            }
        }

        /// <summary>
        ///     Executes the script with the specified identifiers.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="ids">The identifiers.</param>
        public void ExecuteScript(ClientEndpoint client, params int[] ids)
        {
            lock (_syncRoot)
            {
                var callbackChannel = _connectivityCache.GetCallbackChannel(client);
                var scripts = GetScriptsById(ids);
                callbackChannel.ExecuteScript(scripts);
            }
        }

        /// <summary>
        ///     Disconnects this instance.
        /// </summary>
        public void Disconnect()
        {
            lock (_syncRoot)
            {
                var callbackChannel = OperationContext.Current.GetCallbackChannel<IContractCallback>();
                _connectivityCache.Remove(callbackChannel);
            }
        }

        /// <summary>
        ///     Gets the remote endpoint.
        /// </summary>
        /// <returns>The remote endpoint.</returns>
        /// <exception cref="InvalidRemoteEndpointException">Could not find remote endpoint</exception>
        /// <exception cref="InvalidRemoteEndpointException">Remote endpoint cannot be null</exception>
        private ClientEndpoint GetClientEndpoint()
        {
            if (!OperationContext.Current.IncomingMessageProperties.ContainsKey(RemoteEndpointMessageProperty.Name))
                throw new InvalidRemoteEndpointException("Could not find remote endpoint");

            if (!(OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] is RemoteEndpointMessageProperty property))
                throw new InvalidRemoteEndpointException("Remote endpoint cannot be null");

            return property.ToClientEndpoint();
        }

        /// <summary>
        ///     Gets the scripts.
        /// </summary>
        /// <returns>The scripts.</returns>
        private IEnumerable<ScriptItem> GetScripts()
        {
            var lastId = 0;
            var scripts = Directory.GetFiles(Settings.ScriptsDirectory, "*.cs");
            return scripts.Select(s => new ScriptItem
            {
                Id = ++lastId,
                Name = Path.GetFileNameWithoutExtension(s),
                Path = s
            });
        }

        /// <summary>
        ///     Gets the scripts by identifiers.
        /// </summary>
        /// <param name="ids">The identifiers.</param>
        /// <returns>The scripts.</returns>
        private IEnumerable<ScriptFile> GetScriptsById(params int[] ids)
        {
            var scripts = GetScripts().Where(s => ids.Any(r => r == s.Id));
            foreach (var script in scripts)
                yield return new ScriptFile
                {
                    Id = script.Id,
                    Name = script.Name,
                    Path = script.Path,
                    Content = File.ReadAllText(script.Path)
                };
        }
    }
}
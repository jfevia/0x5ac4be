using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FountainBlue.Service.Core;

namespace FountainBlue.Service
{
    internal class ConnectivityCache
    {
        private readonly ConcurrentDictionary<ClientEndpoint, IContractCallback> _callbackChannelByClientEndpoint = new ConcurrentDictionary<ClientEndpoint, IContractCallback>();
        private readonly ConcurrentDictionary<IContractCallback, ClientEndpoint> _clientEndpointByCallbackChannel = new ConcurrentDictionary<IContractCallback, ClientEndpoint>();

        /// <summary>
        ///     Gets the clients.
        /// </summary>
        /// <returns>The clients.</returns>
        public IEnumerable<ClientEndpoint> GetClients()
        {
            return _callbackChannelByClientEndpoint.Keys;
        }

        /// <summary>
        ///     Gets the callback channels.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IContractCallback> GetCallbackChannels()
        {
            return _clientEndpointByCallbackChannel.Keys;
        }

        /// <summary>
        ///     Adds the specified client endpoint and its respective callback channel.
        /// </summary>
        /// <param name="clientEndpoint">The client endpoint.</param>
        /// <param name="callbackChannel">The callback channel.</param>
        public void Add(ClientEndpoint clientEndpoint, IContractCallback callbackChannel)
        {
            _callbackChannelByClientEndpoint.AddOrUpdate(clientEndpoint, callbackChannel, (key, value) => value);
            _clientEndpointByCallbackChannel.AddOrUpdate(callbackChannel, clientEndpoint, (key, value) => value);
        }

        /// <summary>
        ///     Determines whether this instance contains the object.
        /// </summary>
        /// <param name="callbackChannel">The callback channel.</param>
        /// <returns>
        ///     <c>true</c> if [contains] [the specified callback channel]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(IContractCallback callbackChannel)
        {
            return _clientEndpointByCallbackChannel.ContainsKey(callbackChannel);
        }

        /// <summary>
        ///     Removes the specified callback channel and its respective client endpoint.
        /// </summary>
        /// <param name="callbackChannel">The callback channel.</param>
        /// <exception cref="System.InvalidOperationException">Could not remove callback channel</exception>
        /// <exception cref="System.InvalidOperationException">Could not remove client endpoint</exception>
        public void Remove(IContractCallback callbackChannel)
        {
            if (!_clientEndpointByCallbackChannel.TryRemove(callbackChannel, out var clientEndpoint))
                throw new InvalidOperationException("Could not remove callback channel");

            if (!_callbackChannelByClientEndpoint.TryRemove(clientEndpoint, out _))
                throw new InvalidOperationException("Could not remove client endpoint");
        }

        /// <summary>
        ///     Gets the callback channel for the specified client endpoint.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <returns>The callback channel.</returns>
        /// <exception cref="FountainBlue.Service.ClientEndpointNotFoundException">Could not find client endpoint in cache</exception>
        public IContractCallback GetCallbackChannel(ClientEndpoint client)
        {
            if (!_callbackChannelByClientEndpoint.TryGetValue(client, out var callbackChannel))
                throw new ClientEndpointNotFoundException("Could not find client endpoint in cache");

            return callbackChannel;
        }

        /// <summary>
        ///     Gets the client for the specified callback channel..
        /// </summary>
        /// <param name="callbackChannel">The callback channel.</param>
        /// <returns>The client.</returns>
        /// <exception cref="FountainBlue.Service.CallbackChannelNotFoundException">Could not find callback channel in cache</exception>
        public ClientEndpoint GetClient(IContractCallback callbackChannel)
        {
            if (!_clientEndpointByCallbackChannel.TryGetValue(callbackChannel, out var client))
                throw new CallbackChannelNotFoundException("Could not find callback channel in cache");

            return client;
        }
    }
}
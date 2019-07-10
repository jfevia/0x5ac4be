using System;
using System.Collections.Generic;
using FountainBlue.Service.Core;

namespace FountainBlue.Service.Provider
{
    public class ClientsEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientsEventArgs" /> class.
        /// </summary>
        /// <param name="clients">The clients.</param>
        public ClientsEventArgs(IEnumerable<ClientEndpoint> clients)
        {
            Clients = clients;
        }

        /// <summary>
        ///     Gets the clients.
        /// </summary>
        /// <value>
        ///     The clients.
        /// </value>
        public IEnumerable<ClientEndpoint> Clients { get; }
    }
}
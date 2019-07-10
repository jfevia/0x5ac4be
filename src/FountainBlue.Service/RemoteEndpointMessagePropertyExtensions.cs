using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel.Channels;
using FountainBlue.Service.Core;

namespace FountainBlue.Service
{
    internal static class RemoteEndpointMessagePropertyExtensions
    {
        /// <summary>
        ///     Converts an instance of <see cref="RemoteEndpointMessageProperty" /> to its <see cref="ClientEndpoint" />
        ///     equivalent.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The client endpoint.</returns>
        public static ClientEndpoint ToClientEndpoint(this RemoteEndpointMessageProperty property)
        {
            var hostEntry = Dns.GetHostEntry(property.Address);
            var firstIPv4 = hostEntry.AddressList?.FirstOrDefault(s => s.AddressFamily == AddressFamily.InterNetwork);
            if (firstIPv4 == null)
                throw new InvalidOperationException("Could not find IPv4 address for endpoint address");

            return new ClientEndpoint {Address = property.Address, Port = property.Port};
        }
    }
}
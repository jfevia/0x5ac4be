using System.Runtime.Serialization;

namespace FountainBlue.Service.Core
{
    [DataContract]
    public class Status
    {
        /// <summary>
        ///     Gets or sets the client.
        /// </summary>
        /// <value>
        ///     The client.
        /// </value>
        [DataMember]
        public ClientEndpoint Client { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        [DataMember]
        public string Message { get; set; }
    }
}
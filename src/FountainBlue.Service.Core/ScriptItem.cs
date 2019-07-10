using System.Runtime.Serialization;

namespace FountainBlue.Service.Core
{
    [DataContract]
    public class ScriptItem
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the path.
        /// </summary>
        /// <value>
        ///     The path.
        /// </value>
        [DataMember]
        public string Path { get; set; }
    }
}
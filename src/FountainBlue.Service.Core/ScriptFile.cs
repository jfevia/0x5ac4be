using System.Runtime.Serialization;

namespace FountainBlue.Service.Core
{
    [DataContract]
    public class ScriptFile : ScriptItem
    {
        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        /// <value>
        ///     The content.
        /// </value>
        [DataMember]
        public string Content { get; set; }
    }
}
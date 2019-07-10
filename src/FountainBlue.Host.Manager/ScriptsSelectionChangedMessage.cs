using System.Collections.Generic;
using FountainBlue.Service.Core;

namespace FountainBlue.Host.Manager
{
    public class ScriptsSelectionChangedMessage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScriptsSelectionChangedMessage" /> class.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        public ScriptsSelectionChangedMessage(IEnumerable<ScriptItem> scripts)
        {
            Scripts = scripts;
        }

        /// <summary>
        ///     Gets the scripts.
        /// </summary>
        /// <value>
        ///     The scripts.
        /// </value>
        public IEnumerable<ScriptItem> Scripts { get; }
    }
}
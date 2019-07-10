using System;
using System.Collections.Generic;
using FountainBlue.Service.Core;

namespace FountainBlue.Service.Provider
{
    public class ExecuteScriptsEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExecuteScriptsEventArgs" /> class.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        public ExecuteScriptsEventArgs(IEnumerable<ScriptFile> scripts)
        {
            Scripts = scripts;
        }

        /// <summary>
        ///     Gets the scripts.
        /// </summary>
        /// <value>
        ///     The scripts.
        /// </value>
        public IEnumerable<ScriptFile> Scripts { get; }
    }
}
using System;
using System.Collections.Generic;
using FountainBlue.Service.Core;

namespace FountainBlue.Service.Provider
{
    public class ScriptItemsEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScriptItemsEventArgs" /> class.
        /// </summary>
        /// <param name="scripts">The scripts.</param>
        public ScriptItemsEventArgs(IEnumerable<ScriptItem> scripts)
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
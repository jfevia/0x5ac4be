using System;
using FountainBlue.Service.Core;

namespace FountainBlue.Service.Provider
{
    public class StatusEventArgs : EventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StatusEventArgs" /> class.
        /// </summary>
        /// <param name="status">The status.</param>
        public StatusEventArgs(Status status)
        {
            Status = status;
        }

        /// <summary>
        ///     Gets the status.
        /// </summary>
        /// <value>
        ///     The status.
        /// </value>
        public Status Status { get; }
    }
}
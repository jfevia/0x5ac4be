using System.Diagnostics;
using System.Threading.Tasks;
using FountainBlue.Scripting;

namespace FountainBlue.Scripts
{
    public class OpenGitHubScript : Script
    {
        private readonly string _address = "https://github.com/jfevia/0x5ac4be";

        /// <summary>
        ///     Opens the specified address in the default browser.
        /// </summary>
        /// <param name="address">The address.</param>
        protected virtual async Task OpenWebpageAsync(string address)
        {
            await Task.Run(() => Process.Start(address));
        }

        /// <summary>
        ///     Executes the instance asynchronously.
        /// </summary>
        public override async Task ExecuteAsync()
        {
            await OpenWebpageAsync(_address);
        }
    }
}

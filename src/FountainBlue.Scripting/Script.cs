using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FountainBlue.Core;

namespace FountainBlue.Scripting
{
    public abstract class Script : IScript
    {
        /// <summary>
        ///     Executes this instance asynchronously.
        /// </summary>
        /// <returns>The task.</returns>
        public abstract Task ExecuteAsync();

        /// <summary>
        ///     Selects the window.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="comparisonType">The type of comparison.</param>
        /// <returns>The task.</returns>
        protected virtual async Task SelectWindowAsync(string title, StringComparison comparisonType)
        {
            await Task.Run(() =>
            {
                var process = GetWindowProcessByTitle(title, comparisonType);
                Win32.SetForegroundWindow(process);
            });
        }

        /// <summary>
        ///     Gets the window process by the main window's title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="comparisonType">The type of comparison.</param>
        /// <returns>The process.</returns>
        /// <exception cref="ArgumentNullException">title</exception>
        /// <exception cref="Exception">Could not find window with title {title}</exception>
        private Process GetWindowProcessByTitle(string title, StringComparison comparisonType)
        {
            if (title == null) throw new ArgumentNullException(nameof(title));

            var processes = Process.GetProcesses();
            var process = processes.FirstOrDefault(s =>
                !string.IsNullOrWhiteSpace(s.MainWindowTitle) &&
                s.MainWindowTitle.IndexOf(title, comparisonType) > 0);

            if (process == null)
                throw new Exception($"Could not find window with title {title}");

            return process;
        }

        /// <summary>
        ///     Sets the cursor position, relative to the window.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="windowTitle">The window title.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>The task.</returns>
        protected virtual async Task SetCursorPositionRelativeToWindowAsync(Point point, string windowTitle, StringComparison comparisonType)
        {
            await Task.Run(() =>
            {
                var process = GetWindowProcessByTitle(windowTitle, comparisonType);
                var size = Win32.GetMainWindowSize(process);
                Win32.SetCursorPosition(point, size);
            });
        }

        /// <summary>
        ///     Performs a mouse left click on the specified point, relative to the window.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="windowTitle">The title of the window.</param>
        /// <param name="comparisonType">The type of comparison.</param>
        /// <returns>The task.</returns>
        protected virtual async Task ClickMouseRelativeToWindowAsync(Point point, string windowTitle, StringComparison comparisonType)
        {
            await Task.Run(() =>
            {
                var process = GetWindowProcessByTitle(windowTitle, comparisonType);
                var size = Win32.GetMainWindowSize(process);
                Win32.PerformLeftClick(point, size);
            });
        }

        /// <summary>
        ///     Sends the specified text.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The task.</returns>
        protected virtual async Task SendTextAsync(string value)
        {
            await Task.Run(() => SendKeys.SendWait(value));
        }

        /// <summary>
        ///     Launches the executable found in the specified path asynchronously.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The task.</returns>
        protected virtual async Task LaunchExeAsync(string path)
        {
            await Task.Run(() => Process.Start(path));
        }

        /// <summary>
        ///     Sleeps the thread for the specified milliseconds.
        /// </summary>
        /// <param name="delay">The delay.</param>
        /// <returns>The task.</returns>
        protected virtual async Task Sleep(TimeSpan delay)
        {
            await Task.Delay(delay);
        }
    }
}
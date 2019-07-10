using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FountainBlue.Core
{
    public static class Win32
    {
        /// <summary>
        ///     Sets the specified process' window as foreground.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns><see langword="true" /> if the action succeeds; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException">process</exception>
        public static bool SetForegroundWindow(Process process)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            return SetForegroundWindow(process.MainWindowHandle);
        }

        /// <summary>
        ///     Gets the size of the main window.
        /// </summary>
        /// <param name="process">The process.</param>
        /// <returns>The size.</returns>
        /// <exception cref="Exception">Could not get main window size</exception>
        public static Rectangle GetMainWindowSize(Process process)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            var rect = new Rect();
            if (!GetMainWindowSize(process.MainWindowHandle, ref rect))
                throw new Exception("Could not get main window size");

            return new Rectangle {X = rect.Left, Y = rect.Top, Height = rect.Bottom - rect.Top, Width = rect.Right - rect.Left};
        }

        /// <summary>
        /// Sets the cursor position. If the <paramref name="area" /> is specified, the position will be relative to its boundaries.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="area">The area.</param>
        public static void SetCursorPosition(Point point, Rectangle? area = null)
        {
            if (area != null)
                point = point.RelativeTo(area.Value);

            Cursor.Position = point;
        }

        /// <summary>
        ///     Performs a left click. If the <paramref name="area" /> is specified, the click will be relative to its boundaries.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="area">The area.</param>
        public static void PerformLeftClick(Point point, Rectangle? area = null)
        {
            if (area != null)
                point = point.RelativeTo(area.Value);

            MouseEvent((uint) (MouseEvents.LeftButtonDown | MouseEvents.LeftButtonUp), (uint) point.X, (uint) point.Y, 0, 0);
        }

        #region P/Invoke

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowRect")]
        private static extern bool GetMainWindowSize(IntPtr hWnd, ref Rect rectangle);

        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        private static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        [Flags]
        private enum MouseEvents
        {
            LeftButtonDown = 0x02,
            LeftButtonUp = 0x04
        }

        private struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        #endregion
    }
}
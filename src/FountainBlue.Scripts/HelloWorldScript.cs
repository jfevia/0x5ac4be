using System;
using System.Drawing;
using System.Threading.Tasks;
using FountainBlue.Scripting;

namespace FountainBlue.Scripts
{
    public class HelloWorldScript : Script
    {
        private const string NotepadExeFilePath = "notepad.exe";
        private const string NotepadTitle = "Untitled - Notepad";
        private const string Text = "Hello World!";
        private readonly TimeSpan Delay = TimeSpan.FromSeconds(3);
        private readonly Point CursorPosition = new Point(50, 100);

        /// <summary>
        ///     Executes this instance asynchronously.
        /// </summary>
        public override async Task ExecuteAsync()
        {
            await LaunchExeAsync(NotepadExeFilePath);
            await Sleep(Delay);
            await SelectWindowAsync(NotepadTitle, StringComparison.OrdinalIgnoreCase);
            await Sleep(Delay);
            await SetCursorPositionRelativeToWindowAsync(CursorPosition, NotepadTitle, StringComparison.OrdinalIgnoreCase);
            await Sleep(Delay);
            await ClickMouseRelativeToWindowAsync(CursorPosition, NotepadTitle, StringComparison.OrdinalIgnoreCase);
            await Sleep(Delay);
            await SendTextAsync(Text);
        }
    }
}

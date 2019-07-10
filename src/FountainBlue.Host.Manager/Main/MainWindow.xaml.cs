using System.Linq;
using System.Windows.Controls;
using FountainBlue.Service.Core;
using GalaSoft.MvvmLight.Messaging;

namespace FountainBlue.Host.Manager.Main
{
    public partial class MainWindow
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Handles the SelectionChanged event of the ListBoxScripts control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">
        ///     The <see cref="System.Windows.Controls.SelectionChangedEventArgs" /> instance containing the event
        ///     data.
        /// </param>
        private void ListBoxScripts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ListBox listBoxScripts))
                return;

            Messenger.Default.Send(new ScriptsSelectionChangedMessage(listBoxScripts.SelectedItems.OfType<ScriptItem>()));
        }
    }
}